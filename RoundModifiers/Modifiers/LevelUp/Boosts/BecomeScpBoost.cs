using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public class BecomeScpBoost : Boost
    {
        public BecomeScpBoost(Tier tier) : base(tier)
        {
        }

        public override bool AssignBoost(Player player)
        {
            if(player.IsScp) return false;
            if(Player.List.Count(p=>p.Role == RoleTypeId.Scp3114) > 0) return false; //Only one SCP-3114 at a time
            if(Round.ElapsedTime.TotalMinutes < 15) return false; //Don't give SCP-3114 at the start of the round
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            RoleTypeId newRole = RoleTypeId.Scp3114;
            
            player.RoleManager.ServerSetRole(newRole, RoleChangeReason.RemoteAdmin);

            if (Map.DecontaminationState != DecontaminationState.Countdown &&
                Map.DecontaminationState != DecontaminationState.Lockdown &&
                Map.DecontaminationState != DecontaminationState.Finish)
            {
                //Light containment is safe
            }
            else
            {
                //Don't put in light containment
                player.Teleport(Room.Get(RoomType.Hcz939).Position+Vector3.up);
            }
            
            Timing.CallDelayed(0.1f, ()=>
            {
                player.MaxHealth = 200;
                player.Health = 200;
            });
            
            return true;
        }

        public override string GetName()
        {
            return "Become SCP Boost";
        }

        public override string GetDescription()
        {
            return "Become SCP-3114 on level up.";
        }
    }
}