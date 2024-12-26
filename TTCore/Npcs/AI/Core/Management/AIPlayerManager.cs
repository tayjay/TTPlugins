using System.Collections.Generic;
using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using TTCore.Npcs.AI.Core.World;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.Management;

public static class AIPlayerManager
{
    /// <summary>
        /// List of registered AI Players.
        /// </summary>
        public static readonly List<AIPlayerProfile> Registered = [];

        /// <summary>
        /// Creates a fake client and adds it to the registered list.
        /// </summary>
        public static AIPlayerProfile CreateAIPlayer(this AIDataProfileBase profile, RoleTypeId role=RoleTypeId.ClassD)
        {
            int id = 1000 + Registered.Count;
            /*GameObject playerBody = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var fakeClient = new FakeClient(id);
            NetworkServer.AddPlayerForConnection(fakeClient, playerBody);*/
            //todo: new Dummy spawn logic
            /*Npc npc = Npc.Spawn("AI-" + id, role, id);
            ReferenceHub hub = npc.GameObject.GetComponent<ReferenceHub>();
            AIPlayer aiCont = npc.GameObject.AddComponent<AIPlayer>();
            AIPlayerProfile prof = new(npc.Connection, id, hub, aiCont, profile);
            Registered.Add(prof);
            profile.Name = "AI-" + id;
            return prof;
            */
            return null;
        }

        public static AIPlayerProfile GetAIPlayer(this int aiId)
        {
            if (aiId >= Registered.Count)
                return null;

            return Registered[aiId];
        }

        public static Player AIIDToPlayer(this int aiId)
        {
            if (Player.TryGet(GetAIPlayer(aiId).ReferenceHub, out Player player))
                return player;
            return null;
        }

        public static AIPlayerProfile GetAI(this Player p) => p.ReferenceHub.GetAI();

        public static AIPlayerProfile GetAI(this ReferenceHub p)
        {
            foreach (AIPlayerProfile prof in Registered)
                if (prof.ReferenceHub == p)
                    return prof;
            return null;
        }

        public static bool TryGetAI(this Player p, out AIPlayerProfile ai) => p.ReferenceHub.TryGetAI(out ai);

        public static bool TryGetAI(this ReferenceHub p, out AIPlayerProfile ai)
        {
            ai = p.GetAI();
            return ai != null;
        }

        public static bool IsAI(this Player p) => p != null && p.ReferenceHub.IsAI();

        public static bool IsAI(this ReferenceHub p) => p != null && p.TryGetAI(out _);

        public static void Delete(this AIPlayerProfile prof)
        {
            Registered.Remove(prof);
            NetworkServer.RemovePlayerForConnection(prof.Connection, true);
        }
}