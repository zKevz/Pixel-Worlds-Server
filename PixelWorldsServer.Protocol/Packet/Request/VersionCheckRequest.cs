using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class VersionCheckRequest : PacketBase
{
    [BsonElement(NetStrings.OPERATING_SYSTEM_KEY)]
    public string OS { get; set; } = string.Empty;

    [BsonElement(NetStrings.OPERATING_SYSTEM_TYPE_KEY)]
    public int OsType { get; set; }
}
