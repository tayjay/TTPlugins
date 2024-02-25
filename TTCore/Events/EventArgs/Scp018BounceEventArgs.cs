using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;

namespace TTCore.Events.EventArgs;

public class Scp018BounceEventArgs : IExiledEvent
{
    public GameObject Owner { get; }
    public ThrownProjectile Projectile { get; }
    public Collider Collider { get; }
    
    public Vector3 Position => Projectile.Position;
    
    public Player PreviousOwner => Player.Get(Projectile.PreviousOwner);
    
    public Scp018BounceEventArgs(GameObject owner, ThrownProjectile projectile, Collider collider)
    {
        Owner = owner;
        Projectile = projectile;
        Collider = collider;
    }
}