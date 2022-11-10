using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class PlayerDataResponse
{
    [BsonElement(NetStrings.PLAYER_USERNAME_KEY)]
    public string Username { get; set; } = string.Empty;

    [BsonElement(NetStrings.TUTORIAL_STATE_FIELD_KEY)]
    public int TutorialState { get; set; }

    [BsonElement(NetStrings.CAMERA_ZOOM_LEVEL_FIELD_KEY)]
    public int CameraZoomLevel { get; set; }

    [BsonElement(NetStrings.CAMERA_ZOOM_VALUE_FIELD_KEY)]
    public float CameraZoomValue { get; set; }

    [BsonElement(NetStrings.GEMS_FIELD_KEY)]
    public int Gems { get; set; }

    [BsonElement(NetStrings.BYTE_COINS_FIELD_KEY)]
    public int ByteCoins { get; set; }

    [BsonElement(NetStrings.SLOTS_FIELD_KEY)]
    public int Slots { get; set; }

    [BsonElement(NetStrings.INVENTORY_FIELD_KEY)]
    public byte[] Inventory { get; set; } = null!;

    [BsonElement(NetStrings.INVENTORY_DATA_FIELD_KEY)]
    public BsonDocument InventoryData { get; set; } = new();

    [BsonElement(NetStrings.BELT1_FIELD_KEY)]
    public int Belt1Item { get; set; }

    [BsonElement(NetStrings.SPOTS_FIELD_KEY)]
    public BlockType[] Spots { get; set; } = null!;

    [BsonElement(NetStrings.FAMILIAR_BLOCK_TYPE_FIELD_KEY)]
    public BlockType FamiliarBlockType { get; set; }

    [BsonElement(NetStrings.PLAYER_COSTUME_FIELD_KEY)]
    public PlayerCostumeType PlayerCostume { get; set; }

    [BsonElement(NetStrings.PLAYER_COSTUME_END_TIME_FIELD_KEY)]
    public long PlayerCostumeEndTime { get; set; }

    [BsonElement(NetStrings.FAMILIAR_NAME_FIELD_KEY)]
    public string FamiliarName { get; set; } = string.Empty;

    [BsonElement(NetStrings.IS_FAMILIAR_MAX_LVL_FIELD_KEY)]
    public bool IsFamiliarMaxLvl { get; set; }

    [BsonElement(NetStrings.FACE_ANIM_FIELD_KEY)]
    public int DefaultFaceAnimationIndex { get; set; }

    [BsonElement(NetStrings.SKIN_INDEX_FIELD_KEY)]
    public int SkinColorIndex { get; set; }

    [BsonElement(NetStrings.GENDER_FIELD_KEY)]
    public Gender Gender { get; set; }

    [BsonElement(NetStrings.NORMAL_CLAIM_TIME_FIELD_KEY)]
    public long LastNormalClaimTime { get; set; }

    [BsonElement(NetStrings.VIP_CLAIM_TIME_FIELD_KEY)]
    public long LastVIPClaimTime { get; set; }

    [BsonElement(NetStrings.HAS_CLAIMED_ADDITIONAL_FIELD_KEY)]
    public bool HasClaimedAdditionalAdBasedDailyBonus { get; set; }

    [BsonElement(NetStrings.STATISTICS_FIELD_KEY)]
    public int[] Statistics { get; set; } = null!;

    [BsonElement(NetStrings.ACCOUNT_AGE_FIELD_KEY)]
    public long AccountAge { get; set; }

    [BsonElement(NetStrings.COUNTRY_CODE_FIELD_KEY)]
    public int CountryCode { get; set; }

    [BsonElement(NetStrings.VIP_END_TIME_FIELD_KEY)]
    public long VIPEndTime { get; set; }

    [BsonElement(NetStrings.NAME_CHANGE_COUNTER_FIELD_KEY)]
    public int NameChangeCounter { get; set; }

    [BsonElement(NetStrings.EXPERIENCE_AMOUNT_FIELD_KEY)]
    public int ExperienceAmount { get; set; }

    [BsonElement(NetStrings.XP_AMOUNT_FIELD_KEY)]
    public int XPAmount { get; set; }

    [BsonElement(NetStrings.NEXT_DAILY_BONUS_GIVEAWAY_FIELD_KEY)]
    public long NextDailyBonusGiveAway { get; set; }

    [BsonElement(NetStrings.NEXT_NORMAL_DAILY_BONUS_CLAIM_FIELD_KEY)]
    public long NextNormalDailyBonusClaim { get; set; }

    [BsonElement(NetStrings.NEXT_VIP_DAILY_BONUS_CLAIM_FIELD_KEY)]
    public long NextVIPDailyBonusClaim { get; set; }

    [BsonElement(NetStrings.NEXT_DAILY_ADS_REFRESH_TIME_FIELD_KEY)]
    public long NextDailyAdsRefreshTime { get; set; }

    [BsonElement(NetStrings.NEXT_DAILY_PVP_REWARDS_REFRESH_TIME_FIELD_KEY)]
    public long NextDailyPvpRewardsRefreshTime { get; set; }

    [BsonElement(NetStrings.LAST_FREE_PRIZE_CLAIM_TIME_FIELD_KEY)]
    public long LastFreePrizeClaimTime { get; set; }

    [BsonElement(NetStrings.NEXT_WELCOME_GIFT_CLAIM_TIME_FIELD_KEY)]
    public long NextWelcomeGiftClaimTime { get; set; }

    [BsonElement(NetStrings.WELCOME_GIFT_INDEX_FIELD_KEY)]
    public int WelcomeGiftIndex { get; set; }

    [BsonElement(NetStrings.PLAYER_ADMIN_STATUS_FIELD_KEY)]
    public int AdminStatus { get; set; }

    [BsonElement(NetStrings.SHOW_LOCATION_FIELD_KEY)]
    public bool ShowLocation { get; set; }

    [BsonElement(NetStrings.SHOW_ONLINE_STATUS_FIELD_KEY)]
    public bool ShowOnlineStatus { get; set; }

    [BsonElement(NetStrings.GENERIC_VERSIONING_FIELD_KEY)]
    public int[] GenericVersioning { get; set; } = null!;

    [BsonElement(NetStrings.PASSIVE_EFFECTS_FIELD_KEY)]
    public int PassiveEffects { get; set; }

    [BsonElement(NetStrings.INSTRUCTION_STATES_AMOUNTS_FIELD_KEY)]
    public int[] InstructionStatesAmounts { get; set; } = null!;

    [BsonElement(NetStrings.INSTRUCTION_STATES_FIELD_KEY)]
    public byte[] InstructionStates { get; set; } = null!;

    [BsonElement(NetStrings.ACHIEVEMENT_CURRENT_VALUES_FIELD_KEY)]
    public int[] AchievementCurrentValues { get; set; } = null!;

    [BsonElement(NetStrings.ACHIEVEMENT_COMPLETED_STATES_FIELD_KEY)]
    public byte[] AchievementsCompletedStates { get; set; } = null!;

    [BsonElement(NetStrings.ACHIEVEMENT_REWARDS_CLAIMED_FIELD_KEY)]
    public byte[] AchievementRewardsClaimed { get; set; } = null!;

    [BsonElement(NetStrings.QUEST_LIST_COUNT_FIELD_KEY)]
    public int QuestListCount { get; set; }

    [BsonElement(NetStrings.QUEST_CURRENT_ID_FIELD_KEY)]
    public string[] QuestCurrentID { get; set; } = null!;

    [BsonElement(NetStrings.QUEST_CURRENT_PHASE_FIELD_KEY)]
    public int[] QuestCurrentPhase { get; set; } = null!;

    [BsonElement(NetStrings.FACE_EXPRESSION_LIST_ID_FIELD_KEY)]
    public AnimationNames[] FaceExpressionListID { get; set; } = null!;

    [BsonElement(NetStrings.BOUGHT_EXPRESSIONS_LIST_FIELD_KEY)]
    public AnimationNames[] BoughtExpressionsList { get; set; } = null!;

    [BsonElement(NetStrings.ALREADY_BOUGHT_ONE_TIME_ITEMS_FIELD_KEY)]
    public string[] AlreadyBoughtOneTimeItems { get; set; } = null!;

    [BsonElement(NetStrings.DAILY_QUEST_NEXT_AVAILABLE_LIST_FIELD_KEY)]
    public long[] DailyQuestNextAvailList { get; set; } = null!;

    [BsonElement(NetStrings.PREVIOUS_THREE_DAILY_QUEST_IDS_FIELD_KEY)]
    public string[] PreviousThreeDailyQuestIds { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_1_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial1CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_1_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial1TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_2_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial2CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_2_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial2TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_3_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial3CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_3_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial3TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_4_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial4CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_4_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial4TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_5_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial5CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_5_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial5TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_5_INVENTORY_SIZE_KEY)]
    public int Tutorial5InventorySize { get; set; }

    [BsonElement(NetStrings.TUTORIAL_6_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial6CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_6_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial6TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_7_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial7CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_7_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial7TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_8_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial8CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_8_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial8TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_8_QUEST_VISITED_WORLDS_LIST_FIELD_KEY)]
    public int[] Tutorial8QuestVisitedWorldsList { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_9_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial9CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_9_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial9TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_9_QUEST_VISITED_WORLDS_LIST_FIELD_KEY)]
    public int[] Tutorial9QuestVisitedWorldsList { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_10_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial10CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_10_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial10TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_11_CURRENT_STEP_FIELD_KEY)]
    public int Tutorial11CurrentStep { get; set; }

    [BsonElement(NetStrings.TUTORIAL_11_TRACK_QUEST_STEP_PROGRESS_FIELD_KEY)]
    public int[] Tutorial11TrackQuestStepProgress { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_ID_LIST_FIELD_KEY)]
    public string[] TutorialIDList { get; set; } = null!;

    [BsonElement(NetStrings.TUTORIAL_QUEST_COMPLETE_STATE_FIELD_KEY)]
    public bool TutorialQuestCompleteState { get; set; }

    [BsonElement(NetStrings.LIMITED_OFFERS_FIELD_KEY)]
    public BsonDocument LimitedOffers { get; set; } = null!;

    [BsonElement(NetStrings.LIMITED_OFFERS_USED_FIELD_KEY)]
    public BsonDocument LimitedOffersUsed { get; set; } = null!;

    [BsonElement(NetStrings.IS_STARTER_FIELD_KEY)]
    public bool IsStarter { get; set; }

    [BsonElement(NetStrings.CARD_GAME_FACE_EXPRESSIONS_ENABLED_FIELD_KEY)]
    public bool CardGameFaceExpressionsEnabled { get; set; }

    [BsonElement(NetStrings.CARD_GAME_BODY_EXPRESSIONS_ENABLED_FIELD_KEY)]
    public bool CardGameBodyExpressionsEnabled { get; set; }

    [BsonElement(NetStrings.FTUE_SOLD_ITEM_IDS_FIELD_KEY)]
    public BsonDocument FTUESoldItemIDs { get; set; } = null!;

    [BsonElement(NetStrings.SKIN_COLOR_INDEX_BEFORE_OVERRIDE_FIELD_KEY)]
    public int SkinColorIndexBeforeOverride { get; set; }
}
