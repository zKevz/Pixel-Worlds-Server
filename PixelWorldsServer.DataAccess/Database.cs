using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PixelWorldsServer.DataAccess.Models;

namespace PixelWorldsServer.DataAccess;

public class Database
{
    private readonly ILogger m_Logger;
    private readonly IMongoClient m_MongoClient;
    private readonly IMongoDatabase m_MongoDatabase;
    private readonly IMongoCollection<WorldModel> m_WorldsCollection;
    private readonly IMongoCollection<PlayerModel> m_PlayersCollection;
    private readonly IOptions<DatabaseSettings> m_DatabaseSettings;

    public Database(ILogger<Database> logger, IOptions<DatabaseSettings> databaseSettings)
    {
        m_Logger = logger;
        m_Logger.LogInformation("Connecting to MongoDB...");
        m_DatabaseSettings = databaseSettings;

        m_MongoClient = new MongoClient(m_DatabaseSettings.Value.ConnectionString);
        m_MongoDatabase = m_MongoClient.GetDatabase(m_DatabaseSettings.Value.DatabaseName);
        m_WorldsCollection = m_MongoDatabase.GetCollection<WorldModel>(m_DatabaseSettings.Value.WorldsCollectionName);
        m_PlayersCollection = m_MongoDatabase.GetCollection<PlayerModel>(m_DatabaseSettings.Value.PlayersCollectionName);
        m_Logger.LogInformation("Successfully connected to MongoDB!");
    }

    public async Task<PlayerModel> GetPlayerByIdAsync(string id)
    {
        var players = await m_PlayersCollection.FindAsync(x => x.Id == id).ConfigureAwait(false);
        return await players.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<PlayerModel> GetPlayerByNameAsync(string name)
    {
        var players = await m_PlayersCollection.FindAsync(x => x.Name == name).ConfigureAwait(false);
        return await players.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<PlayerModel> CreatePlayerAsync(string ip, string os)
    {
        PlayerModel playerModel = new()
        {
            IP = ip,
            OS = os,
            Skin = 7,
            Gems = 250,
            Level = 1,
            Slots = 20,
            CountryCode = 999,
            CameraZoomValue = 0.25f
        };

        await m_PlayersCollection.InsertOneAsync(playerModel).ConfigureAwait(false);
        playerModel.Name = $"SUBJECT_{playerModel.Id.ToUpper()}";

        await UpdatePlayerNameAsync(playerModel.Id, playerModel.Name).ConfigureAwait(false);
        return playerModel;
    }

    public async Task UpdatePlayerNameAsync(string id, string name)
    {
        var update = Builders<PlayerModel>.Update.Set(x => x.Name, name);
        await m_PlayersCollection.UpdateOneAsync(x => x.Id == id, update).ConfigureAwait(false);
    }

    public async Task SavePlayerAsync(PlayerModel playerModel)
    {
        await m_PlayersCollection.ReplaceOneAsync(x => x.Id == playerModel.Id, playerModel).ConfigureAwait(false);
    }

    public async Task<WorldModel> GetWorldByNameAsync(string name)
    {
        var worlds = await m_WorldsCollection.FindAsync(x => x.Name == name).ConfigureAwait(false);
        return await worlds.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<string> InsertWorldAsync(WorldModel worldModel)
    {
        await m_WorldsCollection.InsertOneAsync(worldModel).ConfigureAwait(false);
        return worldModel.Id;
    }

    public async Task SaveWorldAsync(WorldModel worldModel)
    {
        await m_WorldsCollection.ReplaceOneAsync(x => x.Id == worldModel.Id, worldModel).ConfigureAwait(false);
    }
}
