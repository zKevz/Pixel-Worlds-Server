using PixelWorldsServer.DataAccess;
using PixelWorldsServer.DataAccess.Models;
using System.Collections.Concurrent;
using System.Net;

namespace PixelWorldsServer.Server.Players;

public class PlayerManager
{
    private readonly Database m_Database;
    private readonly ConcurrentDictionary<IPEndPoint, Player> m_Players = new();

    public PlayerManager(Database database)
    {
        m_Database = database;
    }

    public Player AddOrReplacePlayer(IPEndPoint iPEndPoint)
    {
        var player = new Player(iPEndPoint.Address);
        return m_Players.AddOrUpdate(iPEndPoint, player, (x, y) =>
        {
            y.Disconnect();
            return player;
        });
    }

    public bool RemovePlayer(IPEndPoint iPEndPoint)
    {
        return m_Players.TryRemove(iPEndPoint, out _);
    }

    public async Task SaveAllPlayersAsync()
    {
        var tasks = new List<Task>();

        foreach (var (_, player) in m_Players)
        {
            tasks.Add(m_Database.SavePlayerAsync(PlayerModel.CreateCopy(player)));
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
