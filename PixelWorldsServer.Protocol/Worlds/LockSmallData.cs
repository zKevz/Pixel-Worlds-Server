using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Worlds;

public class LockSmallData : WorldItemBase
{
    [BsonElement("playerWhoOwnsLockId")]
    public string PlayerWhoOwnsLockId { get; set; } = string.Empty;

    [BsonElement("playerWhoOwnsLockName")]
    public string PlayerWhoOwnsLockName { get; set; } = string.Empty;

    [BsonIgnore]
    public List<LockAccess> PlayersWhoHaveAccessToLock { get; set; } = new();

    [BsonElement("playersWhoHaveAccessToLock")]
    public string[] PlayersWhoHaveAccessToLockString => SerializeAccess(PlayersWhoHaveAccessToLock);

    [BsonIgnore]
    public List<LockAccess> PlayersWhoHaveMinorAccessToLock { get; set; } = new();

    [BsonElement("playersWhoHaveMinorAccessToLock")]
    public string[] PlayersWhoHaveMinorAccessToLockString => SerializeAccess(PlayersWhoHaveMinorAccessToLock);

    [BsonElement("isOpen")]
    public bool IsOpen { get; set; }

    [BsonElement("ignoreEmptyArea")]
    public bool IgnoreEmptyArea { get; set; }

    [BsonIgnore]
    public List<Vector2i> LockMapPoints { get; set; } = new();

    [BsonElement("lockMapPoints")]
    public byte[] LockMapPointsBinary => SerializeLockMapPoints();

    [BsonElement("creationTime")]
    public DateTime CreationTime { get; set; }

    [BsonElement("lastActivatedTime")]
    public DateTime LastActivatedTime { get; set; }

    [BsonElement("isBattleOn")]
    public bool IsBattleOn { get; set; }

    public LockSmallData() : base(0, BlockType.LockSmall)
    {
    }

    public LockSmallData(int itemId) : base(itemId, BlockType.LockSmall)
    {
    }

    private static string[] SerializeAccess(List<LockAccess> access)
    {
        return access.Select(x => $"{x.Id}|{x.Name}").ToArray();
    }

    private byte[] SerializeLockMapPoints()
    {
        using var ms = new MemoryStream(LockMapPoints.Count * (sizeof(int) + sizeof(int)));
        using var bw = new BinaryWriter(ms);

        foreach (var position in LockMapPoints)
        {
            bw.Write(position.X);
            bw.Write(position.Y);
        }

        return ms.ToArray();
    }
}
