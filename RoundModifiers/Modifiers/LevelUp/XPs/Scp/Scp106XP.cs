using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class Scp106XP : ScpXP, IPocketDimensionEvent
{
    public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
    {
        GiveXP(ev.Scp106, 50);
    }
}