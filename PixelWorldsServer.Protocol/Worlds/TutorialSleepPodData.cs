using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class TutorialSleepPodData : WorldItemBase
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


    public TutorialSleepPodData() : base(0, BlockType.TutorialSleepPod)
    {
    }

    public TutorialSleepPodData(int itemId) : base(itemId, BlockType.TutorialSleepPod)
    {
    }
}
