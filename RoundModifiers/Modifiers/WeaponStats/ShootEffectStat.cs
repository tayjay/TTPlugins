using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class ShootEffectStat : Stat, IShooting
{
    
    public EffectType Type { get; set; }
    
    public byte Intensity { get; set; }
    public byte Duration { get; set; }
    
    public ShootEffectStat(EffectType effectType, byte effectIntensity, byte effectDuration)
    {
        Type = effectType;
        Intensity = effectIntensity;
        Duration = effectDuration;
    }

    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        ev.Player.EnableEffect(Type,Intensity,Duration,true);
    }

    public override double CalculateWeight()
    {
        return Rarity + Intensity;
    }

    public override string Name => "Shoot Effect: " + Type.ToString();
    public override int Rarity => 3;
    public override string Description => "This weapon gives the effect: " + Type.ToString() + " when fired";
    
}