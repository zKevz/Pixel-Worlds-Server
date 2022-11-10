using PixelWorldsServer.DataAccess.Models;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Packet.Response;
using PixelWorldsServer.Protocol.Players;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Protocol.Worlds;
using PixelWorldsServer.Server.Players;
using System.Collections.Concurrent;

namespace PixelWorldsServer.Server.Worlds;

public class World : WorldModel
{
    private int m_BedrockRows;

    public ConcurrentDictionary<string, Player> Players { get; set; } = new();

    public bool AddPlayer(Player player)
    {
        player.World = this;
        return Players.TryAdd(player.Id, player);
    }

    public bool RemovePlayer(Player player)
    {
        player.World = null;

        if (Players.TryRemove(player.Id, out _))
        {
            if (!Players.IsEmpty)
            {
                var notification = new PlayerLeftResponse()
                {
                    ID = NetStrings.PLAYER_LEFT_KEY,
                    PlayerId = player.Id
                };

                foreach (var (_, otherPlayer) in Players)
                {
                    otherPlayer.SendPacket(notification);
                }
            }

            return true;
        }

        return false;
    }

    public void ResetCollectables()
    {
        InventoryId = 0;
        Collectables.Clear();
    }


    private void SetDefaultWorldSize()
    {
        Size.X = 80;
        Size.Y = 60;
    }

    private void SetDefaultBedrockRows()
    {
        m_BedrockRows = 3;
    }

    public LayerBlock GetBlock(int index)
    {
        return BlockLayer[index];
    }

    public LayerBlock GetBlock(int x, int y)
    {
        return GetBlock(x + y * Size.X);
    }

    public SeedData? GetSeed(int index)
    {
        return PlantedSeeds[index];
    }

    public SeedData? GetSeed(int x, int y)
    {
        return GetSeed(x + y * Size.X);
    }

    public LayerBlockBackground GetBackground(int index)
    {
        return BlockBackgroundLayer[index];
    }

    public LayerBlockBackground GetBackground(int x, int y)
    {
        return GetBackground(x + y * Size.X);
    }

    public void SetSeed(int index, SeedData seed)
    {
        PlantedSeeds[index] = seed;
    }

    public void SetSeed(int x, int y, SeedData seed)
    {
        SetSeed(x + y * Size.X, seed);
    }

    public void SetBlock(int index, BlockType blockType)
    {
        BlockLayer[index].BlockType = blockType;
        BlockLayer[index].LastHitTime = DateTime.UtcNow;
        BlockLayer[index].HitsRequired = ConfigData.BlockHitPoints[(int)blockType];
    }

    public void SetBlock(int x, int y, BlockType blockType)
    {
        SetBlock(x + y * Size.X, blockType);
    }

    public void SetBlockBackground(int index, BlockType blockType)
    {
        BlockBackgroundLayer[index].BlockType = blockType;
        BlockBackgroundLayer[index].LastHitTime = DateTime.UtcNow;
        BlockBackgroundLayer[index].HitsRequired = ConfigData.BlockHitPoints[(int)blockType];
    }

    public void SetBlockBackground(int x, int y, BlockType blockType)
    {
        SetBlockBackground(x + y * Size.X, blockType);
    }

    public void InitBlocks()
    {
        int size = Size.X * Size.Y;
        for (int i = 0; i < size; ++i)
        {
            ItemDatas.Add(null);
            BlockLayer.Add(new LayerBlock());
            PlantedSeeds.Add(null);
            BlockWaterLayer.Add(new LayerBlock());
            BlockWiringLayer.Add(new LayerWiring());
            BlockBackgroundLayer.Add(new LayerBlockBackground());
        }
    }


    public void Init(string name, WorldLayoutType worldLayoutType)
    {
        Name = name;

        switch (worldLayoutType)
        {
            case WorldLayoutType.Basic:
            case WorldLayoutType.BasicWithBots:
            case WorldLayoutType.BasicWithCollectables:
            {
                LayerBackgroundType = LayerBackgroundType.ForestBackground;
                SetDefaultWorldSize();
                SetDefaultBedrockRows();
                InitBlocks();

                int bottomLayerHeight = 10;
                int middleLayerHeight = 16;
                int topLayerHeight = 1;

                ShuffleBag<BlockType> bottomLayerShuffleBag = new();
                ShuffleBag<BlockType> middleLayerShuffleBag = new();
                ShuffleBag<BlockType> topLayerShuffleBag = new();

                {
                    int obsidianCount = 7;
                    int marbleCount = 7;
                    int lavaCount = 25;
                    int graniteCount = 20;
                    int soilBlockCount = bottomLayerHeight * Size.X - lavaCount - graniteCount - obsidianCount - marbleCount;

                    bottomLayerShuffleBag.Add(BlockType.SoilBlock, soilBlockCount);
                    bottomLayerShuffleBag.Add(BlockType.Lava, lavaCount);
                    bottomLayerShuffleBag.Add(BlockType.Granite, graniteCount);
                    bottomLayerShuffleBag.Add(BlockType.Obsidian, obsidianCount);
                    bottomLayerShuffleBag.Add(BlockType.Marble, marbleCount);
                }

                {

                    int graniteCount = 40;
                    int soilBlockCount = middleLayerHeight * Size.X - graniteCount;

                    middleLayerShuffleBag.Add(BlockType.SoilBlock, soilBlockCount);
                    middleLayerShuffleBag.Add(BlockType.Granite, graniteCount);
                }

                {
                    int soilBlockCount = topLayerHeight * Size.X;

                    topLayerShuffleBag.Add(BlockType.SoilBlock, soilBlockCount);
                }

                for (int i = 0; i < Size.X; i++)
                {
                    for (int j = 0; j < m_BedrockRows + bottomLayerHeight + middleLayerHeight + topLayerHeight; j++)
                    {
                        var position = new Vector2i(i, j);

                        if (j < m_BedrockRows)
                        {
                            if (j < m_BedrockRows - 2)
                            {
                                SetBlock(i, j, BlockType.EndLava);
                            }
                            else if (j < m_BedrockRows - 1)
                            {
                                SetBlock(i, j, BlockType.EndLavaRock);
                            }
                            else
                            {
                                SetBlock(i, j, BlockType.Bedrock);
                            }

                            continue;
                        }

                        SetBlockBackground(position.X, position.Y, BlockType.CaveWall);

                        if (j < m_BedrockRows + bottomLayerHeight)
                        {
                            SetBlock(position.X, position.Y, bottomLayerShuffleBag.Next());
                        }
                        else if (j >= m_BedrockRows + bottomLayerHeight + middleLayerHeight)
                        {
                            SetBlock(position.X, position.Y, topLayerShuffleBag.Next());
                        }
                        else
                        {
                            SetBlock(position.X, position.Y, middleLayerShuffleBag.Next());
                        }
                    }
                }

                StartingPoint = new Vector2i(Size.X / 2, Size.Y / 2);
                SetBlock(StartingPoint.X, StartingPoint.Y, BlockType.EntrancePortal);
                break;
            }

            default:
            {
                throw new Exception("Invalid world layout type");
            }
        }
    }

    public CollectableData AddCollectable(BlockType blockType, short amount, InventoryItemType inventoryItemType, Vector2i mapPoint, InventoryItemBase inventoryData)
    {
        var newInventoryId = InventoryId++;
        var posX = mapPoint.X + RollDrops.RollPosition(ConfigData.RollCollectableXMin, ConfigData.RollCollectableXMax) * 0.01f;
        var posY = mapPoint.Y + RollDrops.RollPosition(ConfigData.RollCollectableYMin, ConfigData.RollCollectableYMax) * 0.01f;
        var collectableData = new CollectableData(newInventoryId, blockType, amount, inventoryItemType, posX, posY, inventoryData);
        Collectables.Add(collectableData);
        return collectableData;
    }

    public CollectableData AddCollectable(BlockType blockType, short amount, InventoryItemType inventoryItemType, float x, float y)
    {
        var newInventoryId = InventoryId++;
        var collectableData = new CollectableData(newInventoryId, blockType, amount, inventoryItemType, x, y, null!);
        Collectables.Add(collectableData);
        return collectableData;
    }

    public CollectableData[] AddCollectableGems(short gemAmount, Vector2i mapPoint)
    {
        var list = new List<CollectableData>();
        var array = ConfigData.SplitGemValueToGems(gemAmount);

        for (int i = 0; i < array.Length; i++)
        {
            int num = array[i];
            for (int j = 0; j < num; j++)
            {
                int newInventoryId = InventoryId++;
                var posX = mapPoint.X + RollDrops.RollPosition(ConfigData.RollCollectableXMin, ConfigData.RollCollectableXMax) * 0.01f;
                var posY = mapPoint.Y + RollDrops.RollPosition(ConfigData.RollCollectableYMin, ConfigData.RollCollectableYMax) * 0.01f;

                var item = new CollectableData(newInventoryId, 1, posX, posY, (GemType)i);
                Collectables.Add(item);
                list.Add(item);
            }
        }

        return list.ToArray();
    }

    public List<CollectableData>? RandomizeCollectablesForDestroyedBlock(Vector2i mapPoint, BlockType blockType)
    {
        var list = new List<CollectableData>();

        short seedsCount;
        short blocksCount;
        short gemsCount;
        short extraBlocksCount;
        BlockType extraBlock;
        if (blockType == BlockType.Tree)
        {
            var seedDataAt = PlantedSeeds[mapPoint.X + mapPoint.Y * Size.X];
            if (seedDataAt is null || seedDataAt.GrowthEndTime > DateTime.Now)
            {
                return null;
            }

            gemsCount = seedDataAt.HarvestGems;
            blockType = seedDataAt.BlockType;
            seedsCount = seedDataAt.HarvestSeeds;
            blocksCount = seedDataAt.HarvestBlocks;
            extraBlocksCount = seedDataAt.HarvestExtraBlocks;
            extraBlock = ConfigData.TreeExtraDropBlock[(int)blockType];
        }
        else
        {
            seedsCount = (short)(RollDrops.DoesBlockDropSeed(blockType) ? 1 : 0);
            blocksCount = (short)(RollDrops.DoesBlockDropBlock(blockType) ? 1 : 0);
            gemsCount = RollDrops.BlockDropsGems(blockType);
            extraBlocksCount = (short)(RollDrops.DoesBlockDropExtraBlock(blockType) ? 1 : 0);
            extraBlock = ConfigData.BlockExtraDropBlock[(int)blockType];
        }

        if (seedsCount > 0)
        {
            list.Add(AddCollectable(blockType, seedsCount, InventoryItemType.Seed, mapPoint, null!));
        }

        if (blocksCount > 0)
        {
            // (!InventoryDataFactory.DoesEnumNeedDataClass(blockType)) ? null : InventoryDataFactory.SpawnDataClassForEnum(blockType)
            list.Add(AddCollectable(blockType, blocksCount, ConfigData.BlockInventoryItemType[(int)blockType], mapPoint, null!));
        }

        if (gemsCount > 0)
        {
            list.AddRange(AddCollectableGems(gemsCount, mapPoint));
        }

        if (extraBlocksCount > 0)
        {
            // (!InventoryDataFactory.DoesEnumNeedDataClass(blockType2)) ? null : InventoryDataFactory.SpawnDataClassForEnum(blockType2)
            list.Add(AddCollectable(extraBlock, extraBlocksCount, ConfigData.BlockInventoryItemType[(int)extraBlock], mapPoint, null!));
        }

        return list;
    }
}
