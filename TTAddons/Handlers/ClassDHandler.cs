using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using TTCore.API;
using TTCore.Extensions;
using UnityEngine;

namespace TTAddons.Handlers
{
    public class ClassDHandler : IRegistered
    {

        public void OnPlayerSpawning(SpawningEventArgs ev)
        {
            if(ev.Player.Role.Type== RoleTypeId.ClassD)
            {
                //Find nearest door
                Door nearestDoor = ClosestCellDoor(ev.Position);
                (ushort horizontal, ushort vertical) = Quaternion.LookRotation(nearestDoor.Position-ev.Player.Position).ToClientUShorts();
                ev.HorizontalRotation = Quaternion.LookRotation((nearestDoor.Position-ev.Player.Position).normalized).eulerAngles.y;
                Log.Info("Rotating ClassD to face door");
                Timing.CallDelayed(0.2f, () =>
                {
                    ev.Player.Rotation = Quaternion.LookRotation((nearestDoor.Position-ev.Player.Position).normalized);
                });
            }
        }

        protected Door ClosestCellDoor(Vector3 origin)
        {
            Door closestDoor = null;
            float closestDistance = float.MaxValue;
            foreach (Door door in Room.Get(RoomType.LczClassDSpawn).Doors.Where(d=>d.Rooms.Count==1))
            {
                if(Vector3.Distance(origin,door.Position)<closestDistance)
                {
                    closestDoor = door;
                    closestDistance = Vector3.Distance(origin,door.Position);
                }
            }
            return closestDoor;
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Player.Spawning += OnPlayerSpawning;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Player.Spawning -= OnPlayerSpawning;
        }
    }
}