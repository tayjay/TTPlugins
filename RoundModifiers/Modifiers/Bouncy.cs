using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using InventorySystem.Items.Pickups;
using RoundModifiers.API;
using TTCore.Components;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class Bouncy : Modifier
{

    public void OnPickupCreate(PickupAddedEventArgs ev)
    {
        ev.Pickup.GameObject.AddComponent<PickupCollisionHandler>().Init(ev.Pickup);
        /*var pickup = (ev.Pickup.Base as CollisionDetectionPickup);
        if (pickup != null)
        {
            
            Log.Debug("Spawning a "+ pickup?.name + " pickup.");
        }*/
    }
    
    public void OnPickupCollision(PickupCollisionEventArgs ev)
    {
        Pickup pickup = ev.Pickup;
        Collision collision = ev.Collision;
        pickup.Rigidbody.velocity = Vector3.Reflect(pickup.Rigidbody.velocity, collision.contacts[0].normal);
        Log.Debug("Bounce");
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Map.PickupAdded += OnPickupCreate;
        Custom.PickupCollision += OnPickupCollision;
    }

    protected override void UnregisterModifier()
    { 
        Exiled.Events.Handlers.Map.PickupAdded -= OnPickupCreate;
        Custom.PickupCollision -= OnPickupCollision;
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "Bouncy",
        FormattedName = "Bouncy",
        Description = "Makes all items bouncy",
        Impact = ImpactLevel.MinorGameplay,
        Hidden = false,
        MustPreload = false,
        Aliases = new []{"bounce"}
        };
}