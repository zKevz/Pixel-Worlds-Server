using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class GetPlayerDataResponse : PacketBase
{
    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement(NetStrings.PLAYER_USERNAME_KEY)]
    public string PlayerUsername { get; set; } = string.Empty;

    [BsonElement(NetStrings.REAL_USERNAME_KEY)]
    public string RealUsername { get; set; } = string.Empty;

    [BsonElement(NetStrings.PLAYER_DATA_KEY)]
    public byte[] PlayerData { get; set; } = null!;

    [BsonElement(NetStrings.EMAIL_KEY)]
    public string Email { get; set; } = string.Empty;

    [BsonElement(NetStrings.EMAIL_VERIFIED_KEY)]
    public bool EmailVerified { get; set; }

    [BsonElement(NetStrings.WORLD_NAME_KEY)]
    public string[] WorldNames { get; set; } = null!;

    [BsonElement(NetStrings.BAN_PLAYER_KEY)]
    public int BanState { get; set; }

    [BsonElement(NetStrings.NEWS_VERSION_KEY)]
    public int NewsVersion { get; set; }

    [BsonElement(NetStrings.WOTW_VERSION_KEY)]
    public int WOTWVersion { get; set; }

    [BsonElement(NetStrings.WOTW_KEY)]
    public string WOTW { get; set; } = string.Empty;
}
