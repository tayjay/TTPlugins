using System;
using Exiled.Events.EventArgs.Scp914;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp914Events : IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<UpgradingPickupEventArgs> UpgradingPickup;
        
        [MoonSharpHidden]
        public void OnUpgradingPickup(UpgradingPickupEventArgs ev)
        {
            UpgradingPickup?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UpgradingInventoryItemEventArgs> UpgradingInventoryItem;
        
        [MoonSharpHidden]
        public void OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs ev)
        {
            UpgradingInventoryItem?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UpgradingPlayerEventArgs> UpgradingPlayer;
        
        [MoonSharpHidden]
        public void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            UpgradingPlayer?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ActivatingEventArgs> Activating;
        
        [MoonSharpHidden]
        public void OnActivating(ActivatingEventArgs ev)
        {
            Activating?.Invoke(this, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPickup += OnUpgradingPickup;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += OnUpgradingInventoryItem;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += OnUpgradingPlayer;
            Exiled.Events.Handlers.Scp914.Activating += OnActivating;
        }

        public void RegisterEventTypes()
        {
            UserData.RegisterType<UpgradingPickupEventArgs>();
            UserData.RegisterType<UpgradingInventoryItemEventArgs>();
            UserData.RegisterType<UpgradingPlayerEventArgs>();
            UserData.RegisterType<ActivatingEventArgs>();
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPickup -= OnUpgradingPickup;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnUpgradingInventoryItem;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= OnUpgradingPlayer;
            Exiled.Events.Handlers.Scp914.Activating -= OnActivating;
        }
    }
}