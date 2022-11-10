using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class MenuWorldLoadInfoRequest : PacketBase
{
    [BsonElement(NetStrings.WORLD_NAME_KEY)]
    public string WorldName { get; set; } = string.Empty;
}
