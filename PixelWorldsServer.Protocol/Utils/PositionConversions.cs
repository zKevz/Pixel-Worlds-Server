namespace PixelWorldsServer.Protocol.Utils;

public static class PositionConversions
{
    public static Vector2 ConvertPlayerMapPointToWorldPoint(int x, int y)
    {
        return new(x * ConfigData.TileSizeX, y * ConfigData.TileSizeY - ConfigData.TileSizeY * 0.5f);
    }
    public static Vector2i ConvertWorldPointToMapPoint(float x, float y)
    {
        return new Vector2i((int)((x + ConfigData.TileSizeX * 0.5f) / ConfigData.TileSizeX), (int)((y + ConfigData.TileSizeY * 0.5f) / ConfigData.TileSizeY));
    }

    public static void ConvertWorldPointToMapPoint(float x, float y, Vector2i mapPoint)
    {
        mapPoint.X = (int)((x + ConfigData.TileSizeX * 0.5f) / ConfigData.TileSizeX);
        mapPoint.Y = (int)((y + ConfigData.TileSizeY * 0.5f) / ConfigData.TileSizeY);
    }

    public static Vector2i ConvertPlayersWorldPointToMapPoint(float x, float y)
    {
        y += ConfigData.TileSizeY * 0.5f;
        return ConvertWorldPointToMapPoint(x, y);
    }

    public static Vector2i ConvertPlayersWorldPointToMapPointFromFeet(float x, float y)
    {
        y += ConfigData.TileSizeY * 0.01f;
        return ConvertWorldPointToMapPoint(x, y);
    }

    public static Vector2i ConvertCollectablePosToMapPoint(float x, float y)
    {
        return ConvertWorldPointToMapPoint(x * ConfigData.TileSizeX, y * ConfigData.TileSizeY);
    }
}
