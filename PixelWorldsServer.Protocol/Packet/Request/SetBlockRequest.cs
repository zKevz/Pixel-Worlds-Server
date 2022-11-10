using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class SetBlockRequest : PacketBase
{
    [BsonElement(NetStrings.POSITION_X_KEY)]
    public int X { get; set; }

    [BsonElement(NetStrings.POSITION_Y_KEY)]
    public int Y { get; set; }

    [BsonElement(NetStrings.BLOCK_TYPE_KEY)]
    public BlockType BlockType { get; set; }
}
