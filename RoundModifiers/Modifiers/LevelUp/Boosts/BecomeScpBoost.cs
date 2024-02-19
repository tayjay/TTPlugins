using Exiled.API.Enums;
using Exiled.API.Features;
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