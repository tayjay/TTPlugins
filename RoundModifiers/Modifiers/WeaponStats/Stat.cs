namespace RoundModifiers.Modifiers.WeaponStats;

public abstract class Stat
{
    public abstract string Name { get; }
    public abstract int Rarity { get; }
    public abstract string Description { get; }
    
    public virtual double CalculateWeight() => 1.0/Rarity;
}