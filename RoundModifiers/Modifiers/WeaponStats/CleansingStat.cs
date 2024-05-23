using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

public class CleansingStat : Stat, IShooting
{

    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        if (ev.Target == null) return;
        
        float random = UnityEngine.Random.Range(0, 100);
        if (random > 10) return;
        if (ev.Target.Role.Side == ev.Player.Role.Side)
        {
            //Shot an ally
            ev.Target.DisableAllEffects(EffectCategory.Harmful);
            ev.Target.DisableAllEffects(EffectCategory.Negative);
            ev.Player.ShowHUDHint("Cleansed", 5f);
        }
        else
        {
            //Shot an enemy
            ev.Target.DisableAllEffects();
            ev.Player.ShowHUDHint("Cleansed", 5f);
        }
    }

    public override string Name => "Cleansing";
    public override int Rarity => 5;
    public override string Description => "This weapon has a chance to cleanse effects from the target";
}