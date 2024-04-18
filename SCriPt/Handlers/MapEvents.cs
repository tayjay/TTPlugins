using System;
using Exiled.Events.EventArgs.Map;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class MapEvents : IEventHandler
    {
        [MoonSharpVisible(true)] public event EventHandler<PlacingBulletHoleEventArgs> PlacingBulletHole;
        
        [MoonSharpHidden]
        public void OnPlacingBulletHole(PlacingBulletHoleEventArgs ev)
        {
            PlacingBulletHole?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<PlacingBloodEventArgs> PlacingBlood;
        
        [MoonSharpHidden]
        public void OnPlacingBlood(PlacingBloodEventArgs ev)
        {
            PlacingBlood?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<AnnouncingDecontaminationEventArgs> AnnouncingDecontamination;
        
        [MoonSharpHidden]
        public void OnAnnouncingDecontamination(AnnouncingDecontaminationEventArgs ev)
        {
            AnnouncingDecontamination?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<AnnouncingScpTerminationEventArgs> AnnouncingScpTermination;
        
        [MoonSharpHidden]
        public void OnAnnouncingScpTermination(AnnouncingScpTerminationEventArgs ev)
        {
            AnnouncingScpTermination?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<AnnouncingNtfEntranceEventArgs> AnnouncingNtfEntrance;
        
        [MoonSharpHidden]
        public void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev)
        {
            AnnouncingNtfEntrance?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<GeneratorActivatingEventArgs> GeneratorActivating;
        
        [MoonSharpHidden]
        public void OnGeneratorActivating(GeneratorActivatingEventArgs ev)
        {
            GeneratorActivating?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<DecontaminatingEventArgs> Decontaminating;
        
        [MoonSharpHidden]
        public void OnDecontaminating(DecontaminatingEventArgs ev)
        {
            Decontaminating?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<ExplodingGrenadeEventArgs> ExplodingGrenade;
        
        [MoonSharpHidden]
        public void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            ExplodingGrenade?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<SpawningItemEventArgs> SpawningItem;
        
        [MoonSharpHidden]
        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            SpawningItem?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<FillingLockerEventArgs> FillingLocker;
        
        [MoonSharpHidden]
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            FillingLocker?.Invoke(this, ev);
        }

        [MoonSharpVisible(true)] public event EventHandler Generated;
        
        [MoonSharpHidden]
        public void OnGenerated()
        {
            Generated?.Invoke(this, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<ChangingIntoGrenadeEventArgs> ChangingIntoGrenade;
        
        [MoonSharpHidden]
        public void OnChangingIntoGrenade(ChangingIntoGrenadeEventArgs ev)
        {
            ChangingIntoGrenade?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<ChangedIntoGrenadeEventArgs> ChangedIntoGrenade;
        
        [MoonSharpHidden]
        public void OnChangedIntoGrenade(ChangedIntoGrenadeEventArgs ev)
        {
            ChangedIntoGrenade?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<TurningOffLightsEventArgs> TurningOffLights;
        
        [MoonSharpHidden]
        public void OnTurningOffLights(TurningOffLightsEventArgs ev)
        {
            TurningOffLights?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<PickupAddedEventArgs> PickupAdded;
        
        [MoonSharpHidden]
        public void OnPickupAdded(PickupAddedEventArgs ev)
        {
            PickupAdded?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<PickupDestroyedEventArgs> PickupDestroyed;
        
        [MoonSharpHidden]
        public void OnPickupDestroyed(PickupDestroyedEventArgs ev)
        {
            PickupDestroyed?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)] public event EventHandler<SpawningTeamVehicleEventArgs> SpawningTeamVehicle;
        
        [MoonSharpHidden]
        public void OnSpawningTeamVehicle(SpawningTeamVehicleEventArgs ev)
        {
            SpawningTeamVehicle?.Invoke(this, ev);
        }
        
        // /////////////////////////////
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Map.PlacingBulletHole += OnPlacingBulletHole;
            Exiled.Events.Handlers.Map.PlacingBlood += OnPlacingBlood;
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += OnAnnouncingDecontamination;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination += OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnAnnouncingNtfEntrance;
            Exiled.Events.Handlers.Map.GeneratorActivating += OnGeneratorActivating;
            Exiled.Events.Handlers.Map.Decontaminating += OnDecontaminating;
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
            Exiled.Events.Handlers.Map.SpawningItem += OnSpawningItem;
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Map.ChangingIntoGrenade += OnChangingIntoGrenade;
            Exiled.Events.Handlers.Map.ChangedIntoGrenade += OnChangedIntoGrenade;
            Exiled.Events.Handlers.Map.TurningOffLights += OnTurningOffLights;
            Exiled.Events.Handlers.Map.PickupAdded += OnPickupAdded;
            Exiled.Events.Handlers.Map.PickupDestroyed += OnPickupDestroyed;
            Exiled.Events.Handlers.Map.SpawningTeamVehicle += OnSpawningTeamVehicle;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<PlacingBulletHoleEventArgs>();
            UserData.RegisterType<PlacingBloodEventArgs>();
            UserData.RegisterType<AnnouncingDecontaminationEventArgs>();
            UserData.RegisterType<AnnouncingScpTerminationEventArgs>();
            UserData.RegisterType<AnnouncingNtfEntranceEventArgs>();
            UserData.RegisterType<GeneratorActivatingEventArgs>();
            UserData.RegisterType<DecontaminatingEventArgs>();
            UserData.RegisterType<ExplodingGrenadeEventArgs>();
            UserData.RegisterType<SpawningItemEventArgs>();
            UserData.RegisterType<FillingLockerEventArgs>();
            UserData.RegisterType<ChangingIntoGrenadeEventArgs>();
            UserData.RegisterType<ChangedIntoGrenadeEventArgs>();
            UserData.RegisterType<TurningOffLightsEventArgs>();
            UserData.RegisterType<PickupAddedEventArgs>();
            UserData.RegisterType<PickupDestroyedEventArgs>();
            UserData.RegisterType<SpawningTeamVehicleEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Map.PlacingBulletHole -= OnPlacingBulletHole;
            Exiled.Events.Handlers.Map.PlacingBlood -= OnPlacingBlood;
            Exiled.Events.Handlers.Map.AnnouncingDecontamination -= OnAnnouncingDecontamination;
            Exiled.Events.Handlers.Map.AnnouncingScpTermination -= OnAnnouncingScpTermination;
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnAnnouncingNtfEntrance;
            Exiled.Events.Handlers.Map.GeneratorActivating -= OnGeneratorActivating;
            Exiled.Events.Handlers.Map.Decontaminating -= OnDecontaminating;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnExplodingGrenade;
            Exiled.Events.Handlers.Map.SpawningItem -= OnSpawningItem;
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Map.ChangingIntoGrenade -= OnChangingIntoGrenade;
            Exiled.Events.Handlers.Map.ChangedIntoGrenade -= OnChangedIntoGrenade;
            Exiled.Events.Handlers.Map.TurningOffLights -= OnTurningOffLights;
            Exiled.Events.Handlers.Map.PickupAdded -= OnPickupAdded;
            Exiled.Events.Handlers.Map.PickupDestroyed -= OnPickupDestroyed;
            Exiled.Events.Handlers.Map.SpawningTeamVehicle -= OnSpawningTeamVehicle;
        }
    }
}