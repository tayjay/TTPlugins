using System;
using Exiled.Events.EventArgs.Scp096;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp096Events : IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<EnragingEventArgs> Enraging;
        
        [MoonSharpHidden]
        public void OnEnraging(EnragingEventArgs ev)
        {
            Enraging?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<CalmingDownEventArgs> CalmingDown;
        
        [MoonSharpHidden]
        public void OnCalmingDown(CalmingDownEventArgs ev)
        {
            CalmingDown?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<AddingTargetEventArgs> AddingTarget;
        
        [MoonSharpHidden]
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            AddingTarget?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<StartPryingGateEventArgs> StartPryingGate;
        
        [MoonSharpHidden]
        public void OnStartPryingGate(StartPryingGateEventArgs ev)
        {
            StartPryingGate?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChargingEventArgs> Charging;
        
        [MoonSharpHidden]
        public void OnCharging(ChargingEventArgs ev)
        {
            Charging?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TryingNotToCryEventArgs> TryingNotToCry;
        
        [MoonSharpHidden]
        public void OnTryingNotToCry(TryingNotToCryEventArgs ev)
        {
            TryingNotToCry?.Invoke(this, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp096.Enraging += OnEnraging;
            Exiled.Events.Handlers.Scp096.CalmingDown += OnCalmingDown;
            Exiled.Events.Handlers.Scp096.AddingTarget += OnAddingTarget;
            Exiled.Events.Handlers.Scp096.StartPryingGate += OnStartPryingGate;
            Exiled.Events.Handlers.Scp096.Charging += OnCharging;
            Exiled.Events.Handlers.Scp096.TryingNotToCry += OnTryingNotToCry;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<EnragingEventArgs>();
            UserData.RegisterType<CalmingDownEventArgs>();
            UserData.RegisterType<AddingTargetEventArgs>();
            UserData.RegisterType<StartPryingGateEventArgs>();
            UserData.RegisterType<ChargingEventArgs>();
            UserData.RegisterType<TryingNotToCryEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp096.Enraging -= OnEnraging;
            Exiled.Events.Handlers.Scp096.CalmingDown -= OnCalmingDown;
            Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddingTarget;
            Exiled.Events.Handlers.Scp096.StartPryingGate -= OnStartPryingGate;
            Exiled.Events.Handlers.Scp096.Charging -= OnCharging;
            Exiled.Events.Handlers.Scp096.TryingNotToCry -= OnTryingNotToCry;
        }
    }
}