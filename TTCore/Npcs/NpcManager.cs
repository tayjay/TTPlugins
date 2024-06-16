using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using TTCore.Npcs.AI;
using UnityEngine;

namespace TTCore.Npcs
{
    public class NpcManager
    {
        
        public List<Npc> Npcs;
        
        public NpcManager()
        {
            Npcs = new List<Npc>();
        }
        
        public bool SpawnNpc(string npcName, RoleTypeId npcRole, Vector3 npcPosition, out Npc npc,
            bool isSmart = true, bool roundIgnored = true, bool isGodMode = true)
        {
            Npc newNpc = Npc.Spawn(npcName, npcRole, position: npcPosition);
            newNpc.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;
            newNpc.RankName = "NPC";
            newNpc.RankColor = "white";
            if(roundIgnored) Round.IgnoredPlayers.Add(newNpc.ReferenceHub);
            
            Timing.CallDelayed(1f, () =>
            {
                if(isGodMode) newNpc.IsGodModeEnabled = true;
                if (isSmart) newNpc.GameObject.AddComponent<Brain>().Init(newNpc);
            });
            npc = newNpc;
            Npcs.Add(npc);
            return true;
        }


        public bool SpawnNpcSpectator(string npcName, out Npc npc)
        {
            npc = Npc.Spawn(npcName, RoleTypeId.Spectator);
            npc.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;
            /*if (npc.Role is FpcRole fpcRole)
            {
                fpcRole.FirstPersonController.FpcModule.Motor.UpdatePosition();
            }*/

            Npcs.Add(npc);
            return true;
        }
        
        
        
    }
}