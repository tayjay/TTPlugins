using System;
using Exiled.Events.EventArgs.Warhead;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class WarheadEvents
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<StoppingEventArgs> Stopping;
        
        [MoonSharpHidden]
        public void OnStopping(StoppingEventArgs ev)
        {
            Stopping?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<StartingEventArgs> Starting;
        
        [MoonSharpHidden]
        public void OnStarting(StartingEventArgs ev)
        {
            Starting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingLeverStatusEventArgs> ChangingLeverStatus;
        
        [MoonSharpHidden]
        public void OnChangingLeverStatus(ChangingLeverStatusEventArgs ev)
        {
            ChangingLeverStatus?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler Detonated;
        
        [MoonSharpHidden]
        public void OnDetonated()
        {
            Detonated?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DetonatingEventArgs> Detonating;
        
        [MoonSharpHidden]
        public void OnDetonating(DetonatingEventArgs ev)
        {
            Detonating?.Invoke(null, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Warhead.Stopping += OnStopping;
            Exiled.Events.Handlers.Warhead.Starting += OnStarting;
            Exiled.Events.Handlers.Warhead.ChangingLeverStatus += OnChangingLeverStatus;
            Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
            Exiled.Events.Handlers.Warhead.Detonating += OnDetonating;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<StoppingEventArgs>();
            UserData.RegisterType<StartingEventArgs>();
            UserData.RegisterType<ChangingLeverStatusEventArgs>();
            UserData.RegisterType<DetonatingEventArgs>();
        }
        
        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Warhead.Stopping -= OnStopping;
            Exiled.Events.Handlers.Warhead.Starting -= OnStarting;
            Exiled.Events.Handlers.Warhead.ChangingLeverStatus -= OnChangingLeverStatus;
            Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
            Exiled.Events.Handlers.Warhead.Detonating -= OnDetonating;
        }
    }
}