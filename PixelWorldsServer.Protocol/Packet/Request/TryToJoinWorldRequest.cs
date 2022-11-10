using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class TryToJoinWorldRequest : PacketBase
{
    [BsonElement(NetStrings.WORLD_KEY)]
    public string World { get; set; } = string.Empty;

    [BsonElement(NetStrings.AMOUNT_KEY)]
    public int ServerConnectAttempts { get; set; }

    [BsonElement(NetStrings.WORLD_BIOME_KEY)]
    public BasicWorldBiome Biome { get; set; }
}
