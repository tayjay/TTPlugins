using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IPocketDimensionEvent
{
    void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev);
}