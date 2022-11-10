using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class ChatMessageBinary
{
    [BsonElement(NetStrings.NICK_KEY)]
    public string Nick { get; set; } = string.Empty;

    [BsonElement(NetStrings.USER_ID_KEY)]
    public string UserId { get; set; } = string.Empty;

    [BsonElement(NetStrings.CHANNEL_KEY)]
    public string Channel { get; set; } = string.Empty;

    [BsonElement(NetStrings.CHANNEL_INDEX_KEY)]
    public int ChannelIndex { get; set; }

    [BsonElement(NetStrings.MESSAGE_CHAT_KEY)]
    public string MessageChat { get; set; } = string.Empty;

    [BsonElement(NetStrings.CHAT_TIME_KEY)]
    public DateTime Time { get; set; }
}

public class WorldChatMessageResponse : PacketBase
{
    [BsonElement(NetStrings.CHAT_MESSAGE_BINARY)]
    public ChatMessageBinary MessageBinary { get; set; } = null!;
}
