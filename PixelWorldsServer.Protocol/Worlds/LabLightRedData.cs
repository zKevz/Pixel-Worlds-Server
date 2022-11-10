using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class LabLightRedData : WorldItemBase
{
    public LabLightRedData() : base(0, BlockType.LabLightRed)
    {
    }

    public LabLightRedData(int itemId) : base(itemId, BlockType.LabLightRed)
    {
    }
}
