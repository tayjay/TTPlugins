using System;
using Exiled.Events.EventArgs.Scp106;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp106Events : IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<AttackingEventArgs> Attacking;
        
        [MoonSharpHidden]
        public void OnAttacking(AttackingEventArgs ev)
        {
            Attacking?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TeleportingEventArgs> Teleporting;
        
        [MoonSharpHidden]
        public void OnTeleporting(TeleportingEventArgs ev)
        {
            Teleporting?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<StalkingEventArgs> Stalking;
        
        [MoonSharpHidden]
        public void OnStalking(StalkingEventArgs ev)
        {
            Stalking?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ExitStalkingEventArgs> ExitStalking;
        
        [MoonSharpHidden]
        public void OnExitStalking(ExitStalkingEventArgs ev)
        {
            ExitStalking?.Invoke(this, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp106.Attacking += OnAttacking;
            Exiled.Events.Handlers.Scp106.Teleporting += OnTeleporting;
            Exiled.Events.Handlers.Scp106.Stalking += OnStalking;
            Exiled.Events.Handlers.Scp106.ExitStalking += OnExitStalking;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<AttackingEventArgs>();
            UserData.RegisterType<TeleportingEventArgs>();
            UserData.RegisterType<StalkingEventArgs>();
            UserData.RegisterType<ExitStalkingEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp106.Attacking -= OnAttacking;
            Exiled.Events.Handlers.Scp106.Teleporting -= OnTeleporting;
            Exiled.Events.Handlers.Scp106.Stalking -= OnStalking;
            Exiled.Events.Handlers.Scp106.ExitStalking -= OnExitStalking;
        }
    }
}