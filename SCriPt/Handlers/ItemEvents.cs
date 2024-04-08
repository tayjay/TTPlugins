using System;
using Exiled.Events.EventArgs.Cassie;
using Exiled.Events.EventArgs.Item;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class ItemEvents
    {
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingAmmoEventArgs> ChangingAmmo;
        
        [MoonSharpHidden]
        public void OnChangingAmmo(ChangingAmmoEventArgs ev)
        {
            ChangingAmmo?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingAttachmentsEventArgs> ChangingAttachments;
        
        [MoonSharpHidden]
        public void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            ChangingAttachments?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ReceivingPreferenceEventArgs> ReceivingPreference;
        
        [MoonSharpHidden]
        public void OnReceivingPreference(ReceivingPreferenceEventArgs ev)
        {
            ReceivingPreference?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<KeycardInteractingEventArgs> KeycardInteracting;
        
        [MoonSharpHidden]
        public void OnKeycardInteracting(KeycardInteractingEventArgs ev)
        {
            KeycardInteracting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SwingingEventArgs> SwingingJailbird;
        
        [MoonSharpHidden]
        public void OnSwinging(SwingingEventArgs ev)
        {
            SwingingJailbird?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChargingJailbirdEventArgs> ChargingJailbird;
        
        [MoonSharpHidden]
        public void OnChargingJailbird(ChargingJailbirdEventArgs ev)
        {
            ChargingJailbird?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsingRadioPickupBatteryEventArgs> UsingRadioPickupBattery;
        
        [MoonSharpHidden]
        public void OnUsingRadioPickupBattery(UsingRadioPickupBatteryEventArgs ev)
        {
            UsingRadioPickupBattery?.Invoke(null, ev);
        }
        
        
        /////////////////////////////////////////////
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAmmo += OnChangingAmmo;
            Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachments;
            Exiled.Events.Handlers.Item.ReceivingPreference += OnReceivingPreference;
            Exiled.Events.Handlers.Item.KeycardInteracting += OnKeycardInteracting;
            Exiled.Events.Handlers.Item.Swinging += OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
            Exiled.Events.Handlers.Item.UsingRadioPickupBattery += OnUsingRadioPickupBattery;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<ChangingAmmoEventArgs>();
            UserData.RegisterType<ChangingAttachmentsEventArgs>();
            UserData.RegisterType<ReceivingPreferenceEventArgs>();
            UserData.RegisterType<KeycardInteractingEventArgs>();
            UserData.RegisterType<SwingingEventArgs>();
            UserData.RegisterType<ChargingJailbirdEventArgs>();
            UserData.RegisterType<UsingRadioPickupBatteryEventArgs>();
        }
        
        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Item.ChangingAmmo -= OnChangingAmmo;
            Exiled.Events.Handlers.Item.ChangingAttachments -= OnChangingAttachments;
            Exiled.Events.Handlers.Item.ReceivingPreference -= OnReceivingPreference;
            Exiled.Events.Handlers.Item.KeycardInteracting -= OnKeycardInteracting;
            Exiled.Events.Handlers.Item.Swinging -= OnSwinging;
            Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
            Exiled.Events.Handlers.Item.UsingRadioPickupBattery -= OnUsingRadioPickupBattery;
        }
    }
}