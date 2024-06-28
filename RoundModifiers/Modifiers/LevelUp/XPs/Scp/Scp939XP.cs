using Exiled.Events.EventArgs.Scp939;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class Scp939XP : ScpXP, IVoiceEvent
{
    public void OnSavingVoice(SavingVoiceEventArgs ev)
    {
        GiveXP(ev.Player, 30);
    }

    public void OnPlayingVoice(PlayingVoiceEventArgs ev)
    {
        GiveXP(ev.Player, 0.1f);
    }

    public void OnPlayingSound(PlayingSoundEventArgs ev)
    {
        GiveXP(ev.Player, 10);
    }
}