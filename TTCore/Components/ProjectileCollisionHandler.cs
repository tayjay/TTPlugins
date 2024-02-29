using System;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TTCore.Components;

public class ProjectileCollisionHandler : MonoBehaviour
{
    private bool initialized;

    /// <summary>Gets the thrower of the projectile.</summary>
    public GameObject Owner { get; private set; }

    /// <summary>Gets the projectile itself.</summary>
    public ThrownProjectile Projectile { get; private set; }

    /// <summary>
    /// Inits the <see cref="T:Exiled.API.Features.Components.CollisionHandler" /> object.
    /// </summary>
    /// <param name="owner">The projectile owner.</param>
    /// <param name="projectile">The projectile component.</param>
    public void Init(GameObject owner, ThrownProjectile projectile)
    {
        this.Owner = owner;
        this.Projectile = projectile;
        this.initialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Log.Info("Collision detected");
        try
        {
            if (!this.initialized)
            {
                Log.Info("Not initialized");
                return;
            }
            if ((UnityEngine.Object) this.Owner == (UnityEngine.Object) null)
                Log.Info("Owner is null!");
            if ((UnityEngine.Object) this.Projectile == (UnityEngine.Object) null)
                Log.Info("Projectile is null!");
            if (collision == null)
                Log.Info("wat");
            if ((UnityEngine.Object) collision.gameObject == (UnityEngine.Object) null)
                Log.Info("pepehm");
            if ((UnityEngine.Object)collision.gameObject == (UnityEngine.Object)this.Owner ||
                collision.gameObject.TryGetComponent<ThrownProjectile>(out ThrownProjectile _))
            {
                //Log.Info("Collision with owner or another projectile");
                return;
            }
            //todo: Perform trigger code here. Create Event here
            /*Pickup pu = Pickup.CreateAndSpawn(ItemType.SCP018, Projectile.Position, Projectile.Rotation,
                Player.Get(Projectile.PreviousOwner));
            pu.PhysicsModule.Rb.velocity = Vector3.up * 5;
            Log.Info("SCP018 spawned");*/
            Custom.OnScp018Bounce(new Scp018BounceEventArgs(Owner, Projectile, collision.collider));
        }
        catch (Exception ex)
        {
            Log.Info("Error: " + ex);
        }
        //Log.Info("CollisionEnter end");
    }
    
    private void OnCollisionStay(Collision other)
    {
        //Log.Info("Collision stay");
    }

    private void OnCollisionExit(Collision other)
    {
        //Log.Info("Collision exit");
    }

    private void OnDestroy()
    {
        //Log.Info("Destroyed");
    }
    
    private void OnDisable()
    {
        //Log.Info("Disabled");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Log.Info("Trigger enter");
        /*Pickup pu = Pickup.CreateAndSpawn(ItemType.SCP018, Projectile.Position, Projectile.Rotation,
            Player.Get(Projectile.PreviousOwner));
        pu.PhysicsModule.Rb.velocity = new Vector3(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)) * 5;*/
        Custom.OnScp018Bounce(new Scp018BounceEventArgs(Owner, Projectile, other));
        //Log.Info("SCP018 spawned");
    }

    private void OnEnable()
    {
        //Log.Info("Enabled");
    }
}