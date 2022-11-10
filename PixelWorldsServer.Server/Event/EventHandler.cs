using MongoDB.Bson;
using PixelWorldsServer.DataAccess;
using PixelWorldsServer.DataAccess.Models;
using PixelWorldsServer.Protocol;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Packet;
using PixelWorldsServer.Protocol.Packet.Request;
using PixelWorldsServer.Protocol.Packet.Response;
using PixelWorldsServer.Protocol.Utils;
using PixelWorldsServer.Server.Worlds;
using System.Text.RegularExpressions;

namespace PixelWorldsServer.Server.Event;

public class EventHandler
{
    private readonly Database m_Database;
    private readonly WorldManager m_WorldManager;

    private static readonly string[] m_TutorialWorldLists = new string[11]
    {
        "TUTORIAL1",
        "TUTORIAL2",
        "TUTORIAL3",
        "TUTORIAL4",
        "TUTORIAL5",
        "TUTORIAL6",
        "TUTORIAL7",
        "TUTORIAL8",
        "TUTORIAL9",
        "TUTORIAL10",
        "TUTORIAL11"
    };

    public EventHandler(Database database, WorldManager worldManager)
    {
        m_Database = database;
        m_WorldManager = worldManager;
    }

    // Packet that we don't care or not yet implemented
    [Event(NetStrings.GET_LIVE_STREAM_INFO_KEY)]
    [Event(NetStrings.UPDATE_LOCATION_STATUS_KEY)]
    [Event(NetStrings.INSTRUCTION_EVENT_COMPLETED_KEY)]
    [Event(NetStrings.ACHIEVEMENT_KEY)]
    [Event(NetStrings.GET_SCOREBOARD_DATA_KEY)]
    [Event(NetStrings.GET_FRIEND_LIST_KEY)]
    [Event("mp")] // idk what's this
    [Event(NetStrings.READY_TO_PLAY_KEY)]
    [Event(NetStrings.TUTORIAL_STATE_UPDATE_KEY)]
    public static Task IgnoredPacket(EventContext _)
    {
        return Task.CompletedTask;
    }

    [Event(NetStrings.PING_KEY)]
    public static Task OnPing(EventContext context)
    {
        context.Player.SendPacket(new PacketBase()
        {
            ID = NetStrings.PING_KEY,
        });

        return Task.CompletedTask;
    }

    [Event(NetStrings.SYNC_TIME_KEY)]
    public static Task OnSyncTime(EventContext context)
    {
        context.Player.SendPacket(new SyncTimeResponse()
        {
            ID = NetStrings.SYNC_TIME_KEY,
            SyncTime = DateTime.UtcNow.Ticks,
            ServerSleep = 30
        });

        return Task.CompletedTask;
    }

    [Event(NetStrings.VERSION_CHECK_KEY)]
    public static Task OnVersionCheck(EventContext context, VersionCheckRequest versionCheck)
    {
        context.Player.OS = versionCheck.OS;
        context.Player.OSType = versionCheck.OsType;
        context.Player.SendPacket(new VersionCheckResponse()
        {
            ID = NetStrings.VERSION_CHECK_KEY,
            VersionNumber = 94
        });

        return Task.CompletedTask;
    }

    [Event(NetStrings.GET_PLAYER_DATA_KEY)]
    public async Task OnGetPlayerData(EventContext context, GetPlayerDataRequest request)
    {
        PlayerModel? playerModel = null;
        if (request.Token.Length == 24)
        {
            playerModel = await m_Database.GetPlayerByIdAsync(request.Token).ConfigureAwait(false);
        }

        if (playerModel is null)
        {
            playerModel = await m_Database.CreatePlayerAsync(context.Player.IP, context.Player.OS).ConfigureAwait(false);
            context.Player.SendPacket(new LoginTokenUpdateResponse()
            {
                ID = NetStrings.LOGIN_TOKEN_UPDATE_KEY,
                Token = playerModel.Id
            });
        }

        context.Player.LoadCopy(playerModel);

        var playerData = new PlayerDataResponse()
        {
            Gems = context.Player.Gems,
            Slots = context.Player.Slots,
            Spots = context.Player.Spots,
            Gender = context.Player.Gender,
            Username = context.Player.Name,
            XPAmount = context.Player.XP,
            ByteCoins = context.Player.ByteCoins,
            Inventory = context.Player.PackInventory(),
            Belt1Item = context.Player.Belt1Item,
            IsStarter = context.Player.IsStarter,
            VIPEndTime = context.Player.VIPEndTime.Ticks,
            Statistics = context.Player.Statistics,
            AccountAge = context.Player.DateCreated.Ticks,
            CountryCode = context.Player.CountryCode,
            AdminStatus = (int)context.Player.AdminStatus,
            FamiliarName = context.Player.FamiliarName,
            ShowLocation = context.Player.ShowLocation,
            PlayerCostume = context.Player.Costume,
            InventoryData = context.Player.InventoryItemData.ToBsonDocument(),
            TutorialState = (int)context.Player.TutorialState,
            QuestCurrentID = context.Player.QuestCurrentID,
            QuestListCount = context.Player.QuestListCount,
            SkinColorIndex = context.Player.Skin,
            PassiveEffects = context.Player.PassiveEffects,
            CameraZoomLevel = (int)context.Player.CameraZoomLevel,
            CameraZoomValue = context.Player.CameraZoomValue,
            ShowOnlineStatus = context.Player.ShowOnlineStatus,
            IsFamiliarMaxLvl = context.Player.IsFamiliarMaxLvl,
            LastVIPClaimTime = context.Player.LastVIPClaimTime.Ticks,
            ExperienceAmount = context.Player.ExperienceAmount, // what the fuck is the difference with XP
            WelcomeGiftIndex = context.Player.WelcomeGiftIndex,
            QuestCurrentPhase = context.Player.QuestCurrentPhase,
            NameChangeCounter = context.Player.NameChangeCounter,
            InstructionStates = context.Player.InstructionStates,
            FamiliarBlockType = context.Player.FamiliarBlockType,
            LastNormalClaimTime = context.Player.LastNormalClaimTime.Ticks,
            PlayerCostumeEndTime = context.Player.CostumeEndTime.Ticks,
            FaceExpressionListID = context.Player.FaceExpressionListId,
            BoughtExpressionsList = context.Player.BoughtExpressionsList,
            NextVIPDailyBonusClaim = context.Player.NextVIPDailyBonusClaim.Ticks,
            NextDailyBonusGiveAway = context.Player.NextDailyBonusGiveAway.Ticks,
            LastFreePrizeClaimTime = context.Player.LastFreePrizeClaimTime.Ticks,
            NextDailyAdsRefreshTime = context.Player.NextDailyAdsRefreshTime.Ticks,
            DailyQuestNextAvailList = context.Player.DailyQuestNextAvailList,
            NextWelcomeGiftClaimTime = context.Player.NextWelcomeGiftClaimTime.Ticks,
            InstructionStatesAmounts = context.Player.InstructionStatesAmounts,
            AchievementCurrentValues = context.Player.AchievementCurrentValues,
            AchievementRewardsClaimed = context.Player.AchievementRewardsClaimed,
            DefaultFaceAnimationIndex = context.Player.DefaultFaceAnimationIndex,
            NextNormalDailyBonusClaim = context.Player.NextNormalDailyBonusClaim.Ticks,
            AlreadyBoughtOneTimeItems = context.Player.AlreadyBoughtOneTimeItems,
            PreviousThreeDailyQuestIds = context.Player.PreviousThreeDailyQuestIds,
            TutorialQuestCompleteState = context.Player.TutorialQuestCompleteState,
            AchievementsCompletedStates = context.Player.AchievementsCompletedStates,
            SkinColorIndexBeforeOverride = context.Player.SkinColorIndexBeforeOverride,
            NextDailyPvpRewardsRefreshTime = context.Player.NextDailyPvpRewardsRefreshTime.Ticks,
            CardGameFaceExpressionsEnabled = context.Player.CardGameFaceExpressionsEnabled,
            CardGameBodyExpressionsEnabled = context.Player.CardGameBodyExpressionsEnabled,
            HasClaimedAdditionalAdBasedDailyBonus = context.Player.HasClaimedAdditionalAdBasedDailyBonus,

            Tutorial1CurrentStep = 0,
            Tutorial1TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial2CurrentStep = 0,
            Tutorial2TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial3CurrentStep = 0,
            Tutorial3TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial4CurrentStep = 0,
            Tutorial4TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial5CurrentStep = 0,
            Tutorial5TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial5InventorySize = 0,
            Tutorial6CurrentStep = 0,
            Tutorial6TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial7CurrentStep = 0,
            Tutorial7TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial8CurrentStep = 0,
            Tutorial8TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial8QuestVisitedWorldsList = Array.Empty<int>(),
            Tutorial9CurrentStep = 0,
            Tutorial9TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial9QuestVisitedWorldsList = Array.Empty<int>(),
            Tutorial10CurrentStep = 0,
            Tutorial10TrackQuestStepProgress = Array.Empty<int>(),
            Tutorial11CurrentStep = 0,
            Tutorial11TrackQuestStepProgress = Array.Empty<int>(),
            TutorialIDList = m_TutorialWorldLists,
            LimitedOffers = new BsonDocument()
            {
                { "Count", 0 }
            },
            LimitedOffersUsed = new BsonDocument()
            {
                { "Count", 0 }
            },
            FTUESoldItemIDs = new BsonDocument()
            {
                { "Count", 0 }
            },
            GenericVersioning = new int[] { 62 },
        };

        var response = new GetPlayerDataResponse()
        {
            ID = NetStrings.GET_PLAYER_DATA_KEY,
            WOTW = "NUTS",
            Email = "",
            BanState = 0,
            PlayerId = context.Player.Id.ToUpper(),
            WorldNames = Array.Empty<string>(),
            PlayerData = playerData.ToBson(),
            NewsVersion = 62,
            WOTWVersion = 225,
            RealUsername = context.Player.Name,
            EmailVerified = false,
            PlayerUsername = context.Player.Name,
        };

        context.Player.SendPacket(response);
    }

    [Event(NetStrings.MENU_WORLD_LOAD_INFO_KEY)]
    public async Task OnMenuWorldLoadInfo(EventContext context, MenuWorldLoadInfoRequest request)
    {
        request.WorldName = request.WorldName.ToUpper();

        var worldCache = await m_WorldManager.GetExistingWorldAsync(request.WorldName).ConfigureAwait(false);
        if (worldCache is not null)
        {
            context.Player.SendPacket(new MenuWorldLoadInfoResponse()
            {
                ID = NetStrings.MENU_WORLD_LOAD_INFO_KEY,
                Count = worldCache.Players.Count,
                WorldName = request.WorldName
            });
            return;
        }

        var worldModel = await m_Database.GetWorldByNameAsync(request.WorldName).ConfigureAwait(false);
        if (worldModel is not null)
        {
            context.Player.SendPacket(new MenuWorldLoadInfoResponse()
            {
                ID = NetStrings.MENU_WORLD_LOAD_INFO_KEY,
                Count = 0,
                WorldName = request.WorldName
            });
            return;
        }

        context.Player.SendPacket(new MenuWorldLoadInfoResponse()
        {
            ID = NetStrings.MENU_WORLD_LOAD_INFO_KEY,
            Count = -3,
            WorldName = request.WorldName
        });
    }

    private static bool IsNameOk(string playerName)
    {
        if (playerName.Length < 2 || playerName.Length > 15)
        {
            return false;
        }

        return Regex.IsMatch(playerName.ToUpper(), "^([][A-Z_^{}][][A-Z_0-9^{}-]+)$");
    }

    [Event(NetStrings.RENAME_PLAYER_KEY)]
    public async Task OnRenamePlayer(EventContext context, RenamePlayerRequest request)
    {
        var response = new RenamePlayerResponse()
        {
            ID = NetStrings.RENAME_PLAYER_KEY,
        };

        if (!context.Player.Name.StartsWith("SUBJECT_"))
        {
            response.IsSuccess = false;
            response.ErrorState = 8; // Already created a name
        }
        else if (!IsNameOk(request.PlayerName))
        {
            response.IsSuccess = false;
            response.ErrorState = 6; // Invalid name
        }
        else if (await m_Database.GetPlayerByNameAsync(request.PlayerName).ConfigureAwait(false) is not null)
        {
            response.IsSuccess = false;
            response.ErrorState = 7; // Already exist
        }
        else
        {
            response.IsSuccess = true;
            context.Player.Name = request.PlayerName;
            await m_Database.UpdatePlayerNameAsync(context.Player.Id, request.PlayerName).ConfigureAwait(false);
        }

        context.Player.SendPacket(response);
    }

    [Event(NetStrings.TRY_TO_JOIN_WORLD_KEY)]
    public async Task OnTryToJoinWorld(EventContext context, TryToJoinWorldRequest request)
    {
        request.World = request.World.ToUpper();

        var response = new TryToJoinWorldResponse()
        {
            ID = NetStrings.TRY_TO_JOIN_WORLD_KEY,
            Biome = request.Biome,
            WorldName = request.World,
            JoinResult = WorldJoinResult.Ok,
        };

        if (request.Biome != BasicWorldBiome.Forest)
        {
            response.JoinResult = WorldJoinResult.WorldUnavailable;
        }
        else if (await m_Database.GetPlayerByNameAsync(request.World).ConfigureAwait(false) is not null)
        {
            response.JoinResult = WorldJoinResult.AlreadyHere;
        }
        else if (context.Player.TutorialState != TutorialState.NotStarted && m_TutorialWorldLists.Contains(request.World))
        {
            response.JoinResult = WorldJoinResult.NotAllowed;
        }
        else
        {
            await m_WorldManager.GetWorldAsync(request.World).ConfigureAwait(false); // Load / generate
        }

        context.Player.SendPacket(response);
    }

    [Event(NetStrings.GET_WORLD_KEY)]
    public async Task GetWorldAsync(EventContext context, GetWorldRequest request)
    {
        request.World = request.World.ToUpper();

        World? world;
        if (m_TutorialWorldLists.Contains(request.World))
        {
            world = m_WorldManager.CopyTutorialWorld(request.World); // Copy world so that it doesn't override with other players
        }
        else
        {
            world = await m_WorldManager.GetExistingWorldAsync(request.World).ConfigureAwait(false);
        }

        if (world is null)
        {
            throw new Exception("Nice try :D");
        }

        var worldData = new GetWorldResponse()
        {
            ID = NetStrings.GET_WORLD_KEY,
            Size = world.Size,
            ItemId = world.ItemId,
            WorldId = world.Id,
            ItemDatas = world.ItemDatas,
            BlockLayer = world.BlockLayer,
            LayoutType = world.LayoutType,
            MusicIndex = world.MusicIndex,
            InventoryId = world.InventoryId,
            WeatherType = world.WeatherType,
            GravityMode = world.GravityMode,
            PlantedSeeds = world.PlantedSeeds,
            LightingType = world.LightingType,
            StartingPoint = world.StartingPoint,
            BlockWaterLayer = world.BlockWaterLayer,
            BlockWiringLayer = world.BlockWiringLayer,
            LayerBackgroundType = world.LayerBackgroundType,
            BlockBackgroundLayer = world.BlockBackgroundLayer,
        };

        var response = new GetWorldCompressedResponse()
        {
            ID = NetStrings.GET_WORLD_COMPRESSED_KEY,
            WorldData = LZMATools.Compress(worldData.ToBson()),
        };

        if (context.Player.TutorialState == TutorialState.NotStarted && world.Name == "PIXELSTATION")
        {
            context.Player.TutorialState = TutorialState.TutorialCompleted;
        }

        if (!world.AddPlayer(context.Player))
        {
            throw new Exception("Cannot add player to the world player list!");
        }

        context.Player.SendPacket(response);
    }

    [Event(NetStrings.CAMERA_ZOOM_LEVEL_UPDATE_KEY)]
    public static Task OnChangeCameraZoomLevel(EventContext context, ChangeCameraZoomLevelRequest request)
    {
        context.Player.CameraZoomLevel = request.CameraZoomLevel;
        return Task.CompletedTask;
    }

    [Event(NetStrings.CAMERA_ZOOM_VALUE_UPDATE_KEY)]
    public static Task OnChangeCameraZoomValue(EventContext context, ChangeCameraZoomValueRequest request)
    {
        context.Player.CameraZoomValue = request.CameraZoomValue;
        return Task.CompletedTask;
    }

    [Event(NetStrings.REQUEST_OTHER_PLAYER_KEY)]
    public static Task OnRequestOtherPlayer(EventContext context)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Nice try");
        }

        context.Player.SendPacket(new PacketBase()
        {
            ID = NetStrings.REQUEST_OTHER_PLAYER_KEY
        });
        context.Player.Position = PositionConversions.ConvertPlayerMapPointToWorldPoint(world.StartingPoint.X, world.StartingPoint.Y);

        var response = new AddNetworkPlayerResponse()
        {
            D = 0,
            X = context.Player.Position.X,
            Y = context.Player.Position.Y,
            ID = NetStrings.ADD_NETWORK_PLAYER_KEY,
            Age = context.Player.DateCreated.Ticks,
            Level = context.Player.Level,
            Spots = context.Player.Spots,
            IsVIP = context.Player.AdminStatus == AdminStatus.Admin,
            Gender = context.Player.Gender,
            XPLevel = context.Player.XP,
            Country = context.Player.CountryCode,
            PlayerId = context.Player.Id,
            Familiar = context.Player.FamiliarBlockType,
            Timestamp = 0,
            SkinIndex = context.Player.Skin,
            Animation = context.Player.Animation,
            Direction = context.Player.Direction,
            GemsAmount = context.Player.Gems,
            StatusIcon = context.Player.StatusIcon,
            FamiliarName = context.Player.FamiliarName,
            VIPEndTimeAge = context.Player.VIPEndTime.Ticks,
            FaceAnimIndex = context.Player.DefaultFaceAnimationIndex,
            PlayerUsername = context.Player.Name,
            CameFromPortal = true,
            IsFamiliarMaxLvl = context.Player.IsFamiliarMaxLvl,
        };

        foreach (var (id, player) in world.Players)
        {
            if (id != context.Player.Id)
            {
                context.Player.SendPacket(new AddNetworkPlayerResponse()
                {
                    D = 0,
                    X = player.Position.X,
                    Y = player.Position.Y,
                    ID = NetStrings.ADD_NETWORK_PLAYER_KEY,
                    Age = player.DateCreated.Ticks,
                    Level = player.Level,
                    Spots = player.Spots,
                    IsVIP = player.AdminStatus == AdminStatus.Admin,
                    Gender = player.Gender,
                    XPLevel = player.XP,
                    Country = player.CountryCode,
                    PlayerId = player.Id,
                    Familiar = player.FamiliarBlockType,
                    Timestamp = 0,
                    SkinIndex = player.Skin,
                    Animation = player.Animation,
                    Direction = player.Direction,
                    GemsAmount = player.Gems,
                    StatusIcon = player.StatusIcon,
                    FamiliarName = player.FamiliarName,
                    VIPEndTimeAge = player.VIPEndTime.Ticks,
                    FaceAnimIndex = player.DefaultFaceAnimationIndex,
                    CameFromPortal = false,
                    PlayerUsername = player.Name,
                    IsFamiliarMaxLvl = player.IsFamiliarMaxLvl,
                });

                player.SendPacket(response);
            }
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.REQUEST_AI_ENEMY_KEY)]
    public static Task OnRequestAIEnemy(EventContext context)
    {
        context.Player.SendPacket(new PacketBase()
        {
            ID = NetStrings.REQUEST_AI_ENEMY_KEY
        });

        return Task.CompletedTask;
    }

    [Event(NetStrings.REQUEST_AI_PETS_KEY)]
    public static Task OnRequestAIPets(EventContext context)
    {
        context.Player.SendPacket(new PacketBase()
        {
            ID = NetStrings.REQUEST_AI_PETS_KEY
        });

        return Task.CompletedTask;
    }

    [Event(NetStrings.MY_POSITION_KEY)]
    public static Task OnMyPosition(EventContext context, MyPosRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        if (request.X == 0 && request.Y == 0)
        {
            return Task.CompletedTask;
        }

        context.Player.Teleport = request.Teleport;
        context.Player.Direction = request.Direction;
        context.Player.Animation = request.Animation;
        context.Player.Position.X = request.X;
        context.Player.Position.Y = request.Y;

        if (world.Players.Count > 1)
        {
            var response = new MyPosResponse()
            {
                X = request.X,
                Y = request.Y,
                ID = NetStrings.MY_POSITION_KEY,
                Teleport = request.Teleport,
                PlayerId = context.Player.Id,
                Direction = request.Direction,
                Animation = request.Animation
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(response);
                }
            }
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.CHARACTER_CREATED_KEY)]
    public static Task OnCharacterCreated(EventContext context, CharacterCreatedRequest request)
    {
        if (context.Player.TutorialState != TutorialState.NotStarted)
        {
            throw new Exception("Nice try");
        }

        context.Player.Skin = request.SkinColorIndex;
        context.Player.Gender = request.Gender;
        context.Player.CountryCode = request.CountryCode;

        context.Player.AddItem(BlockType.GemSoil, InventoryItemType.Seed, 1);
        context.Player.AddItem(BlockType.GemSoil, InventoryItemType.Block, 4);
        context.Player.AddItem(BlockType.Fertilizer, InventoryItemType.Block, 1);
        context.Player.AddItem(BlockType.LockWorldNoob, InventoryItemType.Block, 1);
        context.Player.AddItem(BlockType.ConsumableRedScroll, InventoryItemType.Consumable, 1);

        if (context.Player.Gender == Gender.Male)
        {
            context.Player.AddItem(BlockType.JumpsuitMale, InventoryItemType.WearableItem, 1);
            context.Player.AddItem(BlockType.HatJumpsuitMale, InventoryItemType.WearableItem, 1);
        }
        else
        {
            context.Player.AddItem(BlockType.JumpsuitFemale, InventoryItemType.WearableItem, 1);
            context.Player.AddItem(BlockType.HatJumpsuitFemale, InventoryItemType.WearableItem, 1);
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.SET_BLOCK_KEY)]
    public static Task OnSetBlock(EventContext context, SetBlockRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        var block = world.GetBlock(request.X, request.Y);
        if (block.BlockType != BlockType.None)
        {
            return Task.CompletedTask;
        }

        if (!context.Player.HasItem(request.BlockType, InventoryItemType.Block))
        {
            return Task.CompletedTask;
        }

        if (ConfigData.BlockInventoryItemType[(int)request.BlockType] != InventoryItemType.Block)
        {
            throw new Exception("Not a block");
        }

        world.SetBlock(request.X, request.Y, request.BlockType);
        context.Player.RemoveItem(request.BlockType, InventoryItemType.Block, 1);

        foreach (var (_, player) in world.Players)
        {
            player.SendPacket(new SetBlockResponse()
            {
                ID = NetStrings.SET_BLOCK_KEY,
                X = request.X,
                Y = request.Y,
                PlayerId = context.Player.Id,
                BlockType = request.BlockType
            });
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.SET_BLOCK_BACKGROUND_KEY)]
    public static Task OnSetBlockBackground(EventContext context, SetBlockRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        if (!context.Player.HasItem(request.BlockType, InventoryItemType.BlockBackground))
        {
            return Task.CompletedTask;
        }

        if (ConfigData.BlockInventoryItemType[(int)request.BlockType] != InventoryItemType.BlockBackground)
        {
            throw new Exception("Not a background");
        }

        world.SetBlockBackground(request.X, request.Y, request.BlockType);
        context.Player.RemoveItem(request.BlockType, InventoryItemType.BlockBackground, 1);

        var response = new SetBlockResponse()
        {
            ID = NetStrings.SET_BLOCK_BACKGROUND_KEY,
            X = request.X,
            Y = request.Y,
            PlayerId = context.Player.Id,
            BlockType = request.BlockType
        };
        foreach (var (_, player) in world.Players)
        {
            player.SendPacket(response);
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.HIT_BLOCK_KEY)]
    public static Task OnHitBlock(EventContext context, HitBlockRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        var block = world.GetBlock(request.X, request.Y);
        if (block.BlockType == BlockType.None)
        {
            return Task.CompletedTask;
        }

        if (block.BlockType == BlockType.Tree)
        {
            var seed = world.GetSeed(request.X, request.Y);
            if (seed is null)
            {
                // what the fuck
                throw new Exception($"No seed in {request.X},{request.Y}");
            }

            if (DateTime.UtcNow >= seed.GrowthEndTime)
            {
                block.HitsRequired = 0;
            }
        }
        else
        {
            if ((DateTime.UtcNow - block.LastHitTime).TotalMilliseconds >= ConfigData.ReviveBlockTime * 1000)
            {
                block.HitsRequired = ConfigData.BlockHitPoints[(int)block.BlockType];
            }
        }

        block.HitsRequired -= 200;
        block.LastHitTime = DateTime.UtcNow;

        if (world.Players.Count > 1)
        {
            var hitResponse = new HitBlockResponse()
            {
                X = request.X,
                Y = request.Y,
                ID = NetStrings.HIT_BLOCK_KEY,
                PlayerId = context.Player.Id,
                TopArmBlockType = BlockType.None,
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(hitResponse);
                }
            }
        }

        if (block.HitsRequired <= 0)
        {
            var response = new DestroyBlockResponse()
            {
                ID = NetStrings.DESTROY_BLOCK_KEY,
                X = request.X,
                Y = request.Y,
                PlayerId = context.Player.Id,
                BlockDestroyedBlockType = block.BlockType
            };

            foreach (var (_, player) in world.Players)
            {
                player.SendPacket(response);
            }

            var collectables = world.RandomizeCollectablesForDestroyedBlock(new Vector2i(request.X, request.Y), block.BlockType);
            if (collectables is not null)
            {
                foreach (var collectable in collectables)
                {
                    var collectResponse = new CollectResponse()
                    {
                        ID = NetStrings.NEW_COLLECTABLE_KEY,
                        IsGem = collectable.IsGem,
                        Amount = collectable.Amount,
                        GemType = collectable.GemType,
                        BlockType = collectable.BlockType,
                        PositionX = collectable.Pos.X,
                        PositionY = collectable.Pos.Y,
                        CollectableId = collectable.Id,
                        InventoryType = collectable.InventoryItemType,
                    };

                    foreach (var (_, player) in world.Players)
                    {
                        player.SendPacket(collectResponse);
                    }
                }
            }

            block.BlockType = BlockType.None;
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.HIT_BLOCK_BACKGROUND_KEY)]
    public static Task OnHitBlockBackground(EventContext context, HitBlockRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        var background = world.GetBackground(request.X, request.Y);
        if (background.BlockType == BlockType.None)
        {
            return Task.CompletedTask;
        }

        if ((DateTime.UtcNow - background.LastHitTime).TotalMilliseconds >= ConfigData.ReviveBlockTime * 1000)
        {
            background.HitsRequired = ConfigData.BlockHitPoints[(int)background.BlockType];
        }

        background.HitsRequired -= 200;
        background.LastHitTime = DateTime.UtcNow;

        if (world.Players.Count > 1)
        {
            var hitResponse = new HitBlockResponse()
            {
                X = request.X,
                Y = request.Y,
                ID = NetStrings.HIT_BLOCK_BACKGROUND_KEY,
                PlayerId = context.Player.Id,
                TopArmBlockType = BlockType.None,
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(hitResponse);
                }
            }
        }

        if (background.HitsRequired <= 0)
        {
            var response = new DestroyBlockResponse()
            {
                ID = NetStrings.DESTROY_BLOCK_KEY,
                X = request.X,
                Y = request.Y,
                PlayerId = context.Player.Id,
                BlockDestroyedBlockType = background.BlockType
            };

            foreach (var (_, player) in world.Players)
            {
                player.SendPacket(response);
            }

            var collectables = world.RandomizeCollectablesForDestroyedBlock(new Vector2i(request.X, request.Y), background.BlockType);
            if (collectables is not null)
            {
                foreach (var collectable in collectables)
                {
                    var collectResponse = new CollectResponse()
                    {
                        ID = NetStrings.NEW_COLLECTABLE_KEY,
                        IsGem = collectable.IsGem,
                        Amount = collectable.Amount,
                        GemType = collectable.GemType,
                        BlockType = collectable.BlockType,
                        PositionX = collectable.Pos.X,
                        PositionY = collectable.Pos.Y,
                        CollectableId = collectable.Id,
                        InventoryType = collectable.InventoryItemType,
                    };

                    foreach (var (_, player) in world.Players)
                    {
                        player.SendPacket(collectResponse);
                    }
                }
            }

            background.BlockType = BlockType.None;
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.SET_SEED_KEY)]
    public static Task OnSetSeed(EventContext context, SetSeedRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        var inventoryItemType = request.BlockType == BlockType.Fertilizer ? InventoryItemType.Block : InventoryItemType.Seed;
        if (!context.Player.HasItem(request.BlockType, inventoryItemType))
        {
            return Task.CompletedTask;
        }

        var seed = world.GetSeed(request.X, request.Y);
        var setFertilizer = false;
        if (seed is not null)
        {
            if (request.BlockType == BlockType.Fertilizer)
            {
                if (context.Player.TutorialState == TutorialState.NotStarted)
                {
                    seed.GrowthEndTime = DateTime.MinValue;
                }
                else
                {
                    seed.GrowthEndTime -= TimeSpan.FromHours(1);
                }
                setFertilizer = true;
            }
            else if (!seed.IsAlreadyCrossBred)
            {
                var resultBlockType = Seeds.GetCrossBreedingResult(new(seed.BlockType, request.BlockType));
                if (resultBlockType != BlockType.None)
                {
                    seed = Seeds.GenerateSeedData(resultBlockType, seed.Position, true);
                    world.SetSeed(seed.Position.X, seed.Position.Y, seed);
                }
                else
                {
                    return Task.CompletedTask;
                }
            }
            else
            {
                return Task.CompletedTask;
            }
        }
        else
        {
            seed = Seeds.GenerateSeedData(request.BlockType, new Vector2i(request.X, request.Y));
            world.SetSeed(request.X, request.Y, seed);
            world.SetBlock(request.X, request.Y, BlockType.Tree);
        }

        context.Player.RemoveItem(request.BlockType, inventoryItemType, 1);

        var response = new SetSeedResponse()
        {
            X = seed.Position.X,
            Y = seed.Position.Y,
            ID = NetStrings.SET_SEED_KEY,
            IsMixed = seed.IsAlreadyCrossBred,
            PlayerId = context.Player.Id,
            BlockType = seed.BlockType,
            HarvestGems = seed.HarvestGems,
            HarvestSeeds = seed.HarvestSeeds,
            HarvestBlocks = seed.HarvestBlocks,
            GrowthEndTime = seed.GrowthEndTime.Ticks,
            SetFertilizer = setFertilizer,
            HarvestExtraBlocks = seed.HarvestExtraBlocks,
            GrowthDurationInSeconds = seed.GrowthDurationInSeconds,
        };

        foreach (var (_, player) in world.Players)
        {
            player.SendPacket(response);
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.BUY_ITEM_PACK_KEY)]
    public static Task OnBuyItemPack(EventContext context, BuyItemPackRequest request)
    {
        if (context.Player.TutorialState == TutorialState.NotStarted)
        {
            if (request.ItemPackId != "BasicClothes")
            {
                throw new Exception("Invalid item pack id");
            }

            context.Player.SendPacket(new BuyItemPackResponse()
            {
                ID = NetStrings.BUY_ITEM_PACK_KEY,
                Success = "PS",
                ItemPackId = request.ItemPackId,
                ItemPackRolls = new int[] { 18, 5, 6 },
            });
            context.Player.AddItem(BlockType.JacketBlack, InventoryItemType.WearableItem, 1);
            context.Player.AddItem(BlockType.PantsSweat, InventoryItemType.WearableItem, 1);
            context.Player.AddItem(BlockType.ShoesBrown, InventoryItemType.WearableItem, 1);

            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.LEAVE_WORLD_KEY)]
    public static Task OnLeaveWorld(EventContext context)
    {
        if (context.World is null)
        {
            throw new Exception("Not in world");
        }

        context.World.RemovePlayer(context.Player);
        context.Player.SendPacket(new PacketBase()
        {
            ID = NetStrings.LEAVE_WORLD_KEY
        });

        return Task.CompletedTask;
    }

    [Event(NetStrings.COLLECT_KEY)]
    public static Task OnCollect(EventContext context, CollectRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            return Task.CompletedTask;
        }

        // very inefficient, prob better to use dictionary
        var collectable = world.Collectables.FirstOrDefault(x => x.Id == request.CollectableId);
        if (collectable is null)
        {
            return Task.CompletedTask;
        }

        if (!context.Player.CanFitItem(collectable.BlockType, collectable.InventoryItemType, collectable.Amount))
        {
            return Task.CompletedTask;
        }

        world.Collectables.Remove(collectable);

        if (collectable.IsGem)
        {
            switch (collectable.GemType)
            {
                case GemType.Gem1:
                {
                    context.Player.Gems += collectable.Amount * ConfigData.Gem1Value;
                    break;
                }

                case GemType.Gem2:
                {
                    context.Player.Gems += collectable.Amount * ConfigData.Gem2Value;
                    break;
                }

                case GemType.Gem3:
                {
                    context.Player.Gems += collectable.Amount * ConfigData.Gem3Value;
                    break;
                }

                case GemType.Gem4:
                {
                    context.Player.Gems += collectable.Amount * ConfigData.Gem4Value;
                    break;
                }

                case GemType.Gem5:
                {
                    context.Player.Gems += collectable.Amount * ConfigData.Gem5Value;
                    break;
                }
            }
        }
        else
        {
            context.Player.AddItem(collectable.BlockType, collectable.InventoryItemType, collectable.Amount);
        }

        context.Player.SendPacket(new CollectResponse()
        {
            ID = NetStrings.COLLECT_KEY,
            IsGem = collectable.IsGem,
            Amount = collectable.Amount,
            GemType = collectable.GemType,
            BlockType = collectable.BlockType,
            PositionX = collectable.Pos.X,
            PositionY = collectable.Pos.Y,
            CollectableId = request.CollectableId,
            InventoryType = collectable.InventoryItemType,
        });

        var response = new RemoveCollectResponse()
        {
            ID = NetStrings.REMOVE_COLLECT_KEY,
            CollectableId = request.CollectableId,
        };

        foreach (var (_, player) in world.Players)
        {
            player.SendPacket(response);
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.WEARABLE_OR_WEAPON_CHANGE_KEY)]
    public static Task OnWearableOrWeaponChange(EventContext context, WearableOrWeaponChangeRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        if (!context.Player.HasItem(request.HotspotBlockType, InventoryItemType.WearableItem))
        {
            throw new Exception("No item in inventory");
        }

        if (!ConfigData.BlockHotSpots.TryGetValue(request.HotspotBlockType, out var list))
        {
            // Maybe send a message related to this?
            return Task.CompletedTask;
        }

        foreach (var spot in list)
        {
            context.Player.Spots[(int)spot] = request.HotspotBlockType;
        }

        if (world.Players.Count > 1)
        {
            var response = new WearableOrWeaponChangeResponse()
            {
                ID = NetStrings.WEARABLE_OR_WEAPON_CHANGE_KEY,
                PlayerId = context.Player.Id,
                HotspotBlockType = request.HotspotBlockType,
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(response);
                }
            }
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.WEARABLE_OR_WEAPON_UNDRESS_KEY)]
    public static Task OnWearableOrWeaponUndress(EventContext context, WearableOrWeaponChangeRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        if (!context.Player.HasItem(request.HotspotBlockType, InventoryItemType.WearableItem))
        {
            throw new Exception("No item in inventory");
        }

        if (!ConfigData.BlockHotSpots.TryGetValue(request.HotspotBlockType, out var list))
        {
            // Maybe send a message related to this?
            return Task.CompletedTask;
        }

        foreach (var spot in list)
        {
            context.Player.Spots[(int)spot] = BlockType.None;
        }

        if (world.Players.Count > 1)
        {
            var response = new WearableOrWeaponChangeResponse()
            {
                ID = NetStrings.WEARABLE_OR_WEAPON_UNDRESS_KEY,
                PlayerId = context.Player.Id,
                HotspotBlockType = request.HotspotBlockType,
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(response);
                }
            }
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.PLAYER_STATUS_ICON_UPDATE)]
    public static Task OnPlayerStatusIconUpdate(EventContext context, PlayerStatusIconUpdateRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        context.Player.StatusIcon = request.StatusIcon;

        if (world.Players.Count > 1)
        {
            var response = new PlayerStatusIconUpdateResponse()
            {
                ID = NetStrings.PLAYER_STATUS_ICON_UPDATE,
                PlayerId = context.Player.Id,
                StatusIcon = request.StatusIcon
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(response);
                }
            }
        }

        return Task.CompletedTask;
    }

    [Event(NetStrings.WORLD_CHAT_MESSAGE_KEY)]
    public static Task OnWorldChatMessage(EventContext context, WorldChatMessageRequest request)
    {
        var world = context.World;
        if (world is null)
        {
            throw new Exception("Not in world");
        }

        if (world.Players.Count > 1)
        {
            var response = new WorldChatMessageResponse()
            {
                ID = NetStrings.WORLD_CHAT_MESSAGE_KEY,
                MessageBinary = new()
                {
                    Time = DateTime.UtcNow,
                    Nick = context.Player.Name,
                    UserId = context.Player.Id,
                    Channel = $"#{world.Name}",
                    MessageChat = request.Message,
                    ChannelIndex = 0,
                }
            };

            foreach (var (id, player) in world.Players)
            {
                if (id != context.Player.Id)
                {
                    player.SendPacket(response);
                }
            }
        }

        return Task.CompletedTask;
    }
}
