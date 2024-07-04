using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIFollow : AIModuleBase
    {
        public float FollowDistance = 3f;
        public float FollowRandomRange = 1.5f;
        public float SprintDistance = 7f;

        AIPathfinder Pathfinder;

        public Player Target
        {
            get => Parent.FollowTarget;
            set => Parent.FollowTarget = value;
        }

        public override void Init()
        {
            Pathfinder = Parent.GetModule<AIPathfinder>();
            Tags = [AIBehaviorBase.AutonomyTag];
            FollowDistance -= Random.Range(0f, FollowRandomRange);
        }

        public override void OnDisabled()
        {
            Pathfinder.LookAtWaypoint = true;
            Pathfinder.ClearDestination();
        }

        public override void Tick()
        {
            if (!Enabled || !HasTarget || Parent.GetDistance(Target) < FollowDistance)
                return;
            //Log.Debug("Following target! "+Target.Nickname);
            Pathfinder.LookAtWaypoint = Parent.HasLOS(Parent.FollowTarget, out _, out _);

            if (!Pathfinder.LookAtWaypoint)
                Parent.MovementEngine.LookPos = Target.ReferenceHub.PlayerCameraReference.position;

            Pathfinder.SetDestination(Target.Position);
            if (DistanceToTarget > SprintDistance)
                Parent.MovementEngine.State = PlayerMovementState.Sprinting;
            else
                Parent.MovementEngine.State = TargetFpc.CurrentMovementState;
            //Log.Debug("Still following target! "+Target.Nickname);
            if (TryGetDoor(out Door door, out bool inVision))
            {
                if (!inVision && !door.IsOpen)
                    Parent.MovementEngine.LookPos = door.Base.transform.position+Vector3.up;
                else if (Parent.TrySetDoor(door, true))
                    OnSetDoor(door);
            }
            
            
            
        }

        public override void OnEnabled() { }

        public bool HasTarget => Parent.HasFollowTarget;

        public float DistanceToTarget
        {
            get
            {
                if (HasTarget)
                    return Vector3.Distance(Target.Position, Parent.Position);
                return 0f;
            }
        }

        public FirstPersonMovementModule TargetFpc
        {
            get
            {
                if (!HasTarget)
                    return null;

                if (Target.ReferenceHub.roleManager.CurrentRole is IFpcRole fpc)
                    return fpc.FpcModule;

                return null;
            }
        }
        
        public float DoorDistance = 1.5f;
        public float DoorFacing = 1.5f;
        public float DoorDotMinimum = 0.1f;
        
        public Door GetDoor(out bool inVision)
        {
            Door door = null;
            /*float doorDist = Mathf.Infinity;
            foreach (DoorVariant d in DoorVariant.AllDoors)
            {
                if (d is BasicNonInteractableDoor)
                    continue;

                float dist = GetDoorDistance(d);
                if (dist <= DoorDistance && (door == null || dist < doorDist))
                {
                    door = d;
                    doorDist = dist;
                }
            }*/
            door = Door.GetClosest(Parent.Position, out float doorDist);
            if(doorDist > DoorDistance)
                door = null;

            inVision = door == null || Parent.GetDotProduct(door.Base.transform.position) >= DoorDotMinimum;
            return door;
        }
        
        protected virtual void OnSetDoor(Door door) { }
        
        public bool TryGetDoor(out Door door, out bool inVision)
        {
            door = GetDoor(out inVision);
            return door != null;
        }

        public float GetDoorDistance(Door door) => Vector3.Distance(door.Position, Parent.Position);
    }