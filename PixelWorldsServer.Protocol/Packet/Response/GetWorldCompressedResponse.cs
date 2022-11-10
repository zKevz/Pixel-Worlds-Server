using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class GetWorldCompressedResponse : PacketBase
{
    [BsonElement(NetStrings.WORLD_KEY)]
    public byte[] WorldData { get; set; } = null!;
}
