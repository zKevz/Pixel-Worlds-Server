using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class TryToJoinWorldResponse : PacketBase
{
    [BsonElement(NetStrings.WORLD_NAME_KEY)]
    public string WorldName { get; set; } = string.Empty;

    [BsonElement(NetStrings.JOIN_RESULT_KEY)]
    public WorldJoinResult JoinResult { get; set; }

    [BsonElement(NetStrings.WORLD_BIOME_KEY)]
    public BasicWorldBiome Biome { get; set; }
}
