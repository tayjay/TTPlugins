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
            RoleTypeId tier1 = ev.NextKnownTeam == SpawnableTeamType.NineTailedFox
                ? RoleTypeId.NtfCaptain
                : RoleTypeId.ChaosMarauder;
            RoleTypeId tier2 = ev.NextKnownTeam == SpawnableTeamType.NineTailedFox
                ? RoleTypeId.NtfSergeant
                : RoleTypeId.ChaosRifleman;
            RoleTypeId tier3 = ev.NextKnownTeam == SpawnableTeamType.NineTailedFox
                ? RoleTypeId.NtfPrivate
                : RoleTypeId.ChaosRepressor;

            Player[] oldPlayers = ev.Players.ToArray();
            
            IOrderedEnumerable<Player> orderedPoints = spectatorPoints.Keys.OrderBy( x => spectatorPoints[x]);
            ev.Players.Clear();
            ev.Players.AddRange(orderedPoints);
            foreach (Player player in oldPlayers)
            {
                ev.Players.AddIfNotContains(player);
            }
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