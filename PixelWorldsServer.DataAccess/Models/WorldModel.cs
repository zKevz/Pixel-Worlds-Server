using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Protocol.Worlds;

namespace PixelWorldsServer.DataAccess.Models;

public class WorldModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = string.Empty;

    public int ItemId { get; set; } = 1;// WorldItem last id
    public int MusicIndex { get; set; }
    public int InventoryId { get; set; } = 1;// Collectables last id

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

    public void LoadCopy(WorldModel worldModel)
    {
        Id = worldModel.Id;
        Name = worldModel.Name;

        ItemId = worldModel.ItemId;
        MusicIndex = worldModel.MusicIndex;
        InventoryId = worldModel.InventoryId;

        Size = worldModel.Size;
        StartingPoint = worldModel.StartingPoint;

        WeatherType = worldModel.WeatherType;
        GravityMode = worldModel.GravityMode;
        LightingType = worldModel.LightingType;
        LayoutType = worldModel.LayoutType;
        LayerBackgroundType = worldModel.LayerBackgroundType;

        BlockLayer = worldModel.BlockLayer;
        PlantedSeeds = worldModel.PlantedSeeds;
        ItemDatas = worldModel.ItemDatas;
        BlockWaterLayer = worldModel.BlockWaterLayer;
        BlockWiringLayer = worldModel.BlockWiringLayer;
        BlockBackgroundLayer = worldModel.BlockBackgroundLayer;
    }

    public static WorldModel CreateCopy(WorldModel worldModel)
    {
        WorldModel result = new();
        result.LoadCopy(worldModel);
        return result;
    }
}

public class WorldItemDataSerializer : SerializerBase<List<WorldItemBase?>>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, List<WorldItemBase?> value)
    {
        context.Writer.WriteStartDocument();
        context.Writer.WriteName("Count");
        context.Writer.WriteInt32(value.Count);
        context.Writer.WriteName("Values");
        context.Writer.WriteStartArray();

        foreach (var item in value)
        {
            if (item is null)
            {
                context.Writer.WriteStartDocument();
                context.Writer.WriteName("class");
                context.Writer.WriteString("null");
                context.Writer.WriteEndDocument();
            }
            else
            {
                BsonDocumentSerializer.Instance.Serialize(context, item.Serialize());
            }
        }

        context.Writer.WriteEndArray();
        context.Writer.WriteEndDocument();
    }

    public override List<WorldItemBase?> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        context.Reader.ReadStartDocument();
        var count = context.Reader.ReadInt32();
        var list = new List<WorldItemBase?>(count);
        context.Reader.ReadStartArray();

        for (int i = 0; i < count; ++i)
        {
            var childDocument = BsonDocumentSerializer.Instance.Deserialize(context);
            var className = childDocument["class"].AsString;
            if (className != "null")
            {
                childDocument.Remove("class");

                var blockType = WorldItemBase.GetBlockTypeViaClassName(className);
                var worldItemType = DataFactory.GetDataTypeForEnum(blockType);
                var worldItemBase = (WorldItemBase)BsonSerializer.Deserialize(childDocument, worldItemType);
                list.Add(worldItemBase);
            }
            else
            {
                list.Add(null);
            }
        }

        context.Reader.ReadEndArray();
        context.Reader.ReadEndDocument();

        return list;
    }
}

public class WorldItemDataSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer GetSerializer(Type type)
    {
        if (type == typeof(List<WorldItemBase?>))
        {
            return new WorldItemDataSerializer();
        }
        return null!;
    }
}
