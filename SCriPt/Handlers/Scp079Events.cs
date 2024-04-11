using System;
using Exiled.Events.EventArgs.Scp079;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class Scp079Events : IEventHandler
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingCameraEventArgs> ChangingCamera;
        
        [MoonSharpHidden]
        public void OnChangingCamera(ChangingCameraEventArgs ev)
        {
            ChangingCamera?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<GainingExperienceEventArgs> GainingExperience;
        
        [MoonSharpHidden]
        public void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            GainingExperience?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<GainingLevelEventArgs> GainingLevel;
        
        [MoonSharpHidden]
        public void OnGainingLevel(GainingLevelEventArgs ev)
        {
            GainingLevel?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractingTeslaEventArgs> InteractingTesla;
        
        [MoonSharpHidden]
        public void OnInteractingTesla(InteractingTeslaEventArgs ev)
        {
            InteractingTesla?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TriggeringDoorEventArgs> TriggeringDoor;
        
        [MoonSharpHidden]
        public void OnTriggeringDoor(TriggeringDoorEventArgs ev)
        {
            TriggeringDoor?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ElevatorTeleportingEventArgs> ElevatorTeleporting;
        
        [MoonSharpHidden]
        public void OnElevatorTeleporting(ElevatorTeleportingEventArgs ev)
        {
            ElevatorTeleporting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<LockingDownEventArgs> LockingDown;
        
        [MoonSharpHidden]
        public void OnLockingDown(LockingDownEventArgs ev)
        {
            LockingDown?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingSpeakerStatusEventArgs> ChangingSpeakerStatus;
        
        [MoonSharpHidden]
        public void OnChangingSpeakerStatus(ChangingSpeakerStatusEventArgs ev)
        {
            ChangingSpeakerStatus?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RecontainedEventArgs> Recontained;
        
        [MoonSharpHidden]
        public void OnRecontained(RecontainedEventArgs ev)
        {
            Recontained?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<PingingEventArgs> Pinging;
        
        [MoonSharpHidden]
        public void OnPinging(PingingEventArgs ev)
        {
            Pinging?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RoomBlackoutEventArgs> RoomBlackout;
        
        [MoonSharpHidden]
        public void OnRoomBlackout(RoomBlackoutEventArgs ev)
        {
            RoomBlackout?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ZoneBlackoutEventArgs> ZoneBlackout;
        
        [MoonSharpHidden]
        public void OnZoneBlackout(ZoneBlackoutEventArgs ev)
        {
            ZoneBlackout?.Invoke(null, ev);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp079.ChangingCamera += OnChangingCamera;
            Exiled.Events.Handlers.Scp079.GainingExperience += OnGainingExperience;
            Exiled.Events.Handlers.Scp079.GainingLevel += OnGainingLevel;
            Exiled.Events.Handlers.Scp079.InteractingTesla += OnInteractingTesla;
            Exiled.Events.Handlers.Scp079.TriggeringDoor += OnTriggeringDoor;
            Exiled.Events.Handlers.Scp079.ElevatorTeleporting += OnElevatorTeleporting;
            Exiled.Events.Handlers.Scp079.LockingDown += OnLockingDown;
            Exiled.Events.Handlers.Scp079.ChangingSpeakerStatus += OnChangingSpeakerStatus;
            Exiled.Events.Handlers.Scp079.Recontained += OnRecontained;
            Exiled.Events.Handlers.Scp079.Pinging += OnPinging;
            Exiled.Events.Handlers.Scp079.RoomBlackout += OnRoomBlackout;
            Exiled.Events.Handlers.Scp079.ZoneBlackout += OnZoneBlackout;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<ChangingCameraEventArgs>();
            UserData.RegisterType<GainingExperienceEventArgs>();
            UserData.RegisterType<GainingLevelEventArgs>();
            UserData.RegisterType<InteractingTeslaEventArgs>();
            UserData.RegisterType<TriggeringDoorEventArgs>();
            UserData.RegisterType<ElevatorTeleportingEventArgs>();
            UserData.RegisterType<LockingDownEventArgs>();
            UserData.RegisterType<ChangingSpeakerStatusEventArgs>();
            UserData.RegisterType<RecontainedEventArgs>();
            UserData.RegisterType<PingingEventArgs>();
            UserData.RegisterType<RoomBlackoutEventArgs>();
            UserData.RegisterType<ZoneBlackoutEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp079.ChangingCamera -= OnChangingCamera;
            Exiled.Events.Handlers.Scp079.GainingExperience -= OnGainingExperience;
            Exiled.Events.Handlers.Scp079.GainingLevel -= OnGainingLevel;
            Exiled.Events.Handlers.Scp079.InteractingTesla -= OnInteractingTesla;
            Exiled.Events.Handlers.Scp079.TriggeringDoor -= OnTriggeringDoor;
            Exiled.Events.Handlers.Scp079.ElevatorTeleporting -= OnElevatorTeleporting;
            Exiled.Events.Handlers.Scp079.LockingDown -= OnLockingDown;
            Exiled.Events.Handlers.Scp079.ChangingSpeakerStatus -= OnChangingSpeakerStatus;
            Exiled.Events.Handlers.Scp079.Recontained -= OnRecontained;
            Exiled.Events.Handlers.Scp079.Pinging -= OnPinging;
            Exiled.Events.Handlers.Scp079.RoomBlackout -= OnRoomBlackout;
            Exiled.Events.Handlers.Scp079.ZoneBlackout -= OnZoneBlackout;
        }
    }
}