using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class ChangeCameraZoomLevelRequest : PacketBase
{
    [BsonElement(NetStrings.CAMERA_ZOOM_LEVEL_UPDATE_FIELD_KEY)]
    public CameraZoomLevel CameraZoomLevel { get; set; }
}
