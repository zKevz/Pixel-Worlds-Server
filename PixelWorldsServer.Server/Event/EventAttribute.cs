namespace PixelWorldsServer.Server.Event;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class EventAttribute : Attribute
{
    public string Id { get; set; }

    public EventAttribute(string id)
    {
        Id = id;
    }
}
