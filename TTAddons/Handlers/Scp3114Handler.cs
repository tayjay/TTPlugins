using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using TTCore.API;
using UnityEngine;

namespace TTAddons.Handlers
{
    public class Scp3114Handler : IRegistered
    {

        public Scp3114Handler()
        {
            
        }
        
        public void OnRoundStart()
        {
            if(Server.PlayerCount<TTAddons.Instance.Config.Scp3114MinPlayers) return;
            double spawnChance = TTAddons.Instance.Config.Scp3114Chance;
            if (Random.Range(0f,1f) < spawnChance)
            {
                SpawnScp3114(Player.List.Where(p =>
                    p.Role.Team == (TTAddons.Instance.Config.Scp3114ReplaceScp ? Team.SCPs : Team.ClassD)).GetRandomValue());
            }
        }
        
        public void SpawnScp3114(Player player)
        {
            if(player==null) return;
            player.RoleManager.ServerSetRole(RoleTypeId.Scp3114, RoleChangeReason.RoundStart);
        }
        
        public void DisguiseAs(Player player, RoleTypeId disguise)
        {
            if (player.Role == RoleTypeId.Scp3114)
            {
                Exiled.API.Features.Roles.Scp3114Role role = (Exiled.API.Features.Roles.Scp3114Role) player.Role;
                Ragdoll.TryCreate(disguise,player.Nickname,"Spawned", out Ragdoll ragdoll);
                role.Ragdoll = ragdoll.Base;
            }
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }
        
        public void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }
    }
}