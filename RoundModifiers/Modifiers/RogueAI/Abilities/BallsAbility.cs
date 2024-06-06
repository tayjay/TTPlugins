using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Pools;
using TTCore.Utilities;
using UnityEngine;

namespace RoundModifiers.Modifiers.RogueAI.Abilities;

public class BallsAbility : Ability
{
    
    private string message;
    private bool isHeld, isNoisy, oneShot, isDone;
    
    
    public BallsAbility(string name, string description, Side helpingSide) : base(name, description, helpingSide, 7, 10)
    {
        this.message = "XMAS_BOUNCYBALLS";
        this.isHeld = false;
        this.isNoisy = false;
        this.oneShot = true;
        this.isDone = false;
    }

    public override bool Setup()
    {
        if(!oneShot) return true;
        if(isDone) return false;
        
        return true;
    }
    
    public override void Start()
    {
        base.Start();
        Cassie.Message(message, isHeld, isNoisy);

        List<Room> targetRooms = ListPool<Room>.Pool.Get();
        targetRooms.Add(Room.Random(ZoneType.LightContainment));
        targetRooms.Add(Room.Random(ZoneType.HeavyContainment));
        targetRooms.Add(Room.Random(ZoneType.Entrance));
        targetRooms.Add(Room.Random(ZoneType.Surface));
        
        foreach(Room room in targetRooms)
        {
            Projectile ball = Projectile.CreateAndSpawn(ProjectileType.Scp018, room.Position+Vector3.up, room.Rotation);
            ball.PhysicsModule.Rb.velocity = Vector3Utils.Random(5f,10f);
        }
        isDone = true;
    }

    public override bool Update()
    {
        return true;
    }
}