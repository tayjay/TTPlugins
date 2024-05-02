using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class KillXP : ScpXP, IDiedEvent
{
    public void OnDied(DiedEventArgs ev)
    {
        if(ev.Attacker.IsScp)
            GiveXP(ev.Attacker, 100);
    }
}