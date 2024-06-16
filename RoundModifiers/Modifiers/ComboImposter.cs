using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class ComboImposter : ComboModifier
{
    public ComboImposter() : base()
    {
        AddModifier<Imposter>();
        AddModifier<ScpChat>();
        AddModifier<Nicknames>();
    }
    
    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "ComboImposter",
        Aliases = new []{"imposterplus", "comboimposter"},
        FormattedName = "Combo Imposter",
        Description = "Combines the effects of the Imposter, ScpChat, and Nicknames modifiers.",
        MustPreload = false,
        Balance = -4,
        Impact = ImpactLevel.MajorGameplay,
        Category = Category.Combo
    };
}