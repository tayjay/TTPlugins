using System.Collections.Generic;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace TTCore.Voice;

public class TimedVoiceMessage
{
    private float startTime;
    public VoiceChatChannel Channel { get; private set; }
    public Dictionary<float,byte[]> Messages { get; private set; }
    
    public TimedVoiceMessage(VoiceChatChannel channel)
    {
        startTime = Time.time;
        Channel = channel;
        Messages = new();
    }

    public void AddMessage(VoiceMessage voiceMessage)
    {
        Messages.Add(Time.time - startTime, voiceMessage.Data);
    }
}