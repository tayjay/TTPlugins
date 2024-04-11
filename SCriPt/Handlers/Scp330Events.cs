using System;
using Exiled.Events.EventArgs.Scp330;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp330Events :IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractingScp330EventArgs> InteractingScp330;
        
        [MoonSharpHidden]
        public void OnInteractingScp330(InteractingScp330EventArgs ev)
        {
            InteractingScp330?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DroppingScp330EventArgs> DroppingScp330;
        
        [MoonSharpHidden]
        public void OnDroppingScp330(DroppingScp330EventArgs ev)
        {
            DroppingScp330?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EatingScp330EventArgs> EatingScp330;
        
        [MoonSharpHidden]
        public void OnEatingScp330(EatingScp330EventArgs ev)
        {
            EatingScp330?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EatenScp330EventArgs> EatenScp330;
        
        [MoonSharpHidden]
        public void OnEatenScp330(EatenScp330EventArgs ev)
        {
            EatenScp330?.Invoke(this, ev);
        }
        
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp330.InteractingScp330 += OnInteractingScp330;
            Exiled.Events.Handlers.Scp330.DroppingScp330 += OnDroppingScp330;
            Exiled.Events.Handlers.Scp330.EatingScp330 += OnEatingScp330;
            Exiled.Events.Handlers.Scp330.EatenScp330 += OnEatenScp330;
        }

        public void RegisterEventTypes()
        {
            UserData.RegisterType<InteractingScp330EventArgs>();
            UserData.RegisterType<DroppingScp330EventArgs>();
            UserData.RegisterType<EatingScp330EventArgs>();
            UserData.RegisterType<EatenScp330EventArgs>();
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp330.InteractingScp330 -= OnInteractingScp330;
            Exiled.Events.Handlers.Scp330.DroppingScp330 -= OnDroppingScp330;
            Exiled.Events.Handlers.Scp330.EatingScp330 -= OnEatingScp330;
            Exiled.Events.Handlers.Scp330.EatenScp330 -= OnEatenScp330;
        }
    }
}