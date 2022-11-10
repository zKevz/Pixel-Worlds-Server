using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using PixelWorldsServer.Server.Players;
using PixelWorldsServer.Server.Worlds;
using System.Collections.Concurrent;
using System.Reflection;

namespace PixelWorldsServer.Server.Event;

public class EventContext
{
    public World? World { get; set; }
    public Player Player { get; set; } = null!;
}

public class IncomingPacket
{
    public Player Player { get; set; } = null!;
    public BsonDocument Document { get; set; } = null!;
    public AutoResetEvent AutoResetEvent { get; set; } = null!;
}

public class EventManager
{
    private readonly ILogger m_Logger;
    private readonly EventHandler m_EventHandler;
    private readonly ConcurrentQueue<IncomingPacket> m_PacketQueue = new();
    private readonly Dictionary<string, MethodInfo> m_RegisteredEvents = new();

    public EventManager(ILogger<EventManager> logger, EventHandler eventHandler)
    {
        m_Logger = logger;
        m_EventHandler = eventHandler;

        Initialize();
    }

    private void Initialize()
    {
        m_Logger.LogInformation("Registering events...");

        foreach (var method in m_EventHandler.GetType().GetMethods())
        {
            var attributes = method.GetCustomAttributes<EventAttribute>();
            foreach (var attribute in attributes)
            {
                m_RegisteredEvents.Add(attribute.Id, method);
            }
        }

        m_Logger.LogInformation("Registered {} events", m_RegisteredEvents.Count);
    }

    public void QueuePacket(BsonDocument document, Player player, AutoResetEvent autoResetEvent)
    {
        m_PacketQueue.Enqueue(new IncomingPacket()
        {
            Player = player,
            Document = document,
            AutoResetEvent = autoResetEvent
        });
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        m_Logger.LogInformation("Event manager is running!");
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!m_PacketQueue.TryDequeue(out var incomingPacket))
            {
                Thread.Sleep(1);
                continue;
            }

            try
            {
                var realTask = InvokeAsync(incomingPacket.Document, incomingPacket.Player);
                var delayTask = Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                var task = await Task.WhenAny(realTask, delayTask);
                if (task == delayTask)
                {
                    m_Logger.LogCritical("Event takes more than 10 seconds!!!");
                }
            }
            catch (Exception exception)
            {
                m_Logger.LogError("Exception: {}", exception);
            }
            finally
            {
                incomingPacket.AutoResetEvent.Set();
            }
        }
    }

    private async Task InvokeAsync(BsonDocument document, Player player)
    {
        string id = document["ID"].AsString;
        if (!m_RegisteredEvents.TryGetValue(id, out var method))
        {
            m_Logger.LogWarning("Unhandled packet {}", document.ToString());
            return;
        }

        var context = new EventContext()
        {
            World = player.World,
            Player = player
        };

        var methodParameters = method.GetParameters();
        object[] parameters;
        if (methodParameters.Length > 1)
        {
            var parameter = methodParameters[1];
            var serialized = BsonSerializer.Deserialize(document, parameter.ParameterType);
            parameters = new[] { context, serialized };
        }
        else
        {
            parameters = new[] { context };
        }

        try
        {
            var task = (Task)method.Invoke(m_EventHandler, parameters)!;
            await task.ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            m_Logger.LogError("Exception: {}", exception);
        }
    }
}
