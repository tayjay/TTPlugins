using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class SpectatorGame : Modifier
{

    private Dictionary<Player, float> spectatorPoints;

    public void OnRespawnWave(RespawningTeamEventArgs ev)
    {
        Player[] oldPlayers = ev.Players.ToArray();
            
        IOrderedEnumerable<Player> orderedPoints = oldPlayers.OrderBy( GetPoints);
        if (orderedPoints.Count() != ev.Players.Count)
        {
            Log.Error("SpectatorGame: Player count mismatch!");
            return;
        }

        foreach (Player player in orderedPoints)
        {
            if(player.Role == RoleTypeId.Spectator) continue;
            Log.Error("SpectatorGame: Player is not a spectator!");
            return;
        }
        
        ev.Players.Clear();
        ev.Players.AddRange(orderedPoints);
        /*foreach (Player player in ev.Players)
        {
            
        }*/
        spectatorPoints.Clear();
    }

    public void OnPlayerDeath(DiedEventArgs ev)
    {
        foreach (Player player in ev.Player.CurrentSpectatingPlayers)
        {
            if(player.Role.Team==Team.ClassD || player.Role.Team==Team.Scientists)
                spectatorPoints[player]+=1;
            else if(player.Role.Team==Team.ChaosInsurgency || player.Role.Team==Team.FoundationForces)
                spectatorPoints[player]+=2;
            else if(player.Role.Team==Team.SCPs && player.Role != RoleTypeId.Scp0492)
                spectatorPoints[player]+=5;
            else
                spectatorPoints[player]+=1;
        }
        spectatorPoints[ev.Player] = 0;
    }

    private float GetPoints(Player player)
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

        spectatorPoints = DictionaryPool<Player, float>.Pool.Get();
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
        Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            
        DictionaryPool<Player, float>.Pool.Return(spectatorPoints);
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