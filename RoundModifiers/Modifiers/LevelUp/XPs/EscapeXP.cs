 using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;


namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class EscapeXP : XP, IEscapingEvent
    {
        public List<uint> HasEscaped { get; set; }
        
        public EscapeXP() : base()
        {
            HasEscaped = ListPool<uint>.Pool.Get();
        }
        
        protected override bool CanGiveXP(Player player)
        {
            return HasEscaped.Contains(player.NetId) == false;
        }
        
        public void OnEscaping(EscapingEventArgs ev)
        {
            float xp = Math.Max(100,
                GetXPNeeded(RoundModifiers.Instance.GetModifier<LevelUp>().PlayerLevel[ev.Player.NetId])); //Give either 100xp, or enough to level up, whichever is more.
            GiveXP(ev.Player, xp);
            HasEscaped.Add(ev.Player.NetId);
            if (ev.Player.IsCuffed)
            {
                GiveXP(ev.Player.Cuffer, 100);
            }
        }
        
        public override void Reset()
        {
            HasEscaped.Clear();
            ListPool<uint>.Pool.Return(HasEscaped);
        }
        
        public override string Name { get; set; } = "Escape Facility";
        public override string Description { get; set; } = "Escape the facility as a Scientist or Class-D.";
        
    }
}