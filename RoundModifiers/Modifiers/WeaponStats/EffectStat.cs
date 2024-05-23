using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class EffectStat : Stat, IAdding
{
    public EffectType Type { get; set; }
    public byte Intensity { get; set; }

    
    
    public EffectStat(EffectType effectType, byte effectIntensity)
    {
        Type = effectType;
        Intensity = effectIntensity;
    }
    

    public override string Name => "Effect" + Type.ToString();
    public override int Rarity => 2;
    public override string Description=> "This weapon has an effect: " + Type.ToString() + " with intensity " + Intensity;

    public void Adding(Firearm firearm, Player player)
    {
        player.ChangeEffectIntensity(Type,Intensity);
    }

    public void Removing(Firearm firearm, Player player)
    {
        player.ChangeEffectIntensity(Type,(byte)(player.GetEffect(Type).Intensity-Intensity));
    }
    
    public override double CalculateWeight()
    {
        return 1.0/(Rarity + Intensity);
    }
}