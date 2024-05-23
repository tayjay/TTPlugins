using System;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using UnityEngine;

namespace TTCore.Components;

public class PickupCollisionHandler  : MonoBehaviour
{
    private bool initialized;
    public Pickup Pickup { get; private set; }
    
    public void Init(Pickup pickup)
    {
        Pickup = pickup;
        this.initialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            if (!this.initialized)
            {
                Log.Info("Not initialized");
                return;
            }
            if (this.Pickup == null)
                Log.Info("Pickup is null!");
            if (collision == null)
                Log.Info("wat");
            if ((UnityEngine.Object) collision.gameObject == (UnityEngine.Object) null)
                Log.Info("pepehm");
            if(collision.gameObject == Pickup.GameObject)
            {
                Log.Info("Collision with owner or another projectile");
                return;
            }
            Custom.OnPickupCollision(new PickupCollisionEventArgs(Pickup, collision));
            Log.Debug("Collision detected with pickup");
        }
        catch (Exception ex)
        {
            Log.Info("Error: " + ex);
        }
    }
}