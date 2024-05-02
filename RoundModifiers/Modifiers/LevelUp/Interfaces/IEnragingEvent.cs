using Exiled.Events.EventArgs.Scp096;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IEnragingEvent
{
    void OnEnraging(EnragingEventArgs ev);
}