using Exiled.Events.EventArgs.Scp914;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IUpgradingPickupEvent
{
    void OnScp914UpgradingPickup(UpgradingPickupEventArgs ev);
}