using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class HitBlockResponse : PacketBase
{
    [BsonElement(NetStrings.POSITION_X_KEY)]
    public int X { get; set; }

    [BsonElement(NetStrings.POSITION_Y_KEY)]
    public int Y { get; set; }

    [BsonElement(NetStrings.TOP_ARM_BLOCK_TYPE_KEY)]
    public BlockType TopArmBlockType { get; set; }

    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;
}
