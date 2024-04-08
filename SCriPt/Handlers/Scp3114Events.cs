using System;
using Exiled.Events.EventArgs.Scp3114;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp3114Events : IEventHandler
    {
        [MoonSharpVisible(true)]
        public event EventHandler<DisguisingEventArgs> Disguising;
        
        [MoonSharpHidden]
        public void OnDisguising(DisguisingEventArgs ev)
        {
            Disguising?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DisguisedEventArgs> Disguised;
        
        [MoonSharpHidden]
        public void OnDisguised(DisguisedEventArgs ev)
        {
            Disguised?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TryUseBodyEventArgs> TryUseBody;
        
        [MoonSharpHidden]
        public void OnTryUseBody(TryUseBodyEventArgs ev)
        {
            TryUseBody?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RevealedEventArgs> Revealed;
        
        [MoonSharpHidden]
        public void OnRevealed(RevealedEventArgs ev)
        {
            Revealed?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RevealingEventArgs> Revealing;
        
        [MoonSharpHidden]
        public void OnRevealing(RevealingEventArgs ev)
        {
            Revealing?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<VoiceLinesEventArgs> VoiceLines;
        
        [MoonSharpHidden]
        public void OnVoiceLines(VoiceLinesEventArgs ev)
        {
            VoiceLines?.Invoke(null, ev);
        }
        
        
        
        //-----------------------------------------
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp3114.Disguising += OnDisguising;
            Exiled.Events.Handlers.Scp3114.Disguised += OnDisguised;
            Exiled.Events.Handlers.Scp3114.TryUseBody += OnTryUseBody;
            Exiled.Events.Handlers.Scp3114.Revealed += OnRevealed;
            Exiled.Events.Handlers.Scp3114.Revealing += OnRevealing;
            Exiled.Events.Handlers.Scp3114.VoiceLines += OnVoiceLines;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<DisguisingEventArgs>();
            UserData.RegisterType<DisguisedEventArgs>();
            UserData.RegisterType<TryUseBodyEventArgs>();
            UserData.RegisterType<RevealedEventArgs>();
            UserData.RegisterType<RevealingEventArgs>();
            UserData.RegisterType<VoiceLinesEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp3114.Disguising -= OnDisguising;
            Exiled.Events.Handlers.Scp3114.Disguised -= OnDisguised;
            Exiled.Events.Handlers.Scp3114.TryUseBody -= OnTryUseBody;
            Exiled.Events.Handlers.Scp3114.Revealed -= OnRevealed;
            Exiled.Events.Handlers.Scp3114.Revealing -= OnRevealing;
            Exiled.Events.Handlers.Scp3114.VoiceLines -= OnVoiceLines;
        }
    }
}