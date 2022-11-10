using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Worlds;

namespace PixelWorldsServer.Protocol.Utils;

public static class Seeds
{
    private static BlockType m_TempHolder;
    private static readonly Dictionary<BlockTuple, BlockType> m_CrossBreedings = new();

    public static void Clear()
    {
        m_CrossBreedings.Clear();
    }

    public static void SetFirstCrossBreedingPart(string firstPart, int _)
    {
        m_TempHolder = (BlockType)int.Parse(firstPart);
    }

    public static void AddCrossBreeding(string secondPart, int index)
    {
        var newSecond = (BlockType)int.Parse(secondPart);
        var blockTuple = new BlockTuple(m_TempHolder, newSecond);
        m_CrossBreedings[blockTuple] = (BlockType)index;
    }

    public static BlockType GetCrossBreedingResult(BlockTuple query)
    {
        if (m_CrossBreedings.ContainsKey(query))
        {
            return m_CrossBreedings[query];
        }
        return BlockType.None;
    }

    public static SeedData GenerateSeedData(BlockType typeOfSeed, Vector2i pos, bool isMixed = false)
    {
        int blockComplexity = ConfigData.BlockComplexity[(int)typeOfSeed];
        int growthDurationSeconds = SeedData.CalculateGrowthTimeInSeconds(blockComplexity);
        int growthTimeInSeconds = ConfigData.GrowthTimeInSeconds[(int)typeOfSeed];
        if (growthTimeInSeconds != ConfigData.DefaultGrowthTimeInSeconds)
        {
            growthDurationSeconds = growthTimeInSeconds;
        }

        return new SeedData(typeOfSeed, pos, growthDurationSeconds, isMixed);
    }

    public static BlockTuple? GetBlockTuple(BlockType blockType)
    {
        foreach (var (key, value) in m_CrossBreedings)
        {
            if (value == blockType)
            {
                return key;
            }
        }

        return null;
    }
}

