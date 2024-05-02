using Exiled.Events.EventArgs.Scp096;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class Scp096XP : ScpXP, IEnragingEvent
{
    public void OnEnraging(EnragingEventArgs ev)
    {
        GiveXP(ev.Player, 100);
    }
}