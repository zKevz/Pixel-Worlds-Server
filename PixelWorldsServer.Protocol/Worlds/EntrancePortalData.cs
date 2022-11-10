using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class EntrancePortalData : WorldItemBase
{
    public EntrancePortalData() : base(0, BlockType.EntrancePortal)
    {
    }

    public EntrancePortalData(int itemId) : base(itemId, BlockType.EntrancePortal)
    {
    }
}
