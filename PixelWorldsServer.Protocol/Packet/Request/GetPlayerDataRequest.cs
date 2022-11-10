using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class GetPlayerDataRequest : PacketBase
{
    [BsonElement(NetStrings.TOKEN_KEY)]
    public string Token { get; set; } = string.Empty;

    [BsonElement(NetStrings.COGNITO_ID_KEY)]
    public string CognitoId { get; set; } = string.Empty;

    [BsonElement("cgy")] // no idea what the fuck is this
    public int CGY { get; set; }
}
