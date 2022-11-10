using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class BuyItemPackResponse : PacketBase
{
    [BsonElement(NetStrings.ITEM_PACK_ID_KEY)]
    public string ItemPackId { get; set; } = string.Empty;

    [BsonElement(NetStrings.ITEM_PACK_ROLLS_KEY)]
    public int[] ItemPackRolls { get; set; } = Array.Empty<int>();

    [BsonElement(NetStrings.ITEM_PACK_ROLLS2_KEY)]
    public int[] ItemPackRolls2 { get; set; } = Array.Empty<int>();

    [BsonElement(NetStrings.SUCCESS_KEY)]
    [BsonIgnoreIfNull]
    public string Success { get; set; } = null!;

    [BsonElement(NetStrings.ERROR_KEY)]
    [BsonIgnoreIfNull]
    public string ErrorReason { get; set; } = null!;
}
