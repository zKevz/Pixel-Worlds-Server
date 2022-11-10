using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class LayerBlockBackground
{
    public BlockType BlockType { get; set; } = BlockType.None;
    public int HitsRequired { get; set; }
    public int HitBuffer { get; set; }
    public int WaitingBlockIndex { get; set; }
    public bool IsWaitingForBlock { get; set; }

    [BsonIgnore]
    public DateTime LastHitTime { get; set; } = DateTime.Now;
}
