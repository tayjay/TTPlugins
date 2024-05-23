using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class SharpStat : Stat, IShooting
{
    public override string Name => "Sharp";
    
    public override int Rarity => 4;
    
    public override string Description => "This weapon causes bleeding on targets, dealing damage over time\nHowever, there is a chance it cuts your hands off.";

    public SharpStat()
    {
        
    }

    public void OnShooting(ShootingEventArgs ev)
    {
        throw new System.NotImplementedException();
    }

    public void OnShot(ShotEventArgs ev)
    {
        
        if (UnityEngine.Random.Range(0, 1000) < 5)
        {
            ev.Player.EnableEffect<SeveredHands>();
            ev.Player.ShowHint("Your hands were cut off by the weapon", 5f);
        }
        if (ev.Target == null) return;
        ev.Target.EnableEffect<Bleeding>(1, 1, true);
    }
}