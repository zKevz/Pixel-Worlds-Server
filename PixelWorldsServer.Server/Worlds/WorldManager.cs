using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using PixelWorldsServer.DataAccess;
using PixelWorldsServer.DataAccess.Models;
using PixelWorldsServer.Protocol;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Protocol.Worlds;

namespace PixelWorldsServer.Server.Worlds;

public class WorldManager
{
    private readonly ILogger m_Logger;
    private readonly Database m_Database;
    private readonly Dictionary<string, World> m_Worlds = new();

    public WorldManager(ILogger<WorldManager> logger, Database database)
    {
        m_Logger = logger;
        m_Database = database;
    }

    public int GetWorldsCount()
    {
        return m_Worlds.Count;
    }

    public async Task<World?> GetWorldAsync(string name, WorldLayoutType worldLayoutType = WorldLayoutType.Basic)
    {
        if (m_Worlds.TryGetValue(name, out World? world))
        {
            return world;
        }

        var worldModel = await m_Database.GetWorldByNameAsync(name).ConfigureAwait(false);
        if (worldModel is not null)
        {
            world = new World();
            world.LoadCopy(worldModel);

            m_Worlds.Add(name, world);

            return world;
        }

        return await GenerateWorldAsync(name, worldLayoutType).ConfigureAwait(false);
    }

    public async Task<World> GenerateWorldAsync(string name, WorldLayoutType worldLayoutType)
    {
        var world = new World();
        world.Init(name, worldLayoutType);
        world.Id = await m_Database.InsertWorldAsync(WorldModel.CreateCopy(world)).ConfigureAwait(false);
        return world;
    }

    public World? CopyTutorialWorld(string name)
    {
        if (!m_Worlds.TryGetValue(name, out World? world))
        {
            return null;
        }

        return new()
        {
            Id = $"DYNAMIC_WORLD_{world.Id}",
            Name = $"DYNAMIC_WORLD_{name}",

            ItemId = world.ItemId,
            MusicIndex = world.MusicIndex,
            InventoryId = world.InventoryId,

            Size = world.Size,
            StartingPoint = world.StartingPoint,

            WeatherType = world.WeatherType,
            GravityMode = world.GravityMode,
            LightingType = world.LightingType,
            LayoutType = world.LayoutType,
            LayerBackgroundType = world.LayerBackgroundType,

            BlockLayer = new(world.BlockLayer),
            PlantedSeeds = new(world.PlantedSeeds),
            ItemDatas = new(world.ItemDatas),
            BlockWaterLayer = new(world.BlockWaterLayer),
            BlockWiringLayer = new(world.BlockWiringLayer),
            Collectables = new(world.Collectables),
            BlockBackgroundLayer = new(world.BlockBackgroundLayer),
        };
    }

    public async Task<World?> GetExistingWorldAsync(string name)
    {
        if (m_Worlds.TryGetValue(name, out World? world))
        {
            return world;
        }

        var worldModel = await m_Database.GetWorldByNameAsync(name).ConfigureAwait(false);
        if (worldModel is not null)
        {
            world = new World();
            world.LoadCopy(worldModel);

            m_Worlds.Add(name, world);

            return world;
        }

        return null;
    }
    private static void GenerateLayerFromByteArray(WorldModel worldModel, byte[] data, LayerType layerType)
    {
        short[] array = new short[data.Length / 2];
        Buffer.BlockCopy(data, 0, array, 0, data.Length);

        for (int i = 0; i < worldModel.Size.X; ++i)
        {
            for (int j = 0; j < worldModel.Size.Y; ++j)
            {
                var index = i + j * worldModel.Size.X;
                var blockType = (BlockType)array[index];

                switch (layerType)
                {
                    case LayerType.Block:
                    {
                        worldModel.BlockLayer[index].BlockType = blockType;
                        break;
                    }

                    case LayerType.Water:
                    {
                        worldModel.BlockWaterLayer[index].BlockType = blockType;
                        break;
                    }

                    case LayerType.Background:
                    {
                        worldModel.BlockBackgroundLayer[index].BlockType = blockType;
                        break;
                    }

                    case LayerType.Wiring:
                    {
                        worldModel.BlockWiringLayer[index].BlockType = blockType;
                        break;
                    }
                }
            }
        }
    }

    private static void GenerateLayerHitsFromByteIntList(WorldModel worldModel, BsonArray array, LayerType layerType)
    {
        for (int i = 0; i < worldModel.Size.X; ++i)
        {
            for (int j = 0; j < worldModel.Size.Y; ++j)
            {
                var index = i + j * worldModel.Size.X;
                var hitsRequired = array[index].AsInt32;

                switch (layerType)
                {
                    case LayerType.Block:
                    {
                        worldModel.BlockLayer[index].HitsRequired = hitsRequired;
                        break;
                    }

                    case LayerType.Water:
                    {
                        worldModel.BlockWaterLayer[index].HitsRequired = hitsRequired;
                        break;
                    }

                    case LayerType.Background:
                    {
                        worldModel.BlockBackgroundLayer[index].HitsRequired = hitsRequired;
                        break;
                    }

                    case LayerType.Wiring:
                    {
                        worldModel.BlockWiringLayer[index].HitsRequired = hitsRequired;
                        break;
                    }
                }
            }
        }
    }

    private static void GenerateLayerHitBuffersFromByteIntList(WorldModel worldModel, BsonArray array, LayerType layerType)
    {
        for (int i = 0; i < worldModel.Size.X; ++i)
        {
            for (int j = 0; j < worldModel.Size.Y; ++j)
            {
                var index = i + j * worldModel.Size.X;
                var hitsRequired = array[index].AsInt32;

                switch (layerType)
                {
                    case LayerType.Block:
                    {
                        worldModel.BlockLayer[index].HitBuffer = hitsRequired;
                        break;
                    }

                    case LayerType.Water:
                    {
                        worldModel.BlockWaterLayer[index].HitBuffer = hitsRequired;
                        break;
                    }

                    case LayerType.Background:
                    {
                        worldModel.BlockBackgroundLayer[index].HitBuffer = hitsRequired;
                        break;
                    }

                    case LayerType.Wiring:
                    {
                        worldModel.BlockWiringLayer[index].HitBuffer = hitsRequired;
                        break;
                    }
                }
            }
        }
    }

    public async Task<World?> LoadWorldFromBinaryAsync(string name, string path)
    {
        m_Logger.LogInformation("Loading world {}..", name);

        if (!File.Exists(path))
        {
            m_Logger.LogWarning("Skipping world {} because path {} doesn't exist", name, path);
            return null;
        }

        var world = new World();

        {
            var worldModelDb = await m_Database.GetWorldByNameAsync(name).ConfigureAwait(false);
            if (worldModelDb is not null)
            {
                world.LoadCopy(worldModelDb);

                m_Worlds.Add(name, world);
                return world;
            }
        }

        using var fs = File.OpenRead(path);
        var data = LZMATools.Decompress(fs);
        var document = BsonSerializer.Deserialize<BsonDocument>(data);
        var worldModel = new WorldModel()
        {
            Name = name,
            ItemId = document["WorldItemId"].AsInt32,
            MusicIndex = document["WorldMusicIndex"]["Count"].AsInt32,
            InventoryId = document["InventoryId"].AsInt32,
            Size = new Vector2i()
            {
                X = document["WorldSizeSettingsType"]["WorldSizeX"].AsInt32,
                Y = document["WorldSizeSettingsType"]["WorldSizeY"].AsInt32
            },
            StartingPoint = new Vector2i()
            {
                X = document["WorldStartPoint"]["x"].AsInt32,
                Y = document["WorldStartPoint"]["y"].AsInt32
            },
            LayoutType = (WorldLayoutType)document["WorldLayoutType"]["Count"].AsInt32,
            GravityMode = (GravityMode)document["WorldGravityMode"]["GM"].AsInt32,
            WeatherType = (WeatherType)document["WorldWeatherType"]["Count"].AsInt32,
            LightingType = (LightingType)document["WorldLightingType"]["Count"].AsInt32,
            LayerBackgroundType = (LayerBackgroundType)document["WorldBackgroundType"]["Count"].AsInt32,
        };

        int size = worldModel.Size.X * worldModel.Size.Y;
        for (int i = 0; i < size; ++i)
        {
            worldModel.ItemDatas.Add(null);
            worldModel.BlockLayer.Add(new LayerBlock());
            worldModel.PlantedSeeds.Add(null);
            worldModel.BlockWaterLayer.Add(new LayerBlock());
            worldModel.BlockWiringLayer.Add(new LayerWiring());
            worldModel.BlockBackgroundLayer.Add(new LayerBlockBackground());
        }

        foreach (var element in document["WorldItems"].AsBsonDocument)
        {
            string[] arrays = element.Name.Split(' ');
            var x = int.Parse(arrays[1]);
            var y = int.Parse(arrays[2]);

            var childDocument = element.Value.AsBsonDocument;
            var className = childDocument["class"].AsString;
            childDocument.Remove("class");

            var blockType = WorldItemBase.GetBlockTypeViaClassName(className);
            var worldItemType = DataFactory.GetDataTypeForEnum(blockType);
            var worldItemBase = (WorldItemBase)BsonSerializer.Deserialize(childDocument, worldItemType);
            worldModel.ItemDatas[x + y * worldModel.Size.X] = worldItemBase;
        }

        GenerateLayerFromByteArray(worldModel, document["BlockLayer"].AsByteArray, LayerType.Block);
        GenerateLayerFromByteArray(worldModel, document["BackgroundLayer"].AsByteArray, LayerType.Background);
        GenerateLayerFromByteArray(worldModel, document["WaterLayer"].AsByteArray, LayerType.Water);
        GenerateLayerFromByteArray(worldModel, document["WiringLayer"].AsByteArray, LayerType.Wiring);

        GenerateLayerHitsFromByteIntList(worldModel, document["BlockLayerHits"].AsBsonArray, LayerType.Block);
        GenerateLayerHitsFromByteIntList(worldModel, document["BackgroundLayerHits"].AsBsonArray, LayerType.Background);
        GenerateLayerHitsFromByteIntList(worldModel, document["WaterLayerHits"].AsBsonArray, LayerType.Water);
        GenerateLayerHitsFromByteIntList(worldModel, document["WiringLayerHits"].AsBsonArray, LayerType.Wiring);

        GenerateLayerHitBuffersFromByteIntList(worldModel, document["BlockLayerHitBuffers"].AsBsonArray, LayerType.Block);
        GenerateLayerHitBuffersFromByteIntList(worldModel, document["BackgroundLayerHitBuffers"].AsBsonArray, LayerType.Background);
        GenerateLayerHitBuffersFromByteIntList(worldModel, document["WaterLayerHitBuffers"].AsBsonArray, LayerType.Water);
        GenerateLayerHitBuffersFromByteIntList(worldModel, document["WiringLayerHitBuffers"].AsBsonArray, LayerType.Wiring);

        world.LoadCopy(worldModel);
        m_Worlds.Add(name, world);

        await m_Database.InsertWorldAsync(worldModel).ConfigureAwait(false);

        return world;
    }

    public async Task SaveAllWorldsAsync()
    {
        var tasks = new List<Task>();

        foreach (var (_, world) in m_Worlds)
        {
            // to boost the performance!
            tasks.Add(m_Database.SaveWorldAsync(WorldModel.CreateCopy(world)));
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
