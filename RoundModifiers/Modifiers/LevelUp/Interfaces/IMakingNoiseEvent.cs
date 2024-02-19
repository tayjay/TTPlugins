using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IMakingNoiseEvent
    {
        void OnMakingNoise(MakingNoiseEventArgs ev);
    }
}