using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IKeybindAbility
{
    void OnPressKeybind(TogglingNoClipEventArgs ev);
}