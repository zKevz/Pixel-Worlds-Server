using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using PixelWorldsServer.DataAccess;
using PixelWorldsServer.DataAccess.Models;
using PixelWorldsServer.Protocol.Packet;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Server.Event;
using PixelWorldsServer.Server.Players;
using System.Net;
using System.Net.Sockets;

namespace PixelWorldsServer.Server.Network;

public class TcpServer
{
    private readonly ILogger m_Logger;
    private readonly Database m_Database;
    private readonly TcpListener m_TcpListener;
    private readonly EventManager m_EventManager;
    private readonly PlayerManager m_PlayerManager;

    public TcpServer(ILogger<TcpServer> logger, EventManager eventManager, Database database, PlayerManager playerManager)
    {
        m_Logger = logger;
        m_Database = database;
        m_TcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, 10001));
        m_TcpListener.Server.ReceiveTimeout = 1000;
        m_EventManager = eventManager;
        m_PlayerManager = playerManager;
    }

    public async Task HandleConnectionAsync(TcpClient client, Player player, CancellationToken token)
    {
        var stream = client.GetStream();
        var autoResetEvent = new AutoResetEvent(false);

        int packetDataLength = 0;
        byte[]? packetData = null;
        byte[] buffer = new byte[1024];

        while (!token.IsCancellationRequested)
        {
            try
            {
                int bytesRead = await stream.ReadAsync(buffer, token).ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    return;
                }

                // If packet data is null
                if (packetData is null)
                {
                    if (bytesRead < sizeof(int))
                    {
                        m_Logger.LogWarning("Client sends packet less than 4 bytes long");
                        return; // Too less packet
                    }

                    packetDataLength = BitConverter.ToInt32(buffer);
                    if (packetDataLength > 1024 * 64) // 64 KB
                    {
                        m_Logger.LogWarning("Client sends packet more than 64KB long");
                        return; // Too long packet
                    }

                    packetData = new byte[bytesRead - sizeof(int)]; // skip the length
                    packetDataLength -= 4;
                    Array.Copy(buffer, 4, packetData, 0, packetData.Length);
                }
                else
                {
                    int offset = packetData.Length;
                    Array.Resize(ref packetData, packetData.Length + buffer.Length);
                    Array.Copy(buffer, 0, packetData, offset, buffer.Length);
                }

                if (packetData.Length < packetDataLength)
                {
                    continue; // Waiting for more data..
                }

                var document = BsonSerializer.Deserialize<BsonDocument>(packetData);
                var messageCount = document["mc"].AsInt32;
                if (messageCount == 1 && document[NetStrings.FIRST_MESSAGE_KEY][NetStrings.ID_KEY].AsString == NetStrings.PING_KEY)
                {
                    player.SendPacket(new PacketBase()
                    {
                        ID = NetStrings.PING_KEY
                    });
                }
                else
                {
                    for (int i = 0; i < messageCount; ++i)
                    {
                        var message = document[$"m{i}"].AsBsonDocument;
                        m_EventManager.QueuePacket(message, player, autoResetEvent);
                    }

                    if (messageCount > 0)
                    {
                        // If the handler takes more than 100 ms then just send
                        // the client empty message so that the client doesn't die
                        if (!autoResetEvent.WaitOne(TimeSpan.FromMilliseconds(100)))
                        {
                            m_Logger.LogWarning("Event takes more than 100 milliseconds: {}", document);
                        }
                    }
                }

                if (player.IsDisconnected())
                {
                    return;
                }

                await SendRespondAsync(player, stream, token).ConfigureAwait(false);

                packetData = null;
                packetDataLength = 0;
            }
            catch (Exception exception)
            {
                m_Logger.LogError("Exception occured: {}", exception);
                return;
            }
        }
    }

    private static async Task SendRespondAsync(Player player, NetworkStream stream, CancellationToken token)
    {
        var responseDocument = player.ConsumePackets();
        var responseBytes = responseDocument.ToBson();

        /*if (responseDocument["mc"] > 0)
        {
            m_Logger.LogInformation("Sending packet: {}", responseDocument);
        }*/

        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        bw.Write(responseBytes.Length + sizeof(int));
        bw.Write(responseBytes);

        await stream.WriteAsync(ms.ToArray(), token).ConfigureAwait(false);
        await stream.FlushAsync(token).ConfigureAwait(false);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        m_TcpListener.Start();
        m_Logger.LogInformation("Server is running!");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var client = await m_TcpListener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
                var endPoint = client.Client.LocalEndPoint;
                if (endPoint is null)
                {
                    client.Close();
                    continue;
                }

                if (endPoint is not IPEndPoint iPEndPoint)
                {
                    client.Close();
                    continue;
                }

                m_Logger.LogInformation("Client {} connected", endPoint);

                var player = m_PlayerManager.AddOrReplacePlayer(iPEndPoint);
                var _ = HandleConnectionAsync(client, player, cancellationToken).ContinueWith(async x =>
                {
                    if (!m_PlayerManager.RemovePlayer(iPEndPoint))
                    {
                        m_Logger.LogWarning("Failed to remove player with address: {}", iPEndPoint);
                    }

                    m_Logger.LogInformation("Client {} disconnected", endPoint);

                    if (player.World is not null && !player.World.RemovePlayer(player))
                    {
                        m_Logger.LogWarning("Failed to remove player with address: {} from world", iPEndPoint);
                    }

                    await m_Database.SavePlayerAsync(PlayerModel.CreateCopy(player)).ConfigureAwait(false);

                    client.Close();
                    client.Dispose();
                }, cancellationToken);
            }
            catch (Exception) when (cancellationToken.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Server crashed, exception: {}", ex);
            }
        }
    }

    public void Stop()
    {
        m_Logger.LogInformation("Stopping server..");
        m_TcpListener.Stop();
    }
}
