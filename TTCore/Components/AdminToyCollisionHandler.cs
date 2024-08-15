using System;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Toys;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TTCore.Components;

public class AdminToyCollisionHandler : MonoBehaviour
{
    private bool initialized;

    private void Awake()
    {
        
    }

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

        if (AdminToy.Get(toy).ToyType==AdminToyType.PrimitiveObject)
        {
            Primitive primitive = AdminToy.Get(toy) as Primitive;
            switch (primitive.Type)
            {
                case PrimitiveType.Capsule:
                    CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
                    capsule.isTrigger = true;
                    capsule.height = primitive.Scale.y;
                    capsule.radius = primitive.Scale.x;
                    break;
                case PrimitiveType.Cube:
                    BoxCollider cube = gameObject.AddComponent<BoxCollider>();
                    cube.isTrigger = true;
                    cube.size = primitive.Scale;
                    break;
                case PrimitiveType.Sphere:
                    SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
                    sphere.isTrigger = true;
                    sphere.radius = primitive.Scale.x;
                    break;
                default:
                    BoxCollider def = gameObject.AddComponent<BoxCollider>();
                    def.isTrigger = true;
                    def.size = primitive.Scale;
                    break;
            }
        }
        
       //BoxCollider collider = gameObject.AddComponent<BoxCollider>();
       // collider.isTrigger = true;
        //collider.size = Toy.Scale;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        //Log.Info("Trigger enter");
        //Log.Info("Collision with "+other.gameObject.name);
        if (other.gameObject.TryGetComponent<ReferenceHub>(out ReferenceHub rh))
        {
            Log.Info("Collision with player "+rh.nicknameSync._firstNickname);
            
        }
        Custom.OnAdminToyCollision(new AdminToyCollisionEventArgs(Toy, other.gameObject));
    }
    
}