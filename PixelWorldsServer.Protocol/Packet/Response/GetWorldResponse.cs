using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Protocol.Worlds;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class GetWorldResponse : PacketBase
{
    public string WorldId { get; set; } = string.Empty;

    public int ItemId { get; set; } // WorldItem last id
    public int MusicIndex { get; set; }
    public int InventoryId { get; set; } // Collectables last id

    public Vector2i Size { get; set; } = new();
    public Vector2i StartingPoint { get; set; } = new();

    public WeatherType WeatherType { get; set; } = WeatherType.None;
    public GravityMode GravityMode { get; set; } = GravityMode.Normal;
    public LightingType LightingType { get; set; } = LightingType.None;
    public WorldLayoutType LayoutType { get; set; } = WorldLayoutType.Basic;
    public LayerBackgroundType LayerBackgroundType { get; set; } = LayerBackgroundType.ForestBackground;

    public List<LayerBlock> BlockLayer { get; set; } = new();
    public List<SeedData?> PlantedSeeds { get; set; } = new();
    public List<WorldItemBase?> ItemDatas { get; set; } = new();
    public List<LayerBlock> BlockWaterLayer { get; set; } = new();
    public List<LayerWiring> BlockWiringLayer { get; set; } = new();
    public List<CollectableData> Collectables { get; set; } = new();
    public List<LayerBlockBackground> BlockBackgroundLayer { get; set; } = new();
}

public class GetWorldResponseSerializer : SerializerBase<GetWorldResponse>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, GetWorldResponse value)
    {
        context.Writer.WriteStartDocument();
        context.Writer.WriteName("ID");
        context.Writer.WriteString(value.ID);
        context.Writer.WriteName("World");
        context.Writer.WriteString(value.WorldId);

        {
            context.Writer.WriteName("WorldLayoutType");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            context.Writer.WriteInt32((int)value.LayoutType);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldBackgroundType");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            context.Writer.WriteInt32((int)value.LayerBackgroundType);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldWeatherType");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            context.Writer.WriteInt32((int)value.WeatherType);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldLightingType");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            context.Writer.WriteInt32((int)value.LightingType);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldMusicIndex");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            context.Writer.WriteInt32(value.MusicIndex);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldGravityMode");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("GM");
            context.Writer.WriteInt32((int)value.GravityMode);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldStartPoint");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("x");
            context.Writer.WriteInt32(value.StartingPoint.X);
            context.Writer.WriteName("y");
            context.Writer.WriteInt32(value.StartingPoint.Y);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldSizeSettingsType");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("WorldSizeX");
            context.Writer.WriteInt32(value.Size.X);
            context.Writer.WriteName("WorldSizeY");
            context.Writer.WriteInt32(value.Size.Y);
            context.Writer.WriteEndDocument();
        }

        var size = value.Size.X * value.Size.Y;
        var blockLayerData = new byte[size * sizeof(short)];
        var backgroundLayerData = new byte[size * sizeof(short)];
        var waterLayerData = new byte[size * sizeof(short)];
        var wiringLayerData = new byte[size * sizeof(short)];

        for (int i = 0; i < size; ++i)
        {
            Buffer.BlockCopy(BitConverter.GetBytes((short)value.BlockLayer[i].BlockType), 0, blockLayerData, i * 2, sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)value.BlockBackgroundLayer[i].BlockType), 0, backgroundLayerData, i * 2, sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)value.BlockWaterLayer[i].BlockType), 0, waterLayerData, i * 2, sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)value.BlockWiringLayer[i].BlockType), 0, wiringLayerData, i * 2, sizeof(short));
        }

        context.Writer.WriteName("BlockLayer");
        context.Writer.WriteBytes(blockLayerData);
        context.Writer.WriteName("BackgroundLayer");
        context.Writer.WriteBytes(backgroundLayerData);
        context.Writer.WriteName("WaterLayer");
        context.Writer.WriteBytes(waterLayerData);
        context.Writer.WriteName("WiringLayer");
        context.Writer.WriteBytes(wiringLayerData);

        void WriteLayerHits(string name, LayerType layerType)
        {
            context.Writer.WriteName(name);
            context.Writer.WriteStartArray();

            for (int i = 0; i < size; ++i)
            {
                switch (layerType)
                {
                    case LayerType.Block:
                    {
                        context.Writer.WriteInt32(value.BlockLayer[i].HitsRequired);
                        break;
                    }

                    case LayerType.Background:
                    {
                        context.Writer.WriteInt32(value.BlockBackgroundLayer[i].HitsRequired);
                        break;
                    }

                    case LayerType.Water:
                    {
                        context.Writer.WriteInt32(value.BlockWaterLayer[i].HitsRequired);
                        break;
                    }

                    case LayerType.Wiring:
                    {
                        context.Writer.WriteInt32(value.BlockWiringLayer[i].HitsRequired);
                        break;
                    }
                }
            }

            context.Writer.WriteEndArray();
        }

        void WriteLayerHitBuffers(string name, LayerType layerType)
        {
            context.Writer.WriteName(name);
            context.Writer.WriteStartArray();

            for (int i = 0; i < size; ++i)
            {
                switch (layerType)
                {
                    case LayerType.Block:
                    {
                        context.Writer.WriteInt32(value.BlockLayer[i].HitBuffer);
                        break;
                    }

                    case LayerType.Background:
                    {
                        context.Writer.WriteInt32(value.BlockBackgroundLayer[i].HitBuffer);
                        break;
                    }

                    case LayerType.Water:
                    {
                        context.Writer.WriteInt32(value.BlockWaterLayer[i].HitBuffer);
                        break;
                    }

                    case LayerType.Wiring:
                    {
                        context.Writer.WriteInt32(value.BlockWiringLayer[i].HitBuffer);
                        break;
                    }
                }
            }

            context.Writer.WriteEndArray();
        }

        // So fucking inefficient, but there is no other way apparently.
        // I can use context.Writer.WriteRawBsonArray, loops once and cache everything
        // but it takes an IByteBuffer which is what the fuck.
        // There is no actual clear documentation about this so i have no idea.
        WriteLayerHits("BlockLayerHits", LayerType.Block);
        WriteLayerHits("BackgroundLayerHits", LayerType.Background);
        WriteLayerHits("WaterLayerHits", LayerType.Water);
        WriteLayerHits("WiringLayerHits", LayerType.Wiring);

        WriteLayerHitBuffers("BlockLayerHitBuffers", LayerType.Block);
        WriteLayerHitBuffers("BackgroundLayerHitBuffers", LayerType.Background);
        WriteLayerHitBuffers("WaterLayerHitBuffers", LayerType.Water);
        WriteLayerHitBuffers("WiringLayerHitBuffers", LayerType.Wiring);

        var seeds = new List<(SeedData, int, int)>();
        var itemDatas = new List<(WorldItemBase, int, int)>();
        for (int x = 0; x < value.Size.X; ++x)
        {
            for (int y = 0; y < value.Size.Y; ++y)
            {
                var index = x + y * value.Size.X;

                var seed = value.PlantedSeeds[index];
                if (seed is not null)
                {
                    seeds.Add((seed, x, y));
                }

                var itemData = value.ItemDatas[index];
                if (itemData is not null)
                {
                    itemDatas.Add((itemData, x, y));
                }
            }
        }

        {
            context.Writer.WriteName("Collectables");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            context.Writer.WriteInt32(value.Collectables.Count);

            foreach (var collectable in value.Collectables)
            {
                context.Writer.WriteName($"C{collectable.Id}");
                context.Writer.WriteStartDocument();

                context.Writer.WriteName(NetStrings.COLLECTABLE_ID_KEY);
                context.Writer.WriteInt32(collectable.Id);

                context.Writer.WriteName(NetStrings.BLOCK_TYPE_KEY);
                context.Writer.WriteInt32((int)collectable.BlockType);

                context.Writer.WriteName(NetStrings.COLLECT_AMOUNT_KEY);
                context.Writer.WriteInt32(collectable.Amount);

                context.Writer.WriteName(NetStrings.INVENTORY_TYPE_KEY);
                context.Writer.WriteInt32((int)collectable.InventoryItemType);

                context.Writer.WriteName(NetStrings.POSITION_X_FLOAT_KEY);
                context.Writer.WriteDouble(collectable.Pos.X);

                context.Writer.WriteName(NetStrings.POSITION_Y_FLOAT_KEY);
                context.Writer.WriteDouble(collectable.Pos.Y);

                context.Writer.WriteName(NetStrings.IS_GEM_KEY);
                context.Writer.WriteBoolean(collectable.IsGem);

                context.Writer.WriteName(NetStrings.GEM_TYPE_KEY);
                context.Writer.WriteInt32((int)collectable.GemType);

                if (collectable.InventoryData is not null)
                {
                    context.Writer.WriteName(NetStrings.INVENTORY_DATA_KEY);
                    BsonDocumentSerializer.Instance.Serialize(context, collectable.InventoryData.Serialize());
                }

                context.Writer.WriteEndDocument();
            }

            context.Writer.WriteEndDocument();
        }

        {
            // TODO: WorldRandomEvents
            context.Writer.WriteName("WorldRandomEvents");
            context.Writer.WriteStartDocument();
            context.Writer.WriteName("Count");
            //context.Writer.WriteInt32(value.RandomEvents.Count);
            context.Writer.WriteInt32(0);
            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("PlantedSeeds");
            context.Writer.WriteStartDocument();

            foreach (var (seed, x, y) in seeds)
            {
                context.Writer.WriteName($"W {x} {y}");
                context.Writer.WriteStartDocument();

                context.Writer.WriteName(NetStrings.POSITION_X_KEY);
                context.Writer.WriteInt32(seed.Position.X);

                context.Writer.WriteName(NetStrings.POSITION_Y_KEY);
                context.Writer.WriteInt32(seed.Position.Y);

                context.Writer.WriteName(NetStrings.BLOCK_TYPE_KEY);
                context.Writer.WriteInt32((int)seed.BlockType);

                context.Writer.WriteName(NetStrings.GROWTH_DURATION_KEY);
                context.Writer.WriteInt32(seed.GrowthDurationInSeconds);

                context.Writer.WriteName(NetStrings.GROWTH_END_TIME_KEY);
                context.Writer.WriteInt64(seed.GrowthEndTime.Ticks);

                context.Writer.WriteName(NetStrings.IS_MIXED_KEY);
                context.Writer.WriteBoolean(seed.IsAlreadyCrossBred);

                context.Writer.WriteName(NetStrings.HARVEST_SEEDS_KEY);
                context.Writer.WriteInt32(seed.HarvestSeeds);

                context.Writer.WriteName(NetStrings.HARVEST_BLOCKS_KEY);
                context.Writer.WriteInt32(seed.HarvestBlocks);

                context.Writer.WriteName(NetStrings.HARVEST_GEMS_KEY);
                context.Writer.WriteInt32(seed.HarvestGems);

                context.Writer.WriteName(NetStrings.HARVEST_EXTRA_BLOCKS_KEY);
                context.Writer.WriteInt32(seed.HarvestExtraBlocks);

                context.Writer.WriteEndDocument();
            }

            context.Writer.WriteEndDocument();
        }

        {
            context.Writer.WriteName("WorldItems");
            context.Writer.WriteStartDocument();

            foreach (var (itemData, x, y) in itemDatas)
            {
                context.Writer.WriteName($"W {x} {y}");
                BsonDocumentSerializer.Instance.Serialize(context, itemData.Serialize());
            }

            context.Writer.WriteEndDocument();
        }

        context.Writer.WriteName("InventoryId");
        context.Writer.WriteInt32(value.InventoryId);
        context.Writer.WriteName("WorldItemId");
        context.Writer.WriteInt32(value.ItemId);

        context.Writer.WriteEndDocument();
    }
}

public class GetWorldResponseSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer GetSerializer(Type type)
    {
        if (type == typeof(GetWorldResponse))
        {
            return new GetWorldResponseSerializer();
        }
        return null!;
    }
}
