using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

// literally the same as LayerBlockBackground but i dont have any idea why pw separates them
public class LayerWiring
{
    public BlockType BlockType { get; set; } = BlockType.None;
    public int HitsRequired { get; set; }
    public int HitBuffer { get; set; }
    public int WaitingBlockIndex { get; set; }
    public bool IsWaitingForBlock { get; set; }

    [BsonIgnore]
    public DateTime LastHitTime { get; set; } = DateTime.Now;
}
