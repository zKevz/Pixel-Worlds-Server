using MongoDB.Bson.Serialization.Attributes;

namespace PixelWorldsServer.Protocol.Utils;

public class Vector2
{
    [BsonElement("x")]
    public float X { get; set; }

    [BsonElement("y")]
    public float Y { get; set; }

    public Vector2()
    {
        X = 0;
        Y = 0;
    }

    public Vector2(float mx, float my)
    {
        X = mx;
        Y = my;
    }

    public Vector2(int mx, int my)
    {
        X = mx;
        Y = my;
    }
}

public class Vector2i
{
    [BsonElement("x")]
    public int X { get; set; }

    [BsonElement("y")]
    public int Y { get; set; }

    public Vector2i()
    {
        X = 0;
        Y = 0;
    }

    public Vector2i(float mx, float my)
    {
        X = (int)mx;
        Y = (int)my;
    }

    public Vector2i(int mx, int my)
    {
        X = mx;
        Y = my;
    }
}
