using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Request;

public class CharacterCreatedRequest : PacketBase
{
    [BsonElement(NetStrings.GENDER_KEY)]
    public Gender Gender { get; set; }

    [BsonElement(NetStrings.COUNTRY_KEY)]
    public int CountryCode { get; set; }

    [BsonElement(NetStrings.SKIN_COLOR_INDEX_KEY)]
    public int SkinColorIndex { get; set; }
}
