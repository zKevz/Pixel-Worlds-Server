namespace PixelWorldsServer.Protocol.Constants;

public enum WorldJoinResult : byte
{
    Ok,
    TooManyPlayersInServer,
    TooManyPlayersInWorld,
    MaintenanceStarting,
    NotValidWorldName,
    UserIsBanned,
    AdminLockedWorld,
    UserHasWarning,
    WorldUnavailable,
    ServerTimeOut,
    IncorrectServerAddress,
    ServerException,
    CustomJoinFail,
    AlreadyHere,
    NotAllowed
}
