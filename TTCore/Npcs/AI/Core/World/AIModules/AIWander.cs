using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;
using UnityEngine.AI;
using BasicNonInteractableDoor = Interactables.Interobjects.BasicNonInteractableDoor;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIWander : AIModuleBase
    {
        public readonly List<Room> Blacklist = [];

        public float WanderTimerMin = 10f;
        public float WanderTimerMax = 30f;
        public float SurfaceRadius = 30f;
        public float RoomRadius = 60f;

        public bool ActiveWhenFollow;

        AIPathfinder Pathfinder;

        float timer;

        public Room GetRandomRoomInLayer()
        {
            List<Room> rooms = [];

            foreach (Room room in Room.List)
                if (room.GameObject.activeSelf && !Blacklist.Contains(room) && Vector3.Distance(Parent.Position, room.Position) <= RoomRadius && RoomIsInLayer(room))
                    rooms.Add(room);

            return rooms.Count > 0 ? rooms.RandomItem() : null;
        }

        public bool RoomIsInLayer(Room room)
        {
            switch (Parent.Core.Profile.Player.Zone)
            {
                case ZoneType.HeavyContainment:
                    if (room.Zone == ZoneType.Entrance)
                        return true;
                    break;
                case ZoneType.Entrance:
                    if (room.Zone == ZoneType.HeavyContainment)
                        return true;
                    break;
            }

            return room.Zone == Parent.Core.Profile.Player.Zone;
        }

        public bool TryGetRandomRoomInLayer(out Room room)
        {
            room = GetRandomRoomInLayer();
            return room != null;
        }

        public override void Init()
        {
            Pathfinder = Parent.GetModule<AIPathfinder>();
            Tags = [AIBehaviorBase.AutonomyTag];
        }

        public override void Tick()
        {
            if (!Enabled || (!ActiveWhenFollow && Parent.HasFollowTarget))
            {
                timer = 0f;
                return;
            }

            if (timer > 0f)
                timer -= Time.fixedDeltaTime;

            if (Pathfinder.AtDestination || timer <= 0f)
                SetDestination();
            if (TryGetDoor(out Door door, out bool inVision))
            {
                if (!inVision && !door.IsOpen)
                    Parent.MovementEngine.LookPos = door.Base.transform.position+Vector3.up;
                else if (Parent.TrySetDoor(door, true))
                    OnSetDoor(door);
            }
        }

        public void SetDestination()
        {
            if (TryGetRandomRoomInLayer(out Room room) && NavMesh.SamplePosition(room.Position, out NavMeshHit _hit, 50f, NavMesh.AllAreas))
            {
                timer = Random.Range(WanderTimerMin, WanderTimerMax);

                if (room.Identifier.Name == MapGeneration.RoomName.Outside)
                    NavMesh.SamplePosition(Parent.Position + Random.insideUnitSphere * SurfaceRadius, out _hit, 100f, NavMesh.AllAreas);

                Pathfinder.SetDestination(_hit.position);
            }
        }

        public override void OnEnabled()
        {
            SetDestination();
        }

        public override void OnDisabled()
        {
            Pathfinder.ClearDestination();
        }
        
        public float DoorDistance = 1.5f;
        public float DoorFacing = 1.5f;
        public float DoorDotMinimum = 0.1f;
        
        
        protected virtual void OnSetDoor(Door door) { }
        
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