using System.Collections.Generic;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using TTCore.Npcs.AI.Pathing;
using UnityEngine;
using BasicNonInteractableDoor = Interactables.Interobjects.BasicNonInteractableDoor;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIFollowPath : AIModuleBase
    {
        public float DoorDistance = 1.5f;
        public float DoorFacing = 1.5f;
        public float DoorDotMinimum = 0.1f;

        public AIMovementEngine MovementEngine => Parent.MovementEngine;

        public Vector3 Position => Parent.ReferenceHub.transform.position;

        public Path Path;

        public int CurrentIndex;

        public bool LookAtWaypoint = true;
        public bool EnableMovement = true;

        public Vector3 OverrideWishDir { get; set; }

        public Vector3 WishDir
        {
            get => MovementEngine.WishDir;
            set
            {
                if (OverrideWishDir != Vector3.zero)
                    MovementEngine.WishDir = OverrideWishDir;
                else
                    MovementEngine.WishDir = value;
            }
        }

        public static bool DebugMode = false;

        //public readonly List<BreakableToyBase> Markers = [];

        public override void Init()
        {
            Tags = [AIBehaviorBase.MoverTag];
            InitPath();
        }

        public override void OnDisabled()
        {
            WishDir = Vector3.zero;
        }

        public override void OnEnabled() { }

        public override void Tick()
        {
            if (!Enabled)
                return;

            if (Path == null)
            {
                WishDir = Vector3.zero;
                return;
            }

            if (Path.TryGetWaypoint(CurrentIndex, out Vector3 waypoint))
            {
                if (EnableMovement)
                    WishDir = GetDirection(waypoint);

                if (!Path.TryGetDistance(Position, CurrentIndex, out float dist) || dist <= Path.WaypointRadius)
                {
                    CurrentIndex++;
                    if (CurrentIndex >= Path.Waypoints.Count)
                        OnEndPath();
                }

                if (LookAtWaypoint)
                {
                    Vector3 w = waypoint;
                    w.y = Parent.CameraPosition.y;
                    MovementEngine.LookPos = w;
                }

                if (TryGetDoor(out Door door, out bool inVision))
                {
                    if (!inVision && !door.IsOpen)
                        Parent.MovementEngine.LookPos = door.Base.transform.position;
                    else if (Parent.TrySetDoor(door, true))
                        OnSetDoor(door);
                }
            }
            else
                WishDir = Vector3.zero;

            DebugUpdatePath();
        }

        public void DebugUpdatePath()
        {
            /*if (!DebugMode)
                return;

            foreach (BreakableToyBase toy in Markers)
                toy.Destroy();

            if (Path == null)
                return;

            foreach (Vector3 pos in Path.Waypoints)
                Markers.Add(BreakableToyManager.SpawnBreakableToy<BreakableToyBase>(null, PrimitiveType.Sphere, pos, Quaternion.identity, new(-0.1f, -0.1f, -0.1f), Color.red));*/
        }

        protected virtual void OnEndPath()
        {
            Path = null;
        }

        protected virtual void OnSetDoor(Door door) { }

        public Vector3 GetDirection(Vector3 waypoint)
        {
            Vector3 pos1 = waypoint;
            pos1.y = Position.y;
            return (pos1 - Position).normalized;
        }

        public void InitPath()
        {
            if (Path != null)
                CurrentIndex = Path.GetNearestIndex(Position);
        }

        public Door GetDoor(out bool inVision)
        {
            Door door = null;
            /*float doorDist = Mathf.Infinity;
            foreach (Door d in Door.List)
            {
                if (d.Base is BasicNonInteractableDoor)
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

        public bool TryGetDoor(out Door door, out bool inVision)
        {
            door = GetDoor(out inVision);
            return door != null;
        }

        public float GetDoorDistance(Door door) => Vector3.Distance(door.Base.transform.position, Parent.Position);

        public Vector3 GetDoorDirection(Door door)
        {
            Vector3 pos = door.Base.transform.position;
            pos.y = Parent.Position.y;
            return (pos - Parent.Position).normalized;
        }
    }