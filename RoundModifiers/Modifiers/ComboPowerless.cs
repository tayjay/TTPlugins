using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class ComboPowerless : ComboModifier
{

    public ComboPowerless() : base()
    {
        AddModifier<Blackout>();
        AddModifier<RadioSilent>();
        AddModifier<NoCassie>();
        AddModifier<NoScp914>();
        AddModifier<Insurrection>();
        AddModifier<NoDecontamination>();
        AddModifier<MicroHIV>();
        AddModifier<NoKOS>();
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player.Role.Team != Team.ChaosInsurgency) return;
        if (ev.Player.HasItem(ItemType.GunFSP9))
        {
            ev.Player.RemoveItem(item => item.Type == ItemType.GunFSP9);
            ev.Player.AddItem(ItemType.GunAK);
        }
    }

    protected override void RegisterModifier()
    {
        base.RegisterModifier();
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
    }
    
    protected override void UnregisterModifier()
    {
        base.UnregisterModifier();
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "ComboPowerless",
        Aliases = new []{"powerless", "combopowerless"},
        FormattedName = "Combo Powerless",
        Description = "Combines the effects of the Blackout, RadioSilent, NoCassie, NoScp914, Insurrection, NoDecontamination, NoKOS, and MicroHIV modifiers.",
        MustPreload = false,
        Balance = -5,
        Impact = ImpactLevel.MajorGameplay,
        Category = Category.Combo
    };
}