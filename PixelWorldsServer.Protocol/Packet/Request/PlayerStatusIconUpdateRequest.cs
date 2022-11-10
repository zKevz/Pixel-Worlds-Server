using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class PlayerStatusIconUpdateRequest : PacketBase
{
    [BsonElement(NetStrings.STATUS_ICON_FIELD_KEY)]
    public StatusIconType StatusIcon { get; set; }
}
