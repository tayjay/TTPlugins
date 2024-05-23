using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class NoJumping : Modifier
{

    public void OnJumping(JumpingEventArgs ev)
    {
        ev.IsAllowed = false;
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Jumping += OnJumping;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Jumping -= OnJumping;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "NoJumping",
        Description = "Players can't jump",
        Aliases = new []{"jump"},
        FormattedName = "<color=red>No Jumping</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
}