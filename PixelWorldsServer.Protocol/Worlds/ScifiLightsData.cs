using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class ScifiLightsData : WorldItemBase
{
    public ScifiLightsData() : base(0, BlockType.ScifiLights)
    {
    }

    public ScifiLightsData(int itemId) : base(itemId, BlockType.ScifiLights)
    {
    }
}
