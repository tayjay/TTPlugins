using System;
using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Toys;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TTCore.Components;

public class AdminToyCollisionHandler : MonoBehaviour
{
    private bool initialized;

    

    /// <summary>Gets the projectile itself.</summary>
    public AdminToyBase Toy { get; private set; }

    /// <summary>
    /// Inits the <see cref="T:Exiled.API.Features.Components.CollisionHandler" /> object.
    /// </summary>
    /// <param name="owner">The projectile owner.</param>
    /// <param name="projectile">The projectile component.</param>
    public void Init(AdminToyBase toy)
    {
        this.Toy = toy;
        this.initialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Log.Info("Collision detected");
        try
        {
            if (!this.initialized)
            {
                Log.Info("Not initialized");
                return;
            }
            if ((UnityEngine.Object) this.Toy == (UnityEngine.Object) null)
                Log.Info("Toy is null!");
            if (collision == null)
                Log.Info("wat");
            if ((UnityEngine.Object) collision.gameObject == (UnityEngine.Object) null)
                Log.Info("pepehm");
            if (collision.gameObject.TryGetComponent<AdminToyBase>(out AdminToyBase _))
            {
                Log.Info("Collision with another toy");
                return;
            }
            //todo: Perform trigger code here. Create Event here
            Log.Info("Collision with "+collision.gameObject.name);
            if(collision.gameObject.TryGetComponent<ReferenceHub>( out ReferenceHub rh))
                Log.Info("Collision with player "+rh.nicknameSync._firstNickname);
        }
        catch (Exception ex)
        {
            Log.Info("Error: " + ex);
        }
        Log.Info("CollisionEnter end");
    }
    
    private void OnCollisionStay(Collision other)
    {
        Log.Info("Collision stay");
    }

    private void OnCollisionExit(Collision other)
    {
        Log.Info("Collision exit");
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
        Log.Info("Trigger enter");
        Log.Info("Collision with "+other.gameObject.name);
        if(other.gameObject.TryGetComponent<ReferenceHub>( out ReferenceHub rh))
            Log.Info("Collision with player "+rh.nicknameSync._firstNickname);
    }

    private void OnEnable()
    {
        //Log.Info("Enabled");
    }
}