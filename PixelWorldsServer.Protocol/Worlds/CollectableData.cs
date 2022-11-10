using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Players;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Worlds;

public class CollectableData
{
    public int Id { get; set; }

    public BlockType BlockType { get; set; }
    public InventoryItemType InventoryItemType { get; set; }

    public InventoryItemBase InventoryData { get; set; } = null!;
    public Vector2 Pos { get; set; } = new();
    public Vector2i MapPoint { get; set; } = new();

    public short Amount { get; set; }
    public bool IsGem { get; set; }
    public GemType GemType { get; set; }

    public CollectableData(int id, BlockType newType, short amount, InventoryItemType inventoryItemType, float posX, float posY, InventoryItemBase inventoryData)
    {
        Id = id;
        Pos.X = posX;
        IsGem = false;
        Pos.Y = posY;
        Amount = amount;
        MapPoint = PositionConversions.ConvertCollectablePosToMapPoint(Pos.X, Pos.Y);
        BlockType = newType;
        InventoryData = inventoryData;
        InventoryItemType = inventoryItemType;
    }

    public CollectableData(BlockType newType, short amount, InventoryItemType inventoryItemType, InventoryItemBase inventoryData)
        : this(0, newType, amount, inventoryItemType, 0f, 0f, inventoryData)
    {
    }

    public CollectableData(int id, short amount, float posX, float posY, GemType gemType)
        : this(id, BlockType.None, amount, InventoryItemType.Block, posX, posY, null!)
    {
        IsGem = true;
        GemType = gemType;
    }

    public CollectableData()
    {
    }
}
