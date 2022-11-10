using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PixelWorldsServer.Protocol.Utils;

namespace PixelWorldsServer.Protocol.Packet.Response;

public class SyncTimeResponse : PacketBase
{
    [BsonElement(NetStrings.SYNC_TIME_FIELD_KEY)]
    public long SyncTime { get; set; }

    [BsonElement(NetStrings.SYNC_TIME_SERVER_SLEEP_KEY)]
    public int ServerSleep { get; set; }
}
