using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace RoundModifiers.Modifiers.WeaponStats;

public class NoneStat : Stat
{
    public override string Name => "None";
    public override int Rarity=> 0;
    public override string Description => "This weapon has no special stats";
    
    public override double CalculateWeight()
    {
        return 1.0;
    }
}