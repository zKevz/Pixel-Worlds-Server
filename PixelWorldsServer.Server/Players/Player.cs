using MongoDB.Bson;
using PixelWorldsServer.DataAccess.Models;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Server.Worlds;
using System.Net;

namespace PixelWorldsServer.Server.Players;

public class Player : PlayerModel
{
    private bool m_ShouldDisconnect = false;
    private readonly object m_DocumentsLocker = new();
    private readonly object m_DisconnectLocker = new();
    private readonly List<BsonDocument> m_Documents = new();

    public int TutorialHitCount { get; set; }
    public bool Teleport { get; set; }

    public Direction Direction { get; set; } = Direction.Left;
    public AnimationNames Animation { get; set; } = AnimationNames.Idle;
    public StatusIconType StatusIcon { get; set; }

    public World? World { get; set; }
    public Vector2 Position { get; set; } = new();
    public IPAddress Address { get; private set; }

    public Player(IPAddress address)
    {
        IP = address.ToString();
        Address = address;
    }

    public void SendPacket<T>(T data)
    {
        var document = data.ToBsonDocument();
        lock (m_DocumentsLocker)
        {
            m_Documents.Add(document);
        }
    }

    public void Disconnect()
    {
        lock (m_DisconnectLocker)
        {
            m_ShouldDisconnect = true;
        }
    }

    public bool IsDisconnected()
    {
        lock (m_DisconnectLocker)
        {
            return m_ShouldDisconnect;
        }
    }

    public BsonDocument ConsumePackets()
    {
        lock (m_DocumentsLocker)
        {
            BsonDocument document = new();
            for (int i = 0; i < m_Documents.Count; ++i)
            {
                document.Add($"m{i}", m_Documents[i]);
            }

            document.Add("mc", m_Documents.Count);
            m_Documents.Clear();

            return document;
        }
    }

    public byte[] PackInventory()
    {
        using var ms = new MemoryStream(Inventory.Count * (sizeof(int) + sizeof(short)));
        using var bw = new BinaryWriter(ms);

        foreach (var (id, amount) in Inventory)
        {
            bw.Write((int)id);
            bw.Write(amount);
        }

        return ms.ToArray();
    }

    public static int BlockTypeAndInventoryItemTypeToInt(BlockType blockType, InventoryItemType inventoryItemType)
    {
        return (int)((uint)inventoryItemType << 24) | (int)blockType;
    }

    public bool CanFitItem(BlockType blockType, InventoryItemType inventoryItemType, short amount)
    {
        var key = BlockTypeAndInventoryItemTypeToInt(blockType, inventoryItemType);
        if (Inventory.TryGetValue(key, out var currentAmount))
        {
            if (amount + currentAmount > 999)
            {
                return false;
            }

            Inventory[key] += currentAmount;
        }
        else if (Inventory.Count >= Slots)
        {
            return false;
        }

        return true;
    }

    public bool HasItem(BlockType blockType, InventoryItemType inventoryItemType)
    {
        int key = BlockTypeAndInventoryItemTypeToInt(blockType, inventoryItemType);
        return Inventory.ContainsKey(key);
    }

    public void AddItem(BlockType blockType, InventoryItemType inventoryItemType, short amount)
    {
        int key = BlockTypeAndInventoryItemTypeToInt(blockType, inventoryItemType);
        if (Inventory.ContainsKey(key))
        {
            Inventory[key] += amount;
        }
        else
        {
            Inventory.Add(key, amount);
        }
    }

    public void RemoveItem(BlockType blockType, InventoryItemType inventoryItemType, short amount)
    {
        int key = BlockTypeAndInventoryItemTypeToInt(blockType, inventoryItemType);

        Inventory[key] -= amount;

        if (Inventory[key] <= 0)
        {
            Inventory.Remove(key);
        }
    }
}
