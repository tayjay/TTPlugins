using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp939;
using MEC;
using TTCore.API;
using TTCore.Utilities;
using TTCore.Voice;
using VoiceChat.Networking;

namespace TTCore.Handlers;

public class VoiceHandler : IRegistered
{
    public bool ShouldModifyVoice { get; set; } = false;
    
    public void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        /*if (!ShouldModifyVoice)
            return;

        if (ev.Player.Scale.y != 1)
        {
            ev.VoiceMessage = AudioUtils.ModifyVoiceMessage(ev.VoiceMessage, 1/ev.Player.Scale.y);
        }*/
    }

    public void OnSavingVoice(SavingVoiceEventArgs ev)
    {
        Timing.CallDelayed(1f, () =>
        {
            
        });
    }

    public void Register()
    {
        Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
    }

    public void Unregister()
    {
        Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
    }
}