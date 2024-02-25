using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using VoiceChat;

namespace RoundModifiers.Modifiers
{
    public class RadioSilent : Modifier
    {
        
        public void OnTransmitting(TransmittingEventArgs ev)
        {
            //Log.Info("Transmitting");
            
            //Log.Info("Transmitting Blocked.");
            ev.IsAllowed = false;
            
        }
        
        public void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (ev.IsAllowed && ev.VoiceMessage.Channel == VoiceChatChannel.Radio)
            {
                //Log.Info("Transmitting Blocked.");
                ev.IsAllowed = false;
            }
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Transmitting += OnTransmitting;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Transmitting -= OnTransmitting;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "RadioSilent",
            FormattedName = "<color=white>Radio Silent</color>",
            Aliases = new []{"noradio", "silentradio"},
            Description = "Players cannot use the radio.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}