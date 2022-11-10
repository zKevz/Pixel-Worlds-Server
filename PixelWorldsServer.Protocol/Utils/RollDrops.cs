using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Utils;

public static class RollDrops
{
    public struct RollChances
    {
        private readonly short[] m_AmountArray = new short[100];

        public RollChances(short[] chances, short[] amounts)
        {
            int counter = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                for (int j = 0; j < chances[i]; j++)
                {
                    m_AmountArray[counter++] = amounts[i];
                }
            }
        }

        public short GetRolledAmount(int roll)
        {
            return m_AmountArray[roll];
        }
    }

    public static bool DoesTreeDropSeed(BlockType blockType)
    {
        return RollD100() < ConfigData.TreeDropSeedPercentage[(int)blockType];
    }

    public static bool DoesBlockDropSeed(BlockType blockType)
    {
        return RollD100() < ConfigData.BlockDropSeedPercentage[(int)blockType];
    }

    public static bool DoesBlockDropBlock(BlockType blockType)
    {
        return RollD100() < ConfigData.BlockDropBlockPercentage[(int)blockType];
    }

    public static bool DoesTreeDropExtraBlock(BlockType blockType)
    {
        int treeExtraDropChance = ConfigData.TreeExtraDropChance[(int)blockType];
        if (treeExtraDropChance == 0)
        {
            return false;
        }
        return 0 == GenericRoll(0, treeExtraDropChance);
    }

    public static bool DoesBlockDropExtraBlock(BlockType blockType)
    {
        int blockExtraDropChance = ConfigData.BlockExtraDropChance[(int)blockType];
        if (blockExtraDropChance == 0)
        {
            return false;
        }
        return 0 == GenericRoll(0, blockExtraDropChance);
    }

    public static short TreeDropsBlocks(BlockType blockType)
    {
        short treeDropBlockRangeMax = ConfigData.TreeDropBlockRangeMax[(int)blockType];
        if (treeDropBlockRangeMax == 0)
        {
            return 0;
        }
        short treeDropBlockRangeMin = ConfigData.TreeDropBlockRangeMin[(int)blockType];
        return GenericRoll(treeDropBlockRangeMin, (short)(treeDropBlockRangeMax + 1));
    }

    public static short TreeDropsGems(BlockType blockType)
    {
        short treeDropGemRangeMax = ConfigData.TreeDropGemRangeMax[(int)blockType];
        if (treeDropGemRangeMax == 0)
        {
            return 0;
        }
        short treeDropGemRangeMin = ConfigData.TreeDropGemRangeMin[(int)blockType];
        return GenericRoll(treeDropGemRangeMin, (short)(treeDropGemRangeMax + 1));
    }

    public static short BlockDropsGems(BlockType blockType)
    {
        short blockDropGemRangeMax = ConfigData.BlockDropGemRangeMax[(int)blockType];
        if (ConfigData.BlockDropGemPercentageOn[(int)blockType])
        {
            if (RollD100() < blockDropGemRangeMax)
            {
                return 1;
            }
            return 0;
        }
        if (blockDropGemRangeMax == 0)
        {
            return 0;
        }
        short blockDropGemRangeMin = ConfigData.BlockDropGemRangeMin[(int)blockType];
        return GenericRoll(blockDropGemRangeMin, (short)(blockDropGemRangeMax + 1));
    }

    private static byte RollD100()
    {
        return (byte)GenericRoll(0, 100);
    }

    private static short GenericRoll(short minInclusive, short maxExclusive)
    {
        return (short)GenericRoll((int)minInclusive, (int)maxExclusive);
    }

    public static int GenericRoll(int minInclusive, int maxExclusive)
    {
        return new Random().Next(minInclusive, maxExclusive);
    }

    public static int RollPosition(int min, int max)
    {
        return new Random().Next(min, max);
    }

    public static void Shuffle<T>(T[] array)
    {
        var length = array.Length;
        var random = new Random();
        while (length > 1)
        {
            int index = random.Next(length--);

            T val = array[length];
            array[length] = array[index];
            array[index] = val;
        }
    }
}

