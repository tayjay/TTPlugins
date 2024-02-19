using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace TTCore.Handlers
{
    public class NpcManager
    {
        
        public List<Npc> Npcs;
        
        public bool SpawnNpc(string npcName, RoleTypeId npcRole, Vector3 npcPosition, out Npc npc,
            bool roundIgnored = true)
        {
            Npc newNpc = Npc.Spawn(npcName, npcRole, position: npcPosition);
            newNpc.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;
            newNpc.RankName = "NPC";
            newNpc.RankColor = "white";
            if(roundIgnored) Round.IgnoredPlayers.Add(newNpc.ReferenceHub);
            
            Timing.CallDelayed(1f, () =>
            {
                newNpc.IsGodModeEnabled = true;
                
            });
            npc = newNpc;
            Npcs.Add(npc);
            return true;
        }


        public bool SpawnNpcSpectator(string npcName, out Npc npc)
        {
            npc = Npc.Spawn(npcName, RoleTypeId.Spectator);
            npc.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;
            return true;
        }
        
        
    }
}