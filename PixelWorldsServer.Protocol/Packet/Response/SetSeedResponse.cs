using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Constants;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class SetSeedResponse : PacketBase
{
    [BsonElement(NetStrings.POSITION_X_KEY)]
    public int X { get; set; }

    [BsonElement(NetStrings.POSITION_Y_KEY)]
    public int Y { get; set; }

    [BsonElement(NetStrings.PLAYER_ID_KEY)]
    public string PlayerId { get; set; } = string.Empty;

    [BsonElement(NetStrings.BLOCK_TYPE_KEY)]
    public BlockType BlockType { get; set; }

    [BsonElement(NetStrings.GROWTH_DURATION_KEY)]
    public int GrowthDurationInSeconds { get; set; }

    [BsonElement(NetStrings.GROWTH_END_TIME_KEY)]
    public long GrowthEndTime { get; set; }

    [BsonElement(NetStrings.IS_MIXED_KEY)]
    public bool IsMixed { get; set; }

    [BsonElement(NetStrings.HARVEST_SEEDS_KEY)]
    public int HarvestSeeds { get; set; }

    [BsonElement(NetStrings.HARVEST_BLOCKS_KEY)]
    public int HarvestBlocks { get; set; }

    [BsonElement(NetStrings.HARVEST_GEMS_KEY)]
    public int HarvestGems { get; set; }

    [BsonElement(NetStrings.HARVEST_EXTRA_BLOCKS_KEY)]
    public int HarvestExtraBlocks { get; set; }

    [BsonElement(NetStrings.SET_FERTILIZER_KEY)]
    public bool SetFertilizer { get; set; }
}
