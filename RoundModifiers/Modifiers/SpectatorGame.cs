using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class SpectatorGame : Modifier
{
    
    private Dictionary<Player, int> spectatorPoints = new Dictionary<Player, int>();

    public void OnRespawnWave(RespawningTeamEventArgs ev)
    {
            
        Player[] oldPlayers = ev.Players.ToArray();
            
        IOrderedEnumerable<Player> orderedPoints = oldPlayers.OrderBy( GetPoints);
        ev.Players.Clear();
        ev.Players.AddRange(orderedPoints);
        spectatorPoints.Clear();
    }

    public void OnPlayerDeath(DiedEventArgs ev)
    {
        foreach (Player player in ev.Player.CurrentSpectatingPlayers)
        {
            spectatorPoints[player]++;
        }
        spectatorPoints[ev.Player] = 0;
    }

    private int GetPoints(Player player)
    {
        if(spectatorPoints.TryGetValue(player, out var points))
            return points;
        spectatorPoints[player] = 0;
        return spectatorPoints[player];
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnWave;
        Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
        Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            
        spectatorPoints.Clear();
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "SpectatorGame",
        FormattedName = "Spectator Game",
        Description = "Players are ranked based on how many people they watch die",
        Aliases = new []{"spectator"},
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
}