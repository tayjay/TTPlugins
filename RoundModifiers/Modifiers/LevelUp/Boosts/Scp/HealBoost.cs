using System;
using Exiled.API.Features;

namespace RoundModifiers.Modifiers.LevelUp.Boosts.Scp;

public class HealBoost : Boost
{
    public HealBoost(Tier tier) : base(tier)
    {
    }

    public override bool AssignBoost(Player player)
    {
        if(!player.IsScp) return false;
        if(HasBoost.TryGetValue(player.NetId, out bool hasBoost) && hasBoost) return false;
        return ApplyBoost(player);
    }

    public override bool ApplyBoost(Player player)
    {
        if (Tier > Tier.Rare)
        {
            HasBoost.Add(player.NetId, true);
        }
        float healAmount = 100 + (50*(int)Tier);
        if(player.Health + healAmount > player.MaxHealth*1.5f) return false; //Don't extra overheal
        float overheal = player.Health + healAmount - player.MaxHealth;
        if(Tier >=Tier.Rare && (overheal>0)) player.HumeShield += overheal/2;
        player.Heal(healAmount, false);
        return true;
    }

    public override string GetName()
    {
        string name = "Heal";
        switch (Tier)
        {
            case Tier.Common:
                name += " I";
                break;
            case Tier.Uncommon:
                name += " II";
                break;
            case Tier.Rare:
                name += " III";
                break;
            case Tier.Epic:
                name += " IV";
                break;
            case Tier.Legendary:
                name += " V";
                break;
        }
        return name;
    }

    public override string GetDescription()
    {
        return "Get a small heal boost for SCPs.";
    }
}