using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Interfaces;

namespace TTCore.Events.EventArgs;

public class PickupCollisionEventArgs : IExiledEvent
{
    public Pickup Pickup { get; }
    public UnityEngine.Collision Collision { get; }
    
    
    public PickupCollisionEventArgs(Pickup pickup, UnityEngine.Collision collision)
    {
        Pickup = pickup;
        Collision = collision;
    }
}