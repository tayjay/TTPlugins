using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using MEC;
using PlayerRoles;
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
        public bool HasBreachStarted = false;
        protected CoroutineHandle _breachCountdown;
        protected Dictionary<Player,RoleTypeId> _scpRoles = new Dictionary<Player, RoleTypeId>();
        public void OnRoundStart()
        {
            _breachCountdown = Timing.RunCoroutine(BreachCountdown());
        }
        
        public IEnumerator<float> BreachCountdown()
        {
            yield return Timing.WaitForSeconds(300f);
            StartBreach();
            
        }

        public void StartBreach()
        {
            HasBreachStarted = true;
            Timing.KillCoroutines(_breachCountdown);
        }
        
        public void OnChangeRole(ChangingRoleEventArgs ev)
        {
            //handling spawning as SCP
            if (HasBreachStarted)
                return;
            if (ev.NewRole.GetTeam() == Team.SCPs)
            {
                if(ev.Reason==SpawnReason.Escaped)
                    return;
                _scpRoles[ev.Player] = ev.NewRole;

                /*TTCore.TTCore.Instance.NpcManager.SpawnNpc(ev.Player.Nickname, ev.NewRole, ev.Player.Position,
                    out Npc clone);*/
                    
                ev.NewRole = RoleTypeId.Spectator;
            }
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            if(HasBreachStarted)
                return;
            //Limit card pickups
            ItemType[] allCards = new ItemType[]
            {
                ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardGuard, ItemType.KeycardZoneManager,
                ItemType.KeycardO5, ItemType.KeycardChaosInsurgency, ItemType.KeycardContainmentEngineer, ItemType.KeycardFacilityManager,
                ItemType.KeycardResearchCoordinator, ItemType.KeycardMTFCaptain, ItemType.KeycardMTFOperative, ItemType.KeycardMTFPrivate
            };
            foreach (ItemType card in allCards)
            {
                if (ev.Player.HasItem(card))
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHUDHint("You can only hold 1 keycard at a time!", 3f);
                    return;
                }
            }
        }
        
        public void OnChangingMoveState(ChangingMoveStateEventArgs ev)
        {
            if(HasBreachStarted)
                return;
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
            if(HasBreachStarted)
                return;
            if(ev.Door.Rooms.First(r=>r.Type==RoomType.LczClassDSpawn)!=null && ev.Door.Rooms.Count!=1)
                ev.IsAllowed = false;

            foreach (Room room in ev.Door.Rooms)
            {
                if (RoomHasScp(room))
                {
                    //Release this Scp
                    foreach (Player p in room.Players)
                    {
                        if (p.Role.Team == Team.SCPs)
                        {
                            _scpRoles.Where((k)=>k.Value==p.Role.Type).Select(k=>k.Key).First().ShowHint("The round has started!", 5f);
                            p.Kick("");
                            
                        }
                    }
                }
            }
        }

        public bool RoomHasScp(Room room)
        {
            foreach (Player p in room.Players)
            {
                if(p.Role.Team==Team.SCPs)
                    return true;
            }

            return false;
        }

        public void OnComputerDoorOpen(TriggeringDoorEventArgs ev)
        {
            
        }
        
        protected override void RegisterModifier()
        {
            
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangeRole;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingMoveState += OnChangingMoveState;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Scp079.TriggeringDoor += OnComputerDoorOpen;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangeRole;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingMoveState -= OnChangingMoveState;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Scp079.TriggeringDoor -= OnComputerDoorOpen;
            
            _scpRoles.Clear();
            HasBreachStarted = false;
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