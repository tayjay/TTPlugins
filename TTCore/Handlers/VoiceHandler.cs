using Exiled.Events.EventArgs.Player;
using TTCore.API;
using TTCore.Utilities;

namespace TTCore.Handlers;

public class VoiceHandler : IRegistered
{
    public bool ShouldModifyVoice { get; set; } = false;
    
    public void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        if (!ShouldModifyVoice)
            return;

        if (ev.Player.Scale.y != 1)
        {
            ev.VoiceMessage = AudioUtils.ModifyVoiceMessage(ev.VoiceMessage, 1/ev.Player.Scale.y);
        }
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