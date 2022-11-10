using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class ScifiArrowData : WorldItemBase
{
    public ScifiArrowData() : base(0, BlockType.ScifiArrow)
    {
    }

    public ScifiArrowData(int itemId) : base(itemId, BlockType.ScifiArrow)
    {
    }
}
