using Exiled.Events.EventArgs.Scp914;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IUpgradingPlayerEvent
    {
        void OnScp914UpgradingPlayer(UpgradingPlayerEventArgs ev);
    }
}