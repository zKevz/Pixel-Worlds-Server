using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class SewerPipeBlackData : WorldItemBase
{
    public SewerPipeBlackData() : base(0, BlockType.SewerPipeBlack)
    {
    }

    public SewerPipeBlackData(int itemId) : base(itemId, BlockType.SewerPipeBlack)
    {
    }
}
