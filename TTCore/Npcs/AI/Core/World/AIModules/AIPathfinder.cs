using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.Commands;
using Interactables.Interobjects;
using MapGeneration;
using TTCore.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIPathfinder : AIFollowPath
    {
        public static ElevatorChamber[] Elevators => Object.FindObjectsOfType<ElevatorChamber>();

        public Vector3 TargetLocation { get; private set; }

        public Vector3 NavPosition => NavMesh.SamplePosition(Position, out NavMeshHit _hit, 50f, NavMesh.AllAreas) ? _hit.position : default;

        public float RepathTimer = 0.2f;
        public float DestinationRadius = 3f;

        public bool AtDestination => Path == null || Vector3.Distance(TargetLocation, Position) <= DestinationRadius;

        private float timer;

        public override void Init()
        {
            Tags = [AIBehaviorBase.MoverTag];
        }

        public void SetDestination(Vector3 destination)
        {
            TargetLocation = destination;
        }

        public Vector3 AdjustDestination(Vector3 dest)
        {
            Room targetRoom = GetRoomAtPosition(dest);

            if (targetRoom == null)
                return dest;

            if (targetRoom.Zone != Parent.Room.Zone)
                return ZoneSpecificAdjustment(dest, targetRoom.Zone);

            return dest;
        }

        protected virtual Vector3 ZoneSpecificAdjustment(Vector3 dest, ZoneType zone)
        {
            if (Parent.Room.Zone == zone)
                return dest;

            /*            switch (zone)
                        {
                            case FacilityZone.HeavyContainment:
                                switch (Parent.Room.Zone)
                                {
                                    case FacilityZone.Entrance:
                                        return dest;
                                    case FacilityZone.LightContainment:
                                        return LCZElevator();
                                    case FacilityZone.Surface:
                                        return SurfaceElevator();
                                }
                                break;
                            case FacilityZone.Entrance:
                                switch (Parent.Room.Zone)
                                {
                                    case FacilityZone.HeavyContainment:
                                        return dest;
                                    case FacilityZone.LightContainment:
                                        return LCZElevator();
                                    case FacilityZone.Surface:
                                        return SurfaceElevator();
                                }
                                break;
                            case FacilityZone.LightContainment:
                                switch (Parent.Room.Zone)
                                {
                                    case FacilityZone.HeavyContainment: 
                                    case FacilityZone.Entrance:
                                        return HCZElevator();
                                    case FacilityZone.Surface:
                                        return SurfaceElevator();
                                }
                                break;
                            case FacilityZone.Surface:
                                switch (Parent.Room.Zone)
                                {
                                    case FacilityZone.HeavyContainment:
                                    case FacilityZone.Entrance:
                                        return EZElevator();
                                    case FacilityZone.LightContainment:
                                        return LCZElevator();
                                }
                                break;
                        }*/

            return dest;
        }

        protected Room GetClosestElevator(ZoneType zone)
        {
            Room closest = null;
            foreach (Room rid in NpcUtilities.ZoneElevatorRooms[zone])
                if (closest == null || Vector3.Distance(closest.transform.position, Parent.Position) > Vector3.Distance(rid.transform.position, Parent.Position))
                    closest = rid;
            return closest;
        }

        protected bool NotInElevatorRoom(ZoneType zone, out Vector3 dest)
        {
            if (!NpcUtilities.ZoneElevatorRooms[ZoneType.LightContainment].Contains(Parent.Room))
            {
                dest = GetClosestElevator(ZoneType.LightContainment).transform.position;
                return true;
            }
            dest = default;
            return false;
        }

        protected virtual Vector3 LCZElevator()
        {
            if (NotInElevatorRoom(ZoneType.LightContainment, out Vector3 dest))
                return dest;
            return default;
            /*ElevatorChamber[] chambers = Elevators;
            foreach (ElevatorChamber chamber in chambers)
            {
                if (!chamber.IsReady)
                    continue;

                chamber.
            }*/
        }

        protected virtual Vector3 HCZElevator()
        {
            if (NotInElevatorRoom(ZoneType.HeavyContainment, out Vector3 dest))
                return dest;
            return default;
        }

        protected virtual Vector3 EZElevator()
        {
            if (NotInElevatorRoom(ZoneType.Entrance, out Vector3 dest))
                return dest;
            return default;
        }

        protected virtual Vector3 SurfaceElevator()
        {
            return default;
        }

        public Room GetRoomAtPosition(Vector3 pos) => Room.Get(pos);

        public void Pathfind()
        {
            Path ??= new();

            if (AtDestination)
                return;

            NavMeshPath path = new();
            NavMesh.CalculatePath(Position, TargetLocation, NavMesh.AllAreas, path);
            Path?.OverridePath(path, TargetLocation);
            CurrentIndex = 0;
        }

        public void ClearDestination()
        {
            Path = null;
            TargetLocation = Position;
        }

        public override void Tick()
        {
            if (timer > 0f)
                timer -= Time.fixedDeltaTime;
            else
            {
                timer = RepathTimer;
                Pathfind();
            }

            base.Tick();
        }
    }