using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class WorldChatMessageRequest : PacketBase
{
    [BsonElement(NetStrings.MESSAGE_KEY)]
    public string Message { get; set; } = string.Empty;
}
