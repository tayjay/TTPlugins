using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using MEC;
using PlayerRoles.FirstPersonControl;
using RoundModifiers.API;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers
{
    /*
     * This class is used to handle the PreBreach modifier
     * It's a normal day at Site-02.
     * Class D are locked in their cells
     * Scientists roam the halls
     * Guards are on duty
     * SCPs are safely contained
     *
     * SCP gameloop:
     * SCPs are unable to move, or spawn as spectators to the round.
     * They will be unable to play until the breach timer counts down, or someone opens the room they are in.
     * If an SCP breaches containment early and is able to get to another zone the round will start.
     *
     * Guards:
     * Cannot run, why would they need to on a normal day?
     * Cannot enter light containment until timer has ended.
     * Can take the time to collect keycards if they want, but if they open an SCP room it is set free
     *
     * Scientists:
     * Cannot go through the checkpoint to heavy containment until timer has ended.
     * Cannot run
     * Can collect keycards and use 914, but can only hold 1 card at a time (So they don't drain them all from light)
     *
     * ClassD:
     * Cannot escape until timer has ended.
     * Door to D-block is locked but they can leave their cells
     * Supplies can be found in the cells.
     * The longer it takes for the round to start the ClassD will have more supplies dropped for them in their cells
     * 
     */
    public class PreBreach : Modifier
    {

        public void OnRoundStart()
        {
            
        }
        
        public void OnChangeRole(ChangingRoleEventArgs ev)
        {
            //handling spawning as SCP
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            //Limit card pickups
        }
        
        public void OnChangingMoveState(ChangingMoveStateEventArgs ev)
        {
            //Try to stop running
            if (ev.NewState == PlayerMovementState.Sprinting)
            {
                Timing.CallDelayed(Timing.WaitForOneFrame, () =>
                {
                    (ev.Player.Role as FpcRole).FirstPersonController.FpcModule.SyncMovementState =
                        PlayerMovementState.Walking;
                    ev.Player.ShowHUDHint("You cannot run yet!", 3f);
                });
            }
        }
        
        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if(ev.Door.Rooms.First(r=>r.Type==RoomType.LczClassDSpawn)!=null && ev.Door.Rooms.Count!=1)
                ev.IsAllowed = false;
        }

        public void OnComputerDoorOpen(TriggeringDoorEventArgs ev)
        {
            
        }
        
        protected override void RegisterModifier()
        {
            throw new System.NotImplementedException();
        }

        protected override void UnregisterModifier()
        {
            throw new System.NotImplementedException();
        }

        public override ModInfo ModInfo { get; } = new ModInfo() 
        {
            Name = "PreBreach",
            FormattedName = "<color=yellow>Pre-Breach</color>",
            Aliases = new []{"PB"},
            Description = "A normal day at Site-02.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
    }
}