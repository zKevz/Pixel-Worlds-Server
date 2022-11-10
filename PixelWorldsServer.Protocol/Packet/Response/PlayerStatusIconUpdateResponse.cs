using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class PlayerStatusIconUpdateResponse : PacketBase
{
    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement(NetStrings.STATUS_ICON_FIELD_KEY)]
    public StatusIconType StatusIcon { get; set; }
}
