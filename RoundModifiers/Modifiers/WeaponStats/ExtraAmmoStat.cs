using Exiled.API.Features;
using Exiled.API.Features.Items;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class ExtraAmmoStat : Stat, IInitializing
{

    public override string Name => "Extra Ammo";
    public override int Rarity => 3;
    public override string Description => $"This weapon has {AmmoMultiplier}x more ammo";
    
    public float AmmoMultiplier { get; set; }
    
    public ExtraAmmoStat(float ammoMultiplier)
    {
        AmmoMultiplier = ammoMultiplier;
    }
    
    public void Initializing(Firearm firearm)
    {
        firearm.MaxMagazineAmmo = (byte)(firearm.TotalMaxAmmo*AmmoMultiplier);
        firearm.MagazineAmmo = firearm.MaxMagazineAmmo;
    }

    public override double CalculateWeight()
    {
        return 1.0/(Rarity*AmmoMultiplier);
    }
}