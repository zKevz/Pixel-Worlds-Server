using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class LanternBlueData : WorldItemBase
{
    public LanternBlueData() : base(0, BlockType.LanternBlue)
    {
    }

    public LanternBlueData(int itemId) : base(itemId, BlockType.LanternBlue)
    {
    }
}
