using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;


namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class EscapeXP : XP, IEscapingEvent
    {
        public List<uint> HasEscaped = new List<uint>();
        
        protected override bool CanGiveXP(Player player)
        {
            return HasEscaped.Contains(player.NetId) == false;
        }
        
        public void OnEscaping(EscapingEventArgs ev)
        {
            GiveXP(ev.Player, 100);
            HasEscaped.Add(ev.Player.NetId);
        }
        
        public override void Reset()
        {
            HasEscaped.Clear();
        }
        
        public override string Name { get; set; } = "Escape Facility";
        public override string Description { get; set; } = "Escape the facility as a Scientist or Class-D.";
        
    }
}