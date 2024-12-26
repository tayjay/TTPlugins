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
            Log.Warn("===========================================");
            Log.Warn("Checking for Larry Lag...");
            CustomEventHandler<ExitStalkingEventArgs> handler = Exiled.Events.Handlers.Scp106.ExitStalking.GetPrivateFieldValue<CustomEventHandler<ExitStalkingEventArgs>>("InnerEvent");
            Log.Warn($"Found {handler.GetInvocationList().Length} handlers: ");
            foreach (Delegate item in handler.GetInvocationList())
            {
                Log.Warn("Found Handler: " + item.Method.Name);
            }
            Log.Warn("-------------------------------------------");
            CustomAsyncEventHandler<ExitStalkingEventArgs> asyncHandler = Exiled.Events.Handlers.Scp106.ExitStalking.GetPrivateFieldValue<CustomAsyncEventHandler<ExitStalkingEventArgs>>("InnerAsyncEvent");
            Log.Warn($"Found {asyncHandler.GetInvocationList().Length} async handlers: ");
            foreach (Delegate item in asyncHandler.GetInvocationList())
            {
                Log.Warn("Found Async Handler: " + item.Method.Name);
            }
            Log.Warn("===========================================");
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