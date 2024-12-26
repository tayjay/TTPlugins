using System.Collections.Generic;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using UnityEngine;

namespace RoundModifiers.Modifiers.WeaponStats;

/*public class PortalStat : Stat, IShooting
{
    public override string Name => "Portal";
    public override int Rarity { get; } = 4;
    public override string Description => "A weapon that can create portals.";
    
    public Dictionary<ushort, Portals> PortalDict = new Dictionary<ushort, Portals>();


    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        ev.Damage = 0;
        if (ev.Player.IsAimingDownWeapon)
        {
            PortalDict[ev.Firearm.Serial].PortalB.Position = ev.Position;
        }
        else
        {
            PortalDict[ev.Firearm.Serial].PortalA.Position = ev.Position;
        }
    }

    public class Portals
    {
        public Primitive PortalA;
        public Primitive PortalB;

        public static PrimitiveSettings PortalASettings = new PrimitiveSettings(PrimitiveType.Sphere, Color.blue,
            Vector3.one, Vector3.zero, Vector3.one*-1, true);
        public static PrimitiveSettings PortalBSettings = new PrimitiveSettings(PrimitiveType.Sphere, Color.red,
            Vector3.one, Vector3.zero, Vector3.one*-1, true);
        
        public Portals()
        {
            PortalA = Primitive.Create(PortalASettings);
            PortalB = Primitive.Create(PortalBSettings);
        }
        
    }
}*/