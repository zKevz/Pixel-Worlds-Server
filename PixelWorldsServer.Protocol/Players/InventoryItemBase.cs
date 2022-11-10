using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Players;

public abstract class InventoryItemBase
{
    [BsonElement("inventoryClass")]
    public string InventoryClass { get; set; } = string.Empty;

    [BsonElement("blockType")]
    public BlockType BlockType { get; set; }

    public BsonDocument Serialize()
    {
        var document = this.ToBsonDocument();
        document.Add("class", GetType().Name);
        return document;
    }
}
