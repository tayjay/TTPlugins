using System;
using Exiled.Events.EventArgs.Scp0492;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp0492Events : IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<TriggeringBloodlustEventArgs> TriggeringBloodlust;
        
        [MoonSharpHidden]
        public void OnTriggeringBloodlust(TriggeringBloodlustEventArgs ev)
        {
            TriggeringBloodlust?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ConsumedCorpseEventArgs> ConsumedCorpse;
        
        [MoonSharpHidden]
        public void OnConsumedCorpse(ConsumedCorpseEventArgs ev)
        {
            ConsumedCorpse?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ConsumingCorpseEventArgs> ConsumingCorpse;
        
        [MoonSharpHidden]
        public void OnConsumingCorpse(ConsumingCorpseEventArgs ev)
        {
            ConsumingCorpse?.Invoke(null, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp0492.TriggeringBloodlust += OnTriggeringBloodlust;
            Exiled.Events.Handlers.Scp0492.ConsumedCorpse += OnConsumedCorpse;
            Exiled.Events.Handlers.Scp0492.ConsumingCorpse += OnConsumingCorpse;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<TriggeringBloodlustEventArgs>();
            UserData.RegisterType<ConsumedCorpseEventArgs>();
            UserData.RegisterType<ConsumingCorpseEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp0492.TriggeringBloodlust -= OnTriggeringBloodlust;
            Exiled.Events.Handlers.Scp0492.ConsumedCorpse -= OnConsumedCorpse;
            Exiled.Events.Handlers.Scp0492.ConsumingCorpse -= OnConsumingCorpse;
        }
    }
}