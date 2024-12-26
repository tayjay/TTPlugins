using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

/*public class PaintBallStat : Stat, IShooting, IInitializing
{
    
    
    
    
    public override string Name { get; } = "Paint Ball";
    public override int Rarity { get; } = 4;
    public override string Description { get; } = "Shoots balls at the cost of a full ammo clip.";
    public void OnShooting(ShootingEventArgs ev)
    {
        ev.IsAllowed = false;
        if(ev.Player.CurrentItem is Firearm firearm)
        {
            if(firearm.Ammo <= 0) return;
            if (firearm.Ammo != firearm.MaxAmmo)
            {
                ev.Player.ShowHUDHint("You must have full ammo to shoot", 5f);
                return;
            }
            firearm.Ammo = 0;
        }
        
        ev.Player.ThrowGrenade(ProjectileType.Scp018);
    }

    public void OnShot(ShotEventArgs ev)
    {
        
    }

    public void Initializing(Firearm firearm)
    {
        firearm.MaxAmmo /= 2;
    }
}*/