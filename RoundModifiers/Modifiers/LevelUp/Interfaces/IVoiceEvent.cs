using Exiled.Events.EventArgs.Scp939;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IVoiceEvent
{
    void OnSavingVoice(SavingVoiceEventArgs ev);
    
    void OnPlayingVoice(PlayingVoiceEventArgs ev);
    
    void OnPlayingSound(PlayingSoundEventArgs ev);
}