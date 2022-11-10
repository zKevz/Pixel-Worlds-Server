using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class TutorialCablePortalData : WorldItemBase
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


    public TutorialCablePortalData() : base(0, BlockType.TutorialCablePortal)
    {
    }

    public TutorialCablePortalData(int itemId) : base(itemId, BlockType.TutorialCablePortal)
    {
    }
}
