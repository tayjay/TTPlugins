using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp106;
using Exiled.Events.Features;
using TTCore.API;
using TTCore.Utilities;

namespace TTAddons.Handlers
{
    public class LarryLagHandler : IRegistered
    {

        public void OnWaitingForPlayers()
        {
            Log.Info("Checking for Larry Lag...");
            CustomEventHandler<ExitStalkingEventArgs> handler = Exiled.Events.Handlers.Scp106.ExitStalking.GetPrivateFieldValue<CustomEventHandler<ExitStalkingEventArgs>>("InnerEvent");
            foreach (Delegate item in handler.GetInvocationList())
            {
                Log.Info("Found Handler: " + item.Method.Name);
            }
            
            CustomAsyncEventHandler<ExitStalkingEventArgs> asyncHandler = Exiled.Events.Handlers.Scp106.ExitStalking.GetPrivateFieldValue<CustomAsyncEventHandler<ExitStalkingEventArgs>>("InnerAsyncEvent");
            
            foreach (Delegate item in asyncHandler.GetInvocationList())
            {
                Log.Info("Found Async Handler: " + item.Method.Name);
            }
        }
        
        
        public void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }
    }
}