using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class LoginTokenUpdateResponse : PacketBase
{
    [BsonElement(NetStrings.TOKEN_KEY)]
    public string Token { get; set; } = string.Empty;
}
