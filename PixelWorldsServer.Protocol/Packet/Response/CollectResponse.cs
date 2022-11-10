using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class CollectResponse : PacketBase
{
    [BsonElement(NetStrings.COLLECTABLE_ID_KEY)]
    public int CollectableId { get; set; }

    [BsonElement(NetStrings.BLOCK_TYPE_KEY)]
    public BlockType BlockType { get; set; }

    [BsonElement(NetStrings.COLLECT_AMOUNT_KEY)]
    public int Amount { get; set; }

    [BsonElement(NetStrings.INVENTORY_TYPE_KEY)]
    public InventoryItemType InventoryType { get; set; }

    [BsonElement(NetStrings.POSITION_X_FLOAT_KEY)]
    public float PositionX { get; set; }

    [BsonElement(NetStrings.POSITION_Y_FLOAT_KEY)]
    public float PositionY { get; set; }

    [BsonElement(NetStrings.IS_GEM_KEY)]
    public bool IsGem { get; set; }

    [BsonElement(NetStrings.GEM_TYPE_KEY)]
    public GemType GemType { get; set; }
}
