using TTCore.Events.EventArgs;

namespace RoundModifiers.Modifiers.WeaponStats.Interfaces;

public interface IInspecting
{
    void Inspecting(InspectFirearmEventArgs ev);
}