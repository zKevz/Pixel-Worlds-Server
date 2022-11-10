using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class PlayerLeftResponse : PacketBase
{
    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;
}
