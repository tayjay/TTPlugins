using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using Respawning;
using TTCore.API;
using Utils.NonAllocLINQ;

namespace TTAddons.Handlers
{
    public class SpectatorHandler : IRegistered
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
        
        public void OnRoundRestart()
        {
            spectatorPoints.Clear();
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnWave;
            Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            
            spectatorPoints.Clear();
        }
    }
}