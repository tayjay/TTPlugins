using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IEscapingPocketDimensionEvent
    {
        void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev);
    }
}