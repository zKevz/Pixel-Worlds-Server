using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Players;

namespace PixelWorldsServer.DataAccess.Models;

public class PlayerModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public int XP { get; set; }
    public int Skin { get; set; }
    public int Gems { get; set; }
    public int Level { get; set; }
    public int Slots { get; set; }
    public int OSType { get; set; }
    public int ByteCoins { get; set; }
    public int Belt1Item { get; set; }
    public int CountryCode { get; set; }
    public int QuestListCount { get; set; }
    public int PassiveEffects { get; set; }
    public int ExperienceAmount { get; set; }
    public int WelcomeGiftIndex { get; set; }
    public int NameChangeCounter { get; set; }
    public int DefaultFaceAnimationIndex { get; set; }
    public int SkinColorIndexBeforeOverride { get; set; }

    public bool IsStarter { get; set; }
    public bool ShowLocation { get; set; }
    public bool ShowOnlineStatus { get; set; }
    public bool IsFamiliarMaxLvl { get; set; }
    public bool TutorialQuestCompleteState { get; set; }
    public bool CardGameFaceExpressionsEnabled { get; set; }
    public bool CardGameBodyExpressionsEnabled { get; set; }
    public bool HasClaimedAdditionalAdBasedDailyBonus { get; set; }

    public float CameraZoomValue { get; set; }

    public string IP { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FamiliarName { get; set; } = string.Empty;

    public Gender Gender { get; set; }
    public AdminStatus AdminStatus { get; set; }
    public PlayerCostumeType Costume { get; set; }
    public BlockType FamiliarBlockType { get; set; }
    public TutorialState TutorialState { get; set; }
    public CameraZoomLevel CameraZoomLevel { get; set; } = CameraZoomLevel.Normal;

    public DateTime VIPEndTime { get; set; } = DateTime.MinValue;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime CostumeEndTime { get; set; } = DateTime.MinValue;
    public DateTime LastVIPClaimTime { get; set; } = DateTime.MinValue;
    public DateTime LastNormalClaimTime { get; set; } = DateTime.MinValue;
    public DateTime NextDailyBonusGiveAway { get; set; } = DateTime.MinValue;
    public DateTime LastFreePrizeClaimTime { get; set; } = DateTime.MinValue;
    public DateTime NextVIPDailyBonusClaim { get; set; } = DateTime.MinValue;
    public DateTime NextDailyAdsRefreshTime { get; set; } = DateTime.MinValue;
    public DateTime NextWelcomeGiftClaimTime { get; set; } = DateTime.MinValue;
    public DateTime NextNormalDailyBonusClaim { get; set; } = DateTime.MinValue;
    public DateTime NextDailyPvpRewardsRefreshTime { get; set; } = DateTime.MinValue;

    public int[] Statistics { get; set; } = null!;
    public BlockType[] Spots { get; set; } = null!;
    public int[] QuestCurrentPhase { get; set; } = null!;
    public string[] QuestCurrentID { get; set; } = null!;
    public byte[] InstructionStates { get; set; } = null!;
    public long[] DailyQuestNextAvailList { get; set; } = null!;
    public int[] InstructionStatesAmounts { get; set; } = null!;
    public int[] AchievementCurrentValues { get; set; } = null!;
    public byte[] AchievementRewardsClaimed { get; set; } = null!;
    public byte[] AchievementsCompletedStates { get; set; } = null!;
    public string[] AlreadyBoughtOneTimeItems { get; set; } = null!;
    public string[] PreviousThreeDailyQuestIds { get; set; } = null!;
    public AnimationNames[] FaceExpressionListId { get; set; } = null!;
    public AnimationNames[] BoughtExpressionsList { get; set; } = null!;

    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<int, short> Inventory { get; set; } = new();

    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<int, InventoryItemBase> InventoryItemData { get; set; } = new();

    public PlayerModel()
    {
        Statistics = Enumerable.Repeat(0, (int)StatisticsKey.LAST_VALUE).ToArray();
        Spots = new BlockType[(int)AnimationHotSpots.END_OF_THE_ENUM] // DefaultMaleHotSpotsBlockTypes
        {
            BlockType.None,
            BlockType.BasicFace,
            BlockType.BasicEyebrows,
            BlockType.BasicEyeballs,
            BlockType.None,
            BlockType.BasicMouth,
            BlockType.BasicTorso,
            BlockType.BasicTopArm,
            BlockType.BasicBottomArm,
            BlockType.BasicLegs,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.Underwear,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.None,
            BlockType.BasicEyelashes,
            BlockType.Underwear,
            BlockType.None,
            BlockType.None,
        };
        QuestCurrentID = Array.Empty<string>();
        QuestCurrentPhase = Array.Empty<int>();
        InstructionStates = new byte[(int)InstructionEventsCompleted.InstructionEventVariables_Count];
        FaceExpressionListId = new AnimationNames[]
        {
            AnimationNames.FaceContent,
            AnimationNames.FaceLaugh,
            AnimationNames.FaceAngryMutru,
            AnimationNames.FaceSad,
            AnimationNames.FaceSuspicious,
            AnimationNames.RockRock,
            AnimationNames.Applause,
            AnimationNames.ThumbsUp,
            AnimationNames.Wink,
            AnimationNames.Waveone
        };
        BoughtExpressionsList = Array.Empty<AnimationNames>();
        DailyQuestNextAvailList = new long[] { 0, 0, 0 };
        InstructionStatesAmounts = new int[] { 0 }; // only 0? idk
        AchievementCurrentValues = new int[(int)Achievement.Achievement_Count];
        AlreadyBoughtOneTimeItems = Array.Empty<string>();
        AchievementRewardsClaimed = new byte[(int)Achievement.Achievement_Count];
        PreviousThreeDailyQuestIds = Array.Empty<string>();
        AchievementsCompletedStates = new byte[(int)Achievement.Achievement_Count];
    }

    public void LoadCopy(PlayerModel playerModel)
    {
        Id = playerModel.Id;

        XP = playerModel.XP;
        Skin = playerModel.Skin;
        Gems = playerModel.Gems;
        Level = playerModel.Level;
        Slots = playerModel.Slots;
        OSType = playerModel.OSType;
        ByteCoins = playerModel.ByteCoins;
        Belt1Item = playerModel.Belt1Item;
        CountryCode = playerModel.CountryCode;
        QuestListCount = playerModel.QuestListCount;
        PassiveEffects = playerModel.PassiveEffects;
        ExperienceAmount = playerModel.ExperienceAmount;
        WelcomeGiftIndex = playerModel.WelcomeGiftIndex;
        NameChangeCounter = playerModel.NameChangeCounter;
        DefaultFaceAnimationIndex = playerModel.DefaultFaceAnimationIndex;
        SkinColorIndexBeforeOverride = playerModel.SkinColorIndexBeforeOverride;

        IsStarter = playerModel.IsStarter;
        ShowLocation = playerModel.ShowLocation;
        ShowOnlineStatus = playerModel.ShowOnlineStatus;
        IsFamiliarMaxLvl = playerModel.IsFamiliarMaxLvl;
        TutorialQuestCompleteState = playerModel.TutorialQuestCompleteState;
        CardGameBodyExpressionsEnabled = playerModel.CardGameBodyExpressionsEnabled;
        CardGameFaceExpressionsEnabled = playerModel.CardGameFaceExpressionsEnabled;
        HasClaimedAdditionalAdBasedDailyBonus = playerModel.HasClaimedAdditionalAdBasedDailyBonus;

        CameraZoomValue = playerModel.CameraZoomValue;

        IP = playerModel.IP;
        OS = playerModel.OS;
        Name = playerModel.Name;
        FamiliarName = playerModel.FamiliarName;

        Gender = playerModel.Gender;
        AdminStatus = playerModel.AdminStatus;
        Costume = playerModel.Costume;
        FamiliarBlockType = playerModel.FamiliarBlockType;
        TutorialState = playerModel.TutorialState;
        CameraZoomLevel = playerModel.CameraZoomLevel;

        VIPEndTime = playerModel.VIPEndTime;
        DateCreated = playerModel.DateCreated;
        CostumeEndTime = playerModel.CostumeEndTime;
        LastVIPClaimTime = playerModel.LastVIPClaimTime;
        LastNormalClaimTime = playerModel.LastNormalClaimTime;
        NextDailyBonusGiveAway = playerModel.NextDailyBonusGiveAway;
        LastFreePrizeClaimTime = playerModel.LastFreePrizeClaimTime;
        NextVIPDailyBonusClaim = playerModel.NextVIPDailyBonusClaim;
        NextDailyAdsRefreshTime = playerModel.NextDailyAdsRefreshTime;
        NextWelcomeGiftClaimTime = playerModel.NextWelcomeGiftClaimTime;
        NextNormalDailyBonusClaim = playerModel.NextNormalDailyBonusClaim;
        NextDailyPvpRewardsRefreshTime = playerModel.NextDailyPvpRewardsRefreshTime;

        Statistics = playerModel.Statistics;
        Spots = playerModel.Spots;
        QuestCurrentPhase = playerModel.QuestCurrentPhase;
        QuestCurrentID = playerModel.QuestCurrentID;
        InstructionStates = playerModel.InstructionStates;
        DailyQuestNextAvailList = playerModel.DailyQuestNextAvailList;
        InstructionStatesAmounts = playerModel.InstructionStatesAmounts;
        AchievementCurrentValues = playerModel.AchievementCurrentValues;
        AchievementRewardsClaimed = playerModel.AchievementRewardsClaimed;
        AchievementsCompletedStates = playerModel.AchievementsCompletedStates;
        AlreadyBoughtOneTimeItems = playerModel.AlreadyBoughtOneTimeItems;
        PreviousThreeDailyQuestIds = playerModel.PreviousThreeDailyQuestIds;
        FaceExpressionListId = playerModel.FaceExpressionListId;
        BoughtExpressionsList = playerModel.BoughtExpressionsList;

        Inventory = playerModel.Inventory;
        InventoryItemData = playerModel.InventoryItemData;
    }

    public static PlayerModel CreateCopy(PlayerModel playerModel)
    {
        PlayerModel result = new();
        result.LoadCopy(playerModel);
        return result;
    }
}
