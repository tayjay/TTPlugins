using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IMakingNoiseEvent
    {
        void OnMakingNoise(MakingNoiseEventArgs ev);
    }
}