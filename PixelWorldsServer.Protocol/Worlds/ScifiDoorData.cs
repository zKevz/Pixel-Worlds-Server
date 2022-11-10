using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Worlds;

public class ScifiDoorData : WorldItemBase
{
    [BsonElement("isLocked")]
    public bool IsLocked { get; set; }

    public ScifiDoorData() : base(0, BlockType.ScifiDoor)
    {
    }

    public ScifiDoorData(int itemId) : base(itemId, BlockType.ScifiDoor)
    {
    }
}
