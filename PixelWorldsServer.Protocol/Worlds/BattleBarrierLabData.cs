using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class BattleBarrierLabData : WorldItemBase
{
    [BsonElement("isOpen")]
    public bool IsOpen { get; set; }

    public BattleBarrierLabData() : base(0, BlockType.BattleBarrierLab)
    {
    }

    public BattleBarrierLabData(int itemId) : base(itemId, BlockType.BattleBarrierLab)
    {
    }
}
