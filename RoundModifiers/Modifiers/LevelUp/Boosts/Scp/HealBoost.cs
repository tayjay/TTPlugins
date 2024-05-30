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
        return ApplyBoost(player);
    }

    public override bool ApplyBoost(Player player)
    {
        player.Heal(100 + (100*(int)Tier), true);
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