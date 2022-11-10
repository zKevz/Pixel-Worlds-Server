using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public abstract class WorldItemBase
{
    [BsonElement("itemId")]
    public int ItemId { get; set; }

    [BsonElement("blockType")]
    public BlockType BlockType { get; set; }

    [BsonElement("direction")]
    public BlockDirection BlockDirection { get; set; }

    [BsonElement("animOn")]
    public bool IsAnimationOn { get; set; }

    [BsonElement("anotherSprite")]
    public bool UseAnotherSprite { get; set; }

    [BsonElement("damageNow")]
    public bool DoDamageNow { get; set; }

    public WorldItemBase()
    {
    }

    public WorldItemBase(int itemId, BlockType blockType)
    {
        ItemId = itemId;
        BlockType = blockType;
    }

    public static BlockType GetBlockTypeViaClassName(string className)
    {
        return Enum.Parse<BlockType>(className.Remove(className.Length - 4), true);
    }

    public BsonDocument Serialize()
    {
        var document = this.ToBsonDocument();
        document.Add("class", GetType().Name);
        return document;
    }
}
