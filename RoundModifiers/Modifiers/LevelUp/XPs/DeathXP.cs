using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XP
{
    public class DeathXP : XP, IDiedEvent
    {
        public void OnDied(DiedEventArgs ev)
        {
            if(ev.Attacker == null) return;
            
            GiveXP(ev.Player, 10);
        }
    }
}