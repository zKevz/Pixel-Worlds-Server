using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class SyncTimeRequest : PacketBase
{
    [BsonElement(NetStrings.TIME_KEY)]
    public long Time { get; set; }
}
