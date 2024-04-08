using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MoonSharp.Interpreter;
using RelativePositioning;
using UnityEngine;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyPickup
    {
        public Pickup Pickup { get; }
        
        [MoonSharpHidden]
        public ProxyPickup(Pickup pickup)
        {
            Pickup = pickup;
        }
        
        public ItemType ItemType => Pickup.Type;
        public int Serial => Pickup.Serial;
        public Quaternion Rotation => Pickup.Rotation;
        public Player PreviousOwner => Pickup.PreviousOwner;

        public Vector3 Position
        {
            get => Pickup.Position;
            set => Pickup.Position = value;
        }
        
        public RelativePosition RelativePosition
        {
            get => Pickup.RelativePosition;
            set => Pickup.RelativePosition = value;
        }
        
        public Vector3 Scale
        {
            get => Pickup.Scale;
            set => Pickup.Scale = value;
        }
        
        public float Weight
        {
            get => Pickup.Weight;
            set => Pickup.Weight = value;
        }
        
        public float PickupTime
        {
            get => Pickup.PickupTime;
            set => Pickup.PickupTime = value;
        }

    }
}