using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class WearableOrWeaponChangeRequest : PacketBase
{
    [BsonElement(NetStrings.HOTSPOT_BLOCK_TYPE_KEY)]
    public BlockType HotspotBlockType { get; set; }
}
