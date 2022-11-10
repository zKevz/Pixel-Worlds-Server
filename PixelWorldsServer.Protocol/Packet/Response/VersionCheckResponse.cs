using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class VersionCheckResponse : PacketBase
{
    [BsonElement(NetStrings.VERSION_NUMBER_KEY)]
    public int VersionNumber { get; set; }
}
