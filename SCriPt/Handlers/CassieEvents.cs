using System;
using Exiled.Events.EventArgs.Cassie;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class CassieEvents
    {
        [MoonSharpVisible(true)]
        public event EventHandler<SendingCassieMessageEventArgs> SendingCassieMessage;
        
        [MoonSharpHidden]
        public void OnSendingCassieMessage(SendingCassieMessageEventArgs ev)
        {
            SendingCassieMessage?.Invoke(null, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Cassie.SendingCassieMessage += OnSendingCassieMessage;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<SendingCassieMessageEventArgs>();
        }
        
        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Cassie.SendingCassieMessage -= OnSendingCassieMessage;
        }
        
    }
}