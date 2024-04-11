using System;
using Exiled.Events.EventArgs.Scp244;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp244Events : IEventHandler
    {
        [MoonSharpVisible(true)]
        public event EventHandler<UsingScp244EventArgs> UsingScp244;
        
        [MoonSharpHidden]
        public void OnUsingScp244(UsingScp244EventArgs ev)
        {
            UsingScp244?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DamagingScp244EventArgs> DamagingScp244;
        
        [MoonSharpHidden]
        public void OnDamagingScp244(DamagingScp244EventArgs ev)
        {
            DamagingScp244?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<OpeningScp244EventArgs> OpeningScp244;
        
        [MoonSharpHidden]
        public void OnOpeningScp244(OpeningScp244EventArgs ev)
        {
            OpeningScp244?.Invoke(this, ev);
        }
        
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp244.UsingScp244 += OnUsingScp244;
            Exiled.Events.Handlers.Scp244.DamagingScp244 += OnDamagingScp244;
            Exiled.Events.Handlers.Scp244.OpeningScp244 += OnOpeningScp244;
        }

        public void RegisterEventTypes()
        {
            UserData.RegisterType<UsingScp244EventArgs>();
            UserData.RegisterType<DamagingScp244EventArgs>();
            UserData.RegisterType<OpeningScp244EventArgs>();
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp244.UsingScp244 -= OnUsingScp244;
            Exiled.Events.Handlers.Scp244.DamagingScp244 -= OnDamagingScp244;
            Exiled.Events.Handlers.Scp244.OpeningScp244 -= OnOpeningScp244;
        }
    }
}