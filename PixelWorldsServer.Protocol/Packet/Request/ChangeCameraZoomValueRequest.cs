using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class ChangeCameraZoomValueRequest : PacketBase
{
    [BsonElement(NetStrings.AMOUNT_KEY)]
    public float CameraZoomValue { get; set; }
}
