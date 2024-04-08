using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MoonSharp.Interpreter;
using UnityEngine;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyDoor
    {
        public Door door { get; }
        
        [MoonSharpHidden]
        public ProxyDoor(Door door)
        {
            this.door = door;
        }
        
        public string Name => door.Name;
        public Vector3 Position => door.Position;
        public Quaternion Rotation => door.Rotation;
        public Vector3 Scale => door.Scale;
        public DoorType Type => door.Type;
        public Room Room => door.Room;
        public ZoneType Zone => door.Zone;
        public bool IsFullyOpen => door.IsFullyOpen;
        public bool IsFullyClosed => door.IsFullyClosed;
        public bool IsMoving => door.IsMoving;
        public bool IsGate => door.IsGate;
        public bool IsCheckpoint => door.IsCheckpoint;
        public bool IsElevator => door.IsElevator;
        public KeycardPermissions KeycardPermissions => door.KeycardPermissions;
        

        public bool IsOpen
        {
            get => door.IsOpen;
            set => door.IsOpen = value;
        }

        public bool AllowsScp106
        {
            get => door.AllowsScp106;
            set => door.AllowsScp106 = value;
        }

        public bool IsLocked => door.IsLocked;
        
        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Lock(float duration, DoorLockType lockType)
        {
            door.Lock(duration, lockType);
        }

        public void Lock(float duration)
        {
            door.Lock(duration, DoorLockType.AdminCommand);
        }
        
        public void Unlock()
        {
            door.Unlock();
        }


    }
}