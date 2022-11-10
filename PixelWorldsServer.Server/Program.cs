using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using PixelWorldsServer.DataAccess;
using PixelWorldsServer.DataAccess.Models;
using PixelWorldsServer.Protocol.Packet.Response;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Server;
using PixelWorldsServer.Server.Event;
using PixelWorldsServer.Server.Network;
using PixelWorldsServer.Server.Players;
using PixelWorldsServer.Server.Worlds;
using EventHandler = PixelWorldsServer.Server.Event.EventHandler;

static bool Init(ILogger logger)
{
    BsonSerializer.RegisterSerializationProvider(new WorldItemDataSerializationProvider());
    BsonSerializer.RegisterSerializationProvider(new GetWorldResponseSerializationProvider());

    logger.LogInformation("Loading blocks config...");

    var start = DateTime.Now;
    if (!ConfigData.Init())
    {
        logger.LogError("Failed to load blocks config!");
        return false;
    }

    logger.LogInformation("Blocks config successfully loaded in {} milliseconds", (DateTime.Now - start).TotalMilliseconds);
    return true;
}

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddLogging(logger =>
            {
                logger.ClearProviders();
                logger.SetMinimumLevel(LogLevel.Debug);
                logger.AddLog4Net("log4net.config");
            })
            .Configure<DatabaseSettings>(hostContext.Configuration.GetSection("Database"))
            .AddSingleton<Database>()
            .AddSingleton<TcpServer>()
            .AddSingleton<EventManager>()
            .AddSingleton<EventHandler>()
            .AddSingleton<PlayerManager>()
            .AddSingleton<WorldManager>()
            .AddHostedService<ServerHostedService>();
    })
    .Build();

var logger = host.Services.GetService<ILogger<ServerHostedService>>()!;
logger.LogInformation("Pixel Worlds Server v0.0.1");
logger.LogInformation("Project created by kevz#2211");

if (!Init(logger))
{
    return;
}

await host.RunAsync().ConfigureAwait(false);
