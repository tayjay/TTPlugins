using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Cassie;
using Exiled.Events.EventArgs.Map;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class Fun : Modifier
    {
        public string OldUnitName { get; set; } = "";
        public string NewUnitName { get; set; } = "";
        
        public void OnNTFAnnouncement(AnnouncingNtfEntranceEventArgs ev)
        {
            OldUnitName = ev.UnitName;
            ev.UnitName = Cassie.VoiceLines.Where(l => l.GetName().StartsWith(ev.UnitName.Substring(0, 1)))
                .GetRandomValue().GetName();
            NewUnitName = ev.UnitName;
        }

        public void OnCassieAnnouncement(SendingCassieMessageEventArgs ev)
        {
            /*if(ev.Words.ToLower().Contains(OldUnitName.ToLower()))
                ev.Words = ev.Words.ToLower().Replace(OldUnitName.ToLower(), NewUnitName.ToLower());*/
            //ev.Words = ".g7";
            Log.Info(ev.Words);
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnNTFAnnouncement;
            Exiled.Events.Handlers.Cassie.SendingCassieMessage += OnCassieAnnouncement;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnNTFAnnouncement;
            Exiled.Events.Handlers.Cassie.SendingCassieMessage -= OnCassieAnnouncement;
        }

        public override ModInfo ModInfo { get; } = new ModInfo() {
            Name = "Fun",
            FormattedName = "<color=white>Fun</color>",
            Aliases = new []{"fun"},
            Description = "For testing purposes only.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}