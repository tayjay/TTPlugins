using Exiled.API.Features;
using TayTaySCPSL.Modifiers.LevelUp.Interfaces;
using UnityEngine;

namespace TayTaySCPSL.Modifiers.LevelUp.Boost
{
    public class HealAOEBoost : Boost, IGameTickEvent
    {
        public HealAOEBoost(Tier tier) : base(tier)
        {
        }

        public override bool AssignBoost(Player player)
        {
            if (HasBoost.ContainsKey(player.NetId))
                return false;
            HasBoost[player.NetId] = true;
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            return true;
        }

        public override string GetName()
        {
            return "Heal AOE";
        }

        public override string GetDescription()
        {
            return "Heal all players around you";
        }

        public void OnGameTick()
        {
            foreach (uint netId in HasBoost.Keys)
            {
                Player player = Player.Get(netId);
                foreach(Player target in Player.List)
                {
                    if(player.Role.Side != target.Role.Side)
                        continue;
                    if (Vector3.Distance(player.Position, target.Position) < 5)
                    {
                        target.Heal(1f);
                    }
                }
            }
        }
    }
}