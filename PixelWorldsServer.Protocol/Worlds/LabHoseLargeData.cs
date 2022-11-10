using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class LabHoseLargeData : WorldItemBase
{
    public LabHoseLargeData() : base(0, BlockType.LabHoseLarge)
    {
    }

    public LabHoseLargeData(int itemId) : base(itemId, BlockType.LabHoseLarge)
    {
    }
}
