using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class BuyItemPackRequest : PacketBase
{
    [BsonElement(NetStrings.ITEM_PACK_ID_KEY)]
    public string ItemPackId { get; set; } = string.Empty;
}
