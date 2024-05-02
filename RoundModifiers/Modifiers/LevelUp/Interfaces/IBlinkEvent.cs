using Exiled.Events.EventArgs.Scp173;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IBlinkEvent
{
    /// <summary>
    /// Called when SCP-173 is requesting to blink. Causes all listed players to blink and teleports 173
    /// </summary>
    void OnBlinkingRequest(BlinkingRequestEventArgs ev);
}