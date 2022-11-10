using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class StereosData : WorldItemBase
{
    public StereosData() : base(0, BlockType.Stereos)
    {
    }

    public StereosData(int itemId) : base(itemId, BlockType.Stereos)
    {
    }
}
