using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class DangerZone : Modifier
{

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if(ev.Player.Role.Type.IsHuman())
            ev.Player.EnableEffect<Scp1853>();
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "DangerZone",
        FormattedName = "<color=green>Danger Zone</color>",
        Description = "Players are affected by SCP-1853.",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 2,
        Category = Category.Effect
    };
}