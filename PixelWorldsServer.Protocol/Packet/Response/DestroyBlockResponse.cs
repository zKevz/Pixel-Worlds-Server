using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class DestroyBlockResponse : PacketBase
{
    [BsonElement(NetStrings.POSITION_X_KEY)]
    public int X { get; set; }

    [BsonElement(NetStrings.POSITION_Y_KEY)]
    public int Y { get; set; }

    [BsonElement(NetStrings.DESTROYED_BLOCK_TYPE_KEY)]
    [BsonIgnoreIfDefault]
    public BlockType BlockDestroyedBlockType { get; set; }

    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;
}
