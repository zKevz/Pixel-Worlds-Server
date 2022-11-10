using Newtonsoft.Json;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Utils;

public static class ConfigData
{
    public delegate int ParseItemNumber(string[] strings);
    public delegate void SetConfigDataValue(string newValue, int index);

    public static readonly int Gem1Value = 1;
    public static readonly int Gem2Value = 5;
    public static readonly int Gem3Value = 20;
    public static readonly int Gem4Value = 50;
    public static readonly int Gem5Value = 100;
    public static readonly int RollCollectableXMin = -14;
    public static readonly int RollCollectableXMax = 15;
    public static readonly int RollCollectableYMin = -14;
    public static readonly int RollCollectableYMax = 15;
    public static readonly float TileSizeX = 0.32f;
    public static readonly float TileSizeY = 0.32f;
    public static readonly float ReviveBlockTime = 5f;

    public static int DefaultBlockHitPoints { get; private set; } = 1000;
    public static int DefaultBlockComplexity { get; private set; }
    public static int DefaultGrowthTimeInSeconds { get; private set; } = 60;
    public static int DefaultTreeExtraDropChance { get; private set; }
    public static int DefaultBlockExtraDropChance { get; private set; }
    public static byte DefaultTreeDropSeedPercentage { get; private set; }
    public static byte DefaultBlockDropSeedPercentage { get; private set; }
    public static byte DefaultBlockDropBlockPercentage { get; private set; }
    public static bool DefaultBlockDropGemPercentageOn { get; private set; }
    public static float DefaultRecycleValue { get; private set; }
    public static short DefaultTreeDropGemRangeMin { get; private set; }
    public static short DefaultTreeDropGemRangeMax { get; private set; }
    public static short DefaultBlockDropGemRangeMin { get; private set; }
    public static short DefaultBlockDropGemRangeMax { get; private set; }
    public static short DefaultTreeDropBlockRangeMin { get; private set; }
    public static short DefaultTreeDropBlockRangeMax { get; private set; }
    public static BlockType DefaultTreeExtraDropBlock { get; private set; }
    public static BlockType DefaultBlockExtraDropBlock { get; private set; }
    public static BlockClass DefaultBlockClass { get; private set; } = BlockClass.GroundSoft;
    public static InventoryItemType DefaultBlockInventoryItemType { get; private set; } = InventoryItemType.Block;

    public static int[] BlockHitPoints { get; private set; } = null!;
    public static int[] BlockComplexity { get; private set; } = null!;
    public static int[] GrowthTimeInSeconds { get; private set; } = null!;
    public static int[] TreeExtraDropChance { get; private set; } = null!;
    public static int[] BlockExtraDropChance { get; private set; } = null!;
    public static byte[] TreeDropSeedPercentage { get; private set; } = null!;
    public static byte[] BlockDropSeedPercentage { get; private set; } = null!;
    public static byte[] BlockDropBlockPercentage { get; private set; } = null!;
    public static bool[] BlockDropGemPercentageOn { get; private set; } = null!;
    public static float[] RecycleValue { get; private set; } = null!;
    public static short[] TreeDropGemRangeMin { get; private set; } = null!;
    public static short[] TreeDropGemRangeMax { get; private set; } = null!;
    public static short[] TreeDropBlockRangeMin { get; private set; } = null!;
    public static short[] TreeDropBlockRangeMax { get; private set; } = null!;
    public static short[] BlockDropGemRangeMin { get; private set; } = null!;
    public static short[] BlockDropGemRangeMax { get; private set; } = null!;
    public static BlockType[] TreeExtraDropBlock { get; private set; } = null!;
    public static BlockType[] BlockExtraDropBlock { get; private set; } = null!;
    public static BlockClass[] BlockClasses { get; private set; } = null!;
    public static InventoryItemType[] BlockInventoryItemType { get; private set; } = null!;

    public static Dictionary<BlockType, List<AnimationHotSpots>> BlockHotSpots { get; set; } = new();

    private static T[] InitArray<T>(T value)
    {
        var array = new T[(int)BlockType.BLOCK_TYPE_COUNT];
        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = value;
        }
        return array;
    }

    private static string ReadText(BinaryReader reader)
    {
        return new string(reader.ReadChars(reader.ReadInt32()));
    }

    private static void ParseSomeCSV(string inputString, SetConfigDataValue?[] dataSetters, ParseItemNumber itemNumberParser)
    {
        var stringReader = new StringReader(inputString);
        while (true)
        {
            string? text = stringReader.ReadLine();
            if (text is null)
            {
                break;
            }

            string[] array = text.Split(',');
            int index = itemNumberParser(array);

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Length > 0 && dataSetters[i] is not null)
                {
                    dataSetters[i]!(array[i], index);
                }
            }
        }
    }

    private static int ParseWorldBlockItemNumber(string[] strings)
    {
        if (int.TryParse(strings[1], out var result))
        {
            return result;
        }

        return -1;
    }

    public static int[] SplitGemValueToGems(int gemValue)
    {
        int num = gemValue / Gem5Value;
        gemValue -= num * Gem5Value;
        int num2 = gemValue / Gem4Value;
        gemValue -= num2 * Gem4Value;
        int num3 = gemValue / Gem3Value;
        gemValue -= num3 * Gem3Value;
        int num4 = gemValue / Gem2Value;
        gemValue -= num4 * Gem2Value;
        int num5 = gemValue / Gem1Value;
        return new int[5] { num5, num4, num3, num2, num };
    }

    private static void InitArrays()
    {
        BlockClasses = InitArray(DefaultBlockClass);
        RecycleValue = InitArray(DefaultRecycleValue);
        BlockHitPoints = InitArray(DefaultBlockHitPoints);
        BlockComplexity = InitArray(DefaultBlockComplexity);
        TreeExtraDropBlock = InitArray(DefaultTreeExtraDropBlock);
        BlockExtraDropBlock = InitArray(DefaultBlockExtraDropBlock);
        TreeExtraDropChance = InitArray(DefaultTreeExtraDropChance);
        GrowthTimeInSeconds = InitArray(DefaultGrowthTimeInSeconds);
        TreeDropGemRangeMin = InitArray(DefaultTreeDropGemRangeMin);
        TreeDropGemRangeMax = InitArray(DefaultTreeDropGemRangeMax);
        BlockDropGemRangeMin = InitArray(DefaultBlockDropGemRangeMin);
        BlockDropGemRangeMax = InitArray(DefaultBlockDropGemRangeMax);
        BlockExtraDropChance = InitArray(DefaultBlockExtraDropChance);
        TreeDropBlockRangeMin = InitArray(DefaultTreeDropBlockRangeMin);
        TreeDropBlockRangeMax = InitArray(DefaultTreeDropBlockRangeMax);
        BlockInventoryItemType = InitArray(DefaultBlockInventoryItemType);
        TreeDropSeedPercentage = InitArray(DefaultTreeDropSeedPercentage);
        BlockDropSeedPercentage = InitArray(DefaultBlockDropSeedPercentage);
        BlockDropBlockPercentage = InitArray(DefaultBlockDropBlockPercentage);
        BlockDropGemPercentageOn = InitArray(DefaultBlockDropGemPercentageOn);
    }

    public static string ReadCsvContent(string csvFilePath)
    {
        using var fs = File.OpenRead(csvFilePath);
        using var br = new BinaryReader(fs);

        var csvId = ReadText(br);
        var content = ReadText(br);
        return content;
    }

    public static bool Init()
    {
        try
        {
            {
                var content = File.ReadAllText("Data/wearable-animation-storage.json");
                BlockHotSpots = JsonConvert.DeserializeObject<Dictionary<BlockType, List<AnimationHotSpots>>>(content)
                    ?? throw new NullReferenceException("Failed to serialize wearable-animation-storage");
            }

            {
                var content = ReadCsvContent("Data/default-blocks-config.csv");
                InitDefaultValues(content);
                InitArrays();
            }

            {
                var content = ReadCsvContent("Data/blocks-config.csv");
                SetConfigDataValue?[] dataSetters = new SetConfigDataValue?[]
                {
                    null,
                    null,
                    SetBlockTypeInventoryItemType,
                    null, //SetBlockSortingLayerType,
					null, //SetInventoryOrderType
					null, //SetIsValidForAuctionHouse
					null, //SetBlockAvailabilityDate
					null, //SetBlockSortingOrderInLayer,
					SetBlockClass,
                    null, //SetDoesBlockHaveCollider,
					null, //SetIsBlockColliderOneWay,
					SetHitsRequired,
                    SetTreeDropSeedPercentage,
                    SetTreeDropBlockRange,
                    SetTreeDropGemRange,
                    SetBlockDropSeedPercentage,
                    SetBlockDropBlockPercentage,
                    SetBlockDropGemRangeOrPercentage,
                    SetTreeExtraDropBlock,
                    SetTreeExtraDropChance,
                    SetBlockExtraDropBlock,
                    SetBlockExtraDropChance,
                    null, //SetBlockCustomGroups,
					SetRecycleValue,
                    Seeds.SetFirstCrossBreedingPart,
                    Seeds.AddCrossBreeding,
                    null, //SetShouldBelowSpriteUseAlternativeSprite,
					null, //SetBlockGroundDamping,
					null, //SetBlockRunSpeed,
					null, //SetBlockGravity,
					null, //SetBlockMaxFallVelocity,
					null, //SetBlockAirResistance,
					null, //SetBlockJumpHeight,
					null, //SetBlockJumpHeight60FPS,
					null, //SetIsBlockTrampolin,
					null, //SetIsBlockSpring,
					null, //SetIsBlockHot,
					null, //SetIsBlockPinball,
					null, //SetBlockBounceForce,
					null, //SetBlockBounceForce60FPS,
					null, //SetIsBlockSwimming,
					null, //SetIsBlockTradeable,
					null, //SetBlockParticleColor,
					null, //SetTreeHSL,
					null, //SetSeedHSL,
					null, //SetIsSeed,
					null, //SetBlockTreeSpriteIndex,
					null, //SetBlockSeedSpriteIndex,
					SetGrowthTimeInSeconds,
                    null, //SetCanBlockBeBehindWater,
					null, //SetShouldBlockUseSpriteAnimation,
					null, //SetBlockSpriteAnimationSpeed,
					null, //SetShouldBlockUseEffectSpriteAnimation,
					null, //SetBlockEffectSpriteAnimationSpeed,
					null, //SetShouldBlockUseLight,
					null, //SetBlockLightSpriteAnimationType,
					null, //SetBlockHitEffectOffnull, //SetX,
					null, //SetBlockHitEffectOffnull, //SetY,
					null, //SetOffnull, //SetAnimationSpeedX,
					null, //SetOffnull, //SetAnimationSpeedY,
					null, //SetBlockHandItemType,
					null, //SetToolUsableForBlock,
					null, //SetBlockStorageIndex,
					null, //SetBlockRange,
					null, //SetCanSitDownToBlock,
					null, //SetHitBuffer,
					null, //SetNewBlockAnimationOn,
					null, //SetShowButtonWhenPlayerIsNearEnough,
					null, //SetShowButtonWhenPlayerIsNearEnoughDistance,
					null, //SetBlockFadeInOutEffect,
					null, //SetBlockEffectSpriteAnimationType,
					null, //SetShouldBlockUseAnimalAnimation,
					null, //SetHidePlayerBlock,
					null, //SetBlockDirection,
					null, //SetSpriteAnimationLoopCount,
					null, //SetIsBlockWind,
					null, //SetHitForce,
					null, //SetBattleDamage,
					null, //SetArmor,
					null, //SetFireElement,
					null, //SetWaterElement,
					null, //SetEarthElement,
					null, //SetAirElement,
					null, //SetLightElement,
					null, //SetDarkElement,
					null, //SetCritChance,
					null, //SetBlockMaterialClass,
					SetBlockComplexity,
                    null, //SetSpriteContainsAlpha,
					null, //SetPlayerJumpModeForBlock,
					null, //SetIsBlockElastic,
					null, //SetShouldGiveBlockBackIntoInventory,
					null, //SetBlockSkinColorIndex,
					null, //SetShardRarity,
					null, //SetShouldCausePoisoned,
					null, //SetBlockMaxCountInWorld
				};

                ParseSomeCSV(content, dataSetters, ParseWorldBlockItemNumber);
            }

            return true;
        }
        catch
        {
            throw;
        }
    }

    private static void SetBlockComplexity(string newValue, int index)
    {
        BlockComplexity[index] = int.Parse(newValue);
    }

    private static void SetGrowthTimeInSeconds(string newValue, int index)
    {
        GrowthTimeInSeconds[index] = int.Parse(newValue);
    }

    // These values are fucked up
    public static void InitDefaultValues(string inputString)
    {
        string[] array = inputString.Split(',');
        DefaultBlockClass = (BlockClass)byte.Parse(array[12]);
        DefaultBlockHitPoints = int.Parse(array[15]);
        DefaultTreeDropSeedPercentage = byte.Parse(array[16]);

        {
            string[] array2 = array[17].Split('-');
            DefaultTreeDropBlockRangeMin = short.Parse(array2[0]);
            DefaultTreeDropBlockRangeMax = short.Parse(array2[1]);
        }

        {
            string[] array2 = array[18].Split('-');
            DefaultTreeDropGemRangeMin = short.Parse(array2[0]);
            DefaultTreeDropGemRangeMax = short.Parse(array2[1]);
        }

        DefaultBlockDropSeedPercentage = byte.Parse(array[19]);
        DefaultBlockDropBlockPercentage = byte.Parse(array[20]);

        {
            string[] array2 = array[21].Split('-');
            DefaultBlockDropGemRangeMin = short.Parse(array2[0]);
            DefaultBlockDropGemRangeMax = short.Parse(array2[1]);
        }

        DefaultTreeExtraDropBlock = (BlockType)int.Parse(array[22]);
        DefaultTreeExtraDropChance = int.Parse(array[23]);
        DefaultBlockExtraDropBlock = (BlockType)int.Parse(array[24]);
        DefaultBlockExtraDropChance = int.Parse(array[25]);
        DefaultRecycleValue = float.Parse(array[27]);
    }

    private static void SetRecycleValue(string newValue, int index)
    {
        RecycleValue[index] = float.Parse(newValue);
    }

    private static void SetBlockExtraDropChance(string newValue, int index)
    {
        BlockExtraDropChance[index] = int.Parse(newValue);
    }

    private static void SetBlockExtraDropBlock(string newValue, int index)
    {
        BlockExtraDropBlock[index] = (BlockType)int.Parse(newValue);
    }

    private static void SetTreeExtraDropChance(string newValue, int index)
    {
        TreeExtraDropChance[index] = int.Parse(newValue);
    }

    private static void SetTreeExtraDropBlock(string newValue, int index)
    {
        TreeExtraDropBlock[index] = (BlockType)int.Parse(newValue);
    }

    private static void SetBlockDropGemRangeOrPercentage(string newValue, int index)
    {
        if (newValue.EndsWith("%"))
        {
            BlockDropGemPercentageOn[index] = true;

            short num = short.Parse(newValue.Remove(newValue.Length - 1));
            BlockDropGemRangeMin[index] = num;
            BlockDropGemRangeMax[index] = num;
        }
        else
        {
            BlockDropGemPercentageOn[index] = false;

            string[] array = newValue.Split('-');
            BlockDropGemRangeMin[index] = short.Parse(array[0]);
            BlockDropGemRangeMax[index] = short.Parse(array[1]);
        }
    }

    private static void SetBlockDropBlockPercentage(string newValue, int index)
    {
        BlockDropBlockPercentage[index] = byte.Parse(newValue);
    }

    private static void SetBlockDropSeedPercentage(string newValue, int index)
    {
        BlockDropSeedPercentage[index] = byte.Parse(newValue);
    }

    private static void SetTreeDropGemRange(string newValue, int index)
    {
        string[] array = newValue.Split('-');
        TreeDropGemRangeMin[index] = short.Parse(array[0]);
        TreeDropGemRangeMax[index] = short.Parse(array[1]);
    }

    private static void SetTreeDropBlockRange(string newValue, int index)
    {
        string[] array = newValue.Split('-');
        TreeDropBlockRangeMin[index] = short.Parse(array[0]);
        TreeDropBlockRangeMax[index] = short.Parse(array[1]);
    }

    private static void SetTreeDropSeedPercentage(string newValue, int index)
    {
        TreeDropSeedPercentage[index] = byte.Parse(newValue);
    }

    private static void SetHitsRequired(string newValue, int index)
    {
        BlockHitPoints[index] = int.Parse(newValue);
    }

    private static void SetBlockClass(string newValue, int index)
    {
        BlockClasses[index] = (BlockClass)byte.Parse(newValue);
    }

    private static void SetBlockTypeInventoryItemType(string newValue, int index)
    {
        BlockInventoryItemType[index] = (InventoryItemType)byte.Parse(newValue);
    }
}
