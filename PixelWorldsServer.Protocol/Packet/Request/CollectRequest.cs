using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class CollectRequest : PacketBase
{
    [BsonElement(NetStrings.COLLECTABLE_ID_KEY)]
    public int CollectableId { get; set; }
}
