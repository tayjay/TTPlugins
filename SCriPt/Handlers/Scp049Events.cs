using System;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Warhead;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp049Events : IEventHandler
    {
        [MoonSharpVisible(true)]
        public event EventHandler<FinishingRecallEventArgs> FinishingRecall;
        
        [MoonSharpHidden]
        public void OnFinishingRecall(FinishingRecallEventArgs ev)
        {
            FinishingRecall?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<StartingRecallEventArgs> StartingRecall;
        
        [MoonSharpHidden]
        public void OnStartingRecall(StartingRecallEventArgs ev)
        {
            StartingRecall?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ActivatingSenseEventArgs> ActivatingSense;
        
        [MoonSharpHidden]
        public void OnActivatingSense(ActivatingSenseEventArgs ev)
        {
            ActivatingSense?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SendingCallEventArgs> SendingCall;
        
        [MoonSharpHidden]
        public void OnSendingCall(SendingCallEventArgs ev)
        {
            SendingCall?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<AttackingEventArgs> Attacking;
        
        [MoonSharpHidden]
        public void OnAttacking(AttackingEventArgs ev)
        {
            Attacking?.Invoke(null, ev);
        }
        
        
        // ///////////////////////////////////////
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp049.FinishingRecall += OnFinishingRecall;
            Exiled.Events.Handlers.Scp049.StartingRecall += OnStartingRecall;
            Exiled.Events.Handlers.Scp049.ActivatingSense += OnActivatingSense;
            Exiled.Events.Handlers.Scp049.SendingCall += OnSendingCall;
            Exiled.Events.Handlers.Scp049.Attacking += OnAttacking;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<FinishingRecallEventArgs>();
            UserData.RegisterType<StartingRecallEventArgs>();
            UserData.RegisterType<ActivatingSenseEventArgs>();
            UserData.RegisterType<SendingCallEventArgs>();
            UserData.RegisterType<AttackingEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp049.FinishingRecall -= OnFinishingRecall;
            Exiled.Events.Handlers.Scp049.StartingRecall -= OnStartingRecall;
            Exiled.Events.Handlers.Scp049.ActivatingSense -= OnActivatingSense;
            Exiled.Events.Handlers.Scp049.SendingCall -= OnSendingCall;
            Exiled.Events.Handlers.Scp049.Attacking -= OnAttacking;
        }
    }
}