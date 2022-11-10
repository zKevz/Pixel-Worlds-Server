namespace PixelWorldsServer.DataAccess;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string PlayersCollectionName { get; set; } = null!;
    public string WorldsCollectionName { get; set; } = null!;
}
