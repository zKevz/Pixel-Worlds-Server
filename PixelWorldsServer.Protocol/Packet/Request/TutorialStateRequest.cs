using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class TutorialStateRequest : PacketBase
{
    [BsonElement(NetStrings.TUTORIAL_STATE_UPDATE_FIELD_KEY)]
    public TutorialState TutorialState { get; set; }
}
