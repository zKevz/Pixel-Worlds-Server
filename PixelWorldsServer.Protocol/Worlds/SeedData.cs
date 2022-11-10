using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Worlds;

public class SeedData
{
    private static readonly int m_GrowthTimeInSecondsForZeroOrLessComplexity = 8640000;
    private static readonly int m_MinGrowthTimeInSeconds = 30;
    private static readonly int m_MaxGrowthTimeInSeconds = 31536000;

    public BlockType BlockType { get; set; }
    public DateTime GrowthEndTime { get; set; }
    public int GrowthDurationInSeconds { get; set; }
    public bool IsAlreadyCrossBred { get; set; }
    public Vector2i Position { get; set; } = new();
    public short HarvestSeeds { get; set; }
    public short HarvestBlocks { get; set; }
    public short HarvestGems { get; set; }
    public short HarvestExtraBlocks { get; set; }

    public SeedData(BlockType blockType, Vector2i position, int growthDurationSeconds, bool isMixed = false)
    {
        BlockType = blockType;
        Position = position;
        GrowthEndTime = DateTime.UtcNow.AddSeconds(growthDurationSeconds);
        GrowthDurationInSeconds = growthDurationSeconds;
        IsAlreadyCrossBred = isMixed;
        HarvestSeeds = (short)(RollDrops.DoesTreeDropSeed(blockType) ? 1 : 0);
        HarvestBlocks = RollDrops.TreeDropsBlocks(blockType);
        HarvestGems = RollDrops.TreeDropsGems(blockType);
        HarvestExtraBlocks = (short)(RollDrops.DoesTreeDropExtraBlock(blockType) ? 1 : 0);
    }

    public static int CalculateGrowthTimeInSeconds(int blockComplexity)
    {
        if (blockComplexity <= 0)
        {
            return m_GrowthTimeInSecondsForZeroOrLessComplexity;
        }

        double growthTime = Math.Floor(Math.Pow(blockComplexity, 3.2) + 30.0 * Math.Pow(blockComplexity, 1.4));

        if (growthTime < m_MinGrowthTimeInSeconds)
        {
            growthTime = m_MinGrowthTimeInSeconds;
        }
        else if (growthTime > m_MaxGrowthTimeInSeconds)
        {
            growthTime = m_MaxGrowthTimeInSeconds;
        }

        return (int)growthTime;
    }
}
