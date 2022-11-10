using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class AddNetworkPlayerResponse : PacketBase
{
    [BsonElement(NetStrings.POSITION_X_KEY)]
    public float X { get; set; }

    [BsonElement(NetStrings.POSITION_Y_KEY)]
    public float Y { get; set; }

    [BsonElement(NetStrings.TIMESTAMP_KEY)]
    public long Timestamp { get; set; }

    [BsonElement(NetStrings.ANIMATION_KEY)]
    public AnimationNames Animation { get; set; }

    [BsonElement(NetStrings.DIRECTION_KEY)]
    public Direction Direction { get; set; }

    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement(NetStrings.PLAYER_USERNAME_KEY)]
    public string PlayerUsername { get; set; } = string.Empty;

    [BsonElement("D")] // idk what the fuck is this
    public int D { get; set; }

    [BsonElement(NetStrings.SPOTS_FIELD_KEY)]
    public BlockType[] Spots { get; set; } = null!;

    [BsonElement(NetStrings.FAMILIAR_BLOCK_TYPE_PLAYER_KEY)]
    public BlockType Familiar { get; set; }

    [BsonElement(NetStrings.FAMILIAR_NAME_PLAYER_KEY)]
    public string FamiliarName { get; set; } = string.Empty;

    [BsonElement(NetStrings.IS_FAMILIAR_MAX_LVL_PLAYER_KEY)]
    public bool IsFamiliarMaxLvl { get; set; }

    [BsonElement(NetStrings.IS_VIP_FIELD_KEY)]
    public bool IsVIP { get; set; }

    [BsonElement(NetStrings.VIP_END_TIME_AGE_FIELD_KEY)]
    public long VIPEndTimeAge { get; set; }

    [BsonElement(NetStrings.COUNTRY_KEY)]
    public int Country { get; set; }

    [BsonElement(NetStrings.AGE_FIELD_KEY)]
    public long Age { get; set; }

    [BsonElement(NetStrings.LEVEL_FIELD_KEY)]
    public int Level { get; set; }

    [BsonElement(NetStrings.XP_LEVEL_FIELD_KEY)]
    public int XPLevel { get; set; }

    [BsonElement(NetStrings.GEMS_AMOUNT_FIELD_KEY)]
    public int GemsAmount { get; set; }

    [BsonElement(NetStrings.GENDER_KEY)]
    public Gender Gender { get; set; }

    [BsonElement(NetStrings.SKIN_INDEX_FIELD_KEY)]
    public int SkinIndex { get; set; }

    [BsonElement(NetStrings.FACE_ANIM_FIELD_KEY)]
    public int FaceAnimIndex { get; set; }

    [BsonElement(NetStrings.IN_PORTAL_FIELD_KEY)]
    public bool CameFromPortal { get; set; }

    [BsonElement(NetStrings.STATUS_ICON_FIELD_KEY)]
    public StatusIconType StatusIcon { get; set; }
}
