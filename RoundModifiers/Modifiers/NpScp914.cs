using Exiled.Events.EventArgs.Scp914;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class NpScp914 : Modifier
    {
        public void OnActivatingScp914(ActivatingEventArgs ev)
        {
            ev.Player.ShowHint("SCP-914 is out of order.");
            ev.IsAllowed = false;
        }

        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Scp914.Activating += OnActivatingScp914;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Scp914.Activating -= OnActivatingScp914;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "NoScp914",
            Aliases = new []{"914"},
            Description = "SCP-914 is out of order.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}