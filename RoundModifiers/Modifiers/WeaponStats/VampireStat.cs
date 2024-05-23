using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class VampireStat : Stat, IShooting
{
    public override string Name => "Vampire";
    public override int Rarity => 5;
    public override string Description => "This weapon will heal the owner for 10% of the damage dealt";
    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        float amount = ev.Damage * 0.1f;
        if(ev.Target.Role.Side==Side.Scp) amount *= 0.2f;
        ev.Player.Heal(amount, true);
    }
}