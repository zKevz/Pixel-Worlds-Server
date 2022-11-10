using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class GetWorldRequest : PacketBase
{
    [BsonElement(NetStrings.ENTRANCE_PORTAL_ID_KEY)]
    public string EntrancePortalId { get; set; } = string.Empty;

    [BsonElement(NetStrings.WORLD_KEY)]
    public string World { get; set; } = string.Empty;

    [BsonElement(NetStrings.WORLD_BIOME_KEY)]
    public BasicWorldBiome Biome { get; set; }
}
