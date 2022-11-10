using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class RenamePlayerResponse : PacketBase
{
    [BsonElement(NetStrings.SUCCESS_KEY)]
    public bool IsSuccess { get; set; }

    [BsonElement(NetStrings.ERROR_KEY)]
    [BsonIgnoreIfDefault]
    public int ErrorState { get; set; }
}
