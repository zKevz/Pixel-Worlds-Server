using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PixelWorldsServer.DataAccess;
using PixelWorldsServer.Server.Event;
using PixelWorldsServer.Server.Network;
using PixelWorldsServer.Server.Players;
using PixelWorldsServer.Server.Worlds;

namespace PixelWorldsServer.Server;

public class ServerHostedService : IHostedService
{
    private readonly TcpServer m_TcpServer;
    private readonly WorldManager m_WorldManager;
    private readonly EventManager m_EventManager;
    private readonly PlayerManager m_PlayerManager;

    private readonly ILogger m_Logger;
    private readonly IOptions<DatabaseSettings> m_DatabaseSettings;

    public ServerHostedService(TcpServer tcpServer, WorldManager worldManager, EventManager eventManager, PlayerManager playerManager, ILogger<ServerHostedService> logger, IOptions<DatabaseSettings> databaseSettings)
    {
        m_Logger = logger;
        m_TcpServer = tcpServer;
        m_EventManager = eventManager;
        m_WorldManager = worldManager;
        m_PlayerManager = playerManager;
        m_DatabaseSettings = databaseSettings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (m_DatabaseSettings.Value.ConnectionString is null)
        {
            m_Logger.LogError("Connection string is null! Did you set it in appsettings.json?");
            return;
        }

        m_Logger.LogInformation("Loading important worlds..");

        for (int i = 1; i <= 12; ++i)
        {
            // I could just copy existing world instead of loading it every fucking time but idc
            await m_WorldManager
                .LoadWorldFromBinaryAsync($"TUTORIAL{i}", "Data/tutorial-world.bin")
                .ConfigureAwait(false);
        }

        m_Logger.LogInformation("Loaded {} worlds", m_WorldManager.GetWorldsCount());

        var eventTask = m_EventManager.StartAsync(cancellationToken);
        var serverTask = m_TcpServer.StartAsync(cancellationToken);
        await Task.WhenAll(eventTask, serverTask).ConfigureAwait(false); // I think i should use WhenAny instead of waiting for both tasks?
    }

    public async Task StopAsync(CancellationToken _)
    {
        m_TcpServer.Stop();
        m_Logger.LogInformation("Saving all players and worlds...");

        var tasks = new Task[]
        {
            m_WorldManager.SaveAllWorldsAsync(),
            m_PlayerManager.SaveAllPlayersAsync()
        };

        await Task.WhenAll(tasks).ConfigureAwait(false);

        m_Logger.LogInformation("All players and worlds are saved!");
    }
}
