using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class ExtraDamageStat : Stat, IShooting
{
    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        if (ev.Target == null) return;

        ev.Damage *= 1.5f;
    }

    public override string Name => "Extra Damage";
    public override int Rarity => 3;
    public override string Description => "This weapon deals 50% more damage";
}