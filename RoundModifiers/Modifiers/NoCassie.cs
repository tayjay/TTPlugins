using Exiled.Events.EventArgs.Cassie;
using Exiled.Events.EventArgs.Map;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class NoCassie : Modifier
    {

        public void OnCassieMessage(SendingCassieMessageEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        
        public void OnAnnounceNtf(AnnouncingNtfEntranceEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        
        public void OnAnnounceScpTerminate(AnnouncingScpTerminationEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Cassie.SendingCassieMessage += OnCassieMessage;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnAnnounceNtf;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination += OnAnnounceScpTerminate;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Cassie.SendingCassieMessage -= OnCassieMessage;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnAnnounceNtf;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination -= OnAnnounceScpTerminate;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "NoCassie",
            FormattedName = "<color=red>No CASSIE</color>",
            Aliases = new []{"cassie"},
            Description = "Disables all Cassie announcements.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = -1,
            Category = Category.Facility
        };
    }
}