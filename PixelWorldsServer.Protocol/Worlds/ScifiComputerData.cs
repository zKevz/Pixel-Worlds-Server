using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class ScifiComputerData : WorldItemBase
{
    public ScifiComputerData() : base(0, BlockType.ScifiComputer)
    {
    }

    public ScifiComputerData(int itemId) : base(itemId, BlockType.ScifiComputer)
    {
    }
}
