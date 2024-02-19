using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;


namespace RoundModifiers.Modifiers.LevelUp.XP
{
    public class EscapePocketDimensionXP : XP, IEscapingPocketDimensionEvent
    {
        public void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            GiveXP(ev.Player, 100);
        }
    }
}