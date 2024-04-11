using System;
using Exiled.Events.EventArgs.Scp173;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp173Events : IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<BlinkingEventArgs> Blinking;
        
        [MoonSharpHidden]
        public void OnBlinking(BlinkingEventArgs ev)
        {
            Blinking?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<BlinkingRequestEventArgs> BlinkingRequest; 
        
        [MoonSharpHidden]
        public void OnBlinkingRequest(BlinkingRequestEventArgs ev)
        {
            BlinkingRequest?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<PlacingTantrumEventArgs> PlacingTantrum;
        
        [MoonSharpHidden]
        public void OnPlacingTantrum(PlacingTantrumEventArgs ev)
        {
            PlacingTantrum?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsingBreakneckSpeedsEventArgs> UsingBreakneckSpeeds;
        
        [MoonSharpHidden]
        public void OnUsingBreakneckSpeeds(UsingBreakneckSpeedsEventArgs ev)
        {
            UsingBreakneckSpeeds?.Invoke(this, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp173.Blinking += OnBlinking;
            Exiled.Events.Handlers.Scp173.BlinkingRequest += OnBlinkingRequest;
            Exiled.Events.Handlers.Scp173.PlacingTantrum += OnPlacingTantrum;
            Exiled.Events.Handlers.Scp173.UsingBreakneckSpeeds += OnUsingBreakneckSpeeds;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<BlinkingEventArgs>();
            UserData.RegisterType<BlinkingRequestEventArgs>();
            UserData.RegisterType<PlacingTantrumEventArgs>();
            UserData.RegisterType<UsingBreakneckSpeedsEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp173.Blinking -= OnBlinking;
            Exiled.Events.Handlers.Scp173.BlinkingRequest -= OnBlinkingRequest;
            Exiled.Events.Handlers.Scp173.PlacingTantrum -= OnPlacingTantrum;
            Exiled.Events.Handlers.Scp173.UsingBreakneckSpeeds -= OnUsingBreakneckSpeeds;
        }
    }
}