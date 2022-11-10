using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class RemoveCollectResponse : PacketBase
{
    [BsonElement(NetStrings.COLLECTABLE_ID_KEY)]
    public int CollectableId { get; set; }
}
