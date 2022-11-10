using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class PortalData : WorldItemBase
{
    [BsonElement("targetWorldID")]
    public string TargetWorldId { get; set; } = string.Empty;

    [BsonElement("targetEntryPointID")]
    public string TargetEntryPointId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("entryPointID")]
    public string EntryPointId { get; set; } = string.Empty;

    [BsonElement("isLocked")]
    public bool IsLocked { get; set; }

    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    public PortalData() : base(0, BlockType.Portal)
    {
    }

    public PortalData(int itemId) : base(itemId, BlockType.Portal)
    {
    }
}
