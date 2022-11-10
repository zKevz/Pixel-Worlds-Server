using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class MenuWorldLoadInfoResponse : PacketBase
{
    [BsonElement(NetStrings.WORLD_NAME_KEY)]
    public string WorldName { get; set; } = string.Empty;

    [BsonElement(NetStrings.COUNT_KEY)]
    public int Count { get; set; } // if it is exist return the players count in that world, or else return negative
}
