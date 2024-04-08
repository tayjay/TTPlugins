using System;
using Exiled.Events.EventArgs.Player;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class PlayerEvents
    {
        [MoonSharpVisible(true)]
        public event EventHandler<PreAuthenticatingEventArgs> PreAuthenticating;
        
        [MoonSharpHidden]
        public void OnPreAuthenticating(PreAuthenticatingEventArgs ev)
        {
            PreAuthenticating?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ReservedSlotsCheckEventArgs> ReservedSlotsCheck;
        
        [MoonSharpHidden]
        public void OnReservedSlotsCheck(ReservedSlotsCheckEventArgs ev)
        {
            ReservedSlotsCheck?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<KickingEventArgs> Kicking;
        
        [MoonSharpHidden]
        public void OnKicking(KickingEventArgs ev)
        {
            Kicking?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<KickedEventArgs> Kicked;

        [MoonSharpHidden]
        public void OnKicked(KickedEventArgs ev)
        {
            Kicked?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<BanningEventArgs> Banning;
        
        [MoonSharpHidden]
        public void OnBanning(BanningEventArgs ev)
        {
            Banning?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<BannedEventArgs> Banned;
        
        [MoonSharpHidden]
        public void OnBanned(BannedEventArgs ev)
        {
            Banned?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EarningAchievementEventArgs> EarningAchievement;
        
        [MoonSharpHidden]
        public void OnEarningAchievement(EarningAchievementEventArgs ev)
        {
            EarningAchievement?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsingItemEventArgs> UsingItem;
        
        [MoonSharpHidden]
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            UsingItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsingItemCompletedEventArgs> UsingItemCompleted;
        
        [MoonSharpHidden]
        public void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
        {
            UsingItemCompleted?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsedItemEventArgs> UsedItem;
        
        [MoonSharpHidden]
        public void OnUsedItem(UsedItemEventArgs ev)
        {
            UsedItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<CancellingItemUseEventArgs> CancellingItemUse;
        
        [MoonSharpHidden]
        public void OnCancellingItemUse(CancellingItemUseEventArgs ev)
        {
            CancellingItemUse?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<CancelledItemUseEventArgs> CancelledItemUse;
        
        [MoonSharpHidden]
        public void OnCancelledItemUse(CancelledItemUseEventArgs ev)
        {
            CancelledItemUse?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractedEventArgs> Interacted;
        
        [MoonSharpHidden]
        public void OnInteracted(InteractedEventArgs ev)
        {
            Interacted?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SpawnedRagdollEventArgs> SpawnedRagdoll;
        
        [MoonSharpHidden]
        public void OnSpawnedRagdoll(SpawnedRagdollEventArgs ev)
        {
            SpawnedRagdoll?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ActivatingWarheadPanelEventArgs> ActivatingWarheadPanel;
        
        [MoonSharpHidden]
        public void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            ActivatingWarheadPanel?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ActivatingWorkstationEventArgs> ActivatingWorkstation;
        
        [MoonSharpHidden]
        public void OnActivatingWorkstation(ActivatingWorkstationEventArgs ev)
        {
            ActivatingWorkstation?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DeactivatingWorkstationEventArgs> DeactivatingWorkstation;
        
        [MoonSharpHidden]
        public void OnDeactivatingWorkstation(DeactivatingWorkstationEventArgs ev)
        {
            DeactivatingWorkstation?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<JoinedEventArgs> Joined;
        
        [MoonSharpHidden]
        public void OnJoined(JoinedEventArgs ev)
        {
            Joined?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<VerifiedEventArgs> Verified;
        
        [MoonSharpHidden]
        public void OnVerified(VerifiedEventArgs ev)
        {
            Verified?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<LeftEventArgs> Left;
        
        [MoonSharpHidden]
        public void OnLeft(LeftEventArgs ev)
        {
            Left?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DestroyingEventArgs> Destroying;
        
        [MoonSharpHidden]
        public void OnDestroying(DestroyingEventArgs ev)
        {
            Destroying?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<HurtingEventArgs> Hurting;
        
        [MoonSharpHidden]
        public void OnHurting(HurtingEventArgs ev)
        {
            Hurting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<HurtEventArgs> Hurt;
        
        [MoonSharpHidden]
        public void OnHurt(HurtEventArgs ev)
        {
            Hurt?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DyingEventArgs> Dying;
        
        [MoonSharpHidden]
        public void OnDying(DyingEventArgs ev)
        {
            Dying?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DiedEventArgs> Died;
        
        [MoonSharpHidden]
        public void OnDied(DiedEventArgs ev)
        {
            Died?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingRoleEventArgs> ChangingRole;
        
        [MoonSharpHidden]
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            ChangingRole?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ThrownProjectileEventArgs> ThrownProjectile;
        
        [MoonSharpHidden]
        public void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            ThrownProjectile?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ThrowingRequestEventArgs> ThrowingRequest;
        
        [MoonSharpHidden]
        public void OnThrowingRequest(ThrowingRequestEventArgs ev)
        {
            ThrowingRequest?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DroppingItemEventArgs> DroppingItem;
        
        [MoonSharpHidden]
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            DroppingItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DroppedItemEventArgs> DroppedItem;
        
        [MoonSharpHidden]
        public void OnDroppedItem(DroppedItemEventArgs ev)
        {
            DroppedItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DroppingNothingEventArgs> DroppingNothing;
        
        [MoonSharpHidden]
        public void OnDroppingNothing(DroppingNothingEventArgs ev)
        {
            DroppingNothing?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<PickingUpItemEventArgs> PickingUpItem;
        
        [MoonSharpHidden]
        public void OnPickingUpItem(PickingUpItemEventArgs ev)
        {
            PickingUpItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<HandcuffingEventArgs> Handcuffing;
        
        [MoonSharpHidden]
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            Handcuffing?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RemovingHandcuffsEventArgs> RemovingHandcuffs;
        
        [MoonSharpHidden]
        public void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
        {
            RemovingHandcuffs?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<IntercomSpeakingEventArgs> IntercomSpeaking;
        
        [MoonSharpHidden]
        public void OnIntercomSpeaking(IntercomSpeakingEventArgs ev)
        {
            IntercomSpeaking?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ShotEventArgs> Shot;
        
        [MoonSharpHidden]
        public void OnShot(ShotEventArgs ev)
        {
            Shot?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ShootingEventArgs> Shooting;
        
        [MoonSharpHidden]
        public void OnShooting(ShootingEventArgs ev)
        {
            Shooting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EnteringPocketDimensionEventArgs> EnteringPocketDimension;
        
        [MoonSharpHidden]
        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            EnteringPocketDimension?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EscapingPocketDimensionEventArgs> EscapingPocketDimension;
        
        [MoonSharpHidden]
        public void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            EscapingPocketDimension?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<FailingEscapePocketDimensionEventArgs> FailingEscapePocketDimension;
        
        [MoonSharpHidden]
        public void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
        {
            FailingEscapePocketDimension?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EnteringKillerCollisionEventArgs> EnteringKillerCollision;
        
        [MoonSharpHidden]
        public void OnEnteringKillerCollision(EnteringKillerCollisionEventArgs ev)
        {
            EnteringKillerCollision?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ReloadingWeaponEventArgs> ReloadingWeapon;
        
        [MoonSharpHidden]
        public void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            ReloadingWeapon?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SpawningEventArgs> Spawning;
        
        [MoonSharpHidden]
        public void OnSpawning(SpawningEventArgs ev)
        {
            Spawning?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SpawnedEventArgs> Spawned;
        
        [MoonSharpHidden]
        public void OnSpawned(SpawnedEventArgs ev)
        {
            Spawned?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangedItemEventArgs> ChangedItem;
        
        [MoonSharpHidden]
        public void OnChangedItem(ChangedItemEventArgs ev)
        {
            ChangedItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingItemEventArgs> ChangingItem;
        
        [MoonSharpHidden]
        public void OnChangingItem(ChangingItemEventArgs ev)
        {
            ChangingItem?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingGroupEventArgs> ChangingGroup;
        
        [MoonSharpHidden]
        public void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            ChangingGroup?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractingDoorEventArgs> InteractingDoor;
        
        [MoonSharpHidden]
        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            InteractingDoor?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractingElevatorEventArgs> InteractingElevator;
        
        [MoonSharpHidden]
        public void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            InteractingElevator?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractingLockerEventArgs> InteractingLocker;
        
        [MoonSharpHidden]
        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            InteractingLocker?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TriggeringTeslaEventArgs> TriggeringTesla;
        
        [MoonSharpHidden]
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            TriggeringTesla?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UnlockingGeneratorEventArgs> UnlockingGenerator;
        
        [MoonSharpHidden]
        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            UnlockingGenerator?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<OpeningGeneratorEventArgs> OpeningGenerator;
        
        [MoonSharpHidden]
        public void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            OpeningGenerator?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ClosingGeneratorEventArgs> ClosingGenerator;
        
        [MoonSharpHidden]
        public void OnClosingGenerator(ClosingGeneratorEventArgs ev)
        {
            ClosingGenerator?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ActivatingGeneratorEventArgs> ActivatingGenerator;
        
        [MoonSharpHidden]
        public void OnActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            ActivatingGenerator?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<StoppingGeneratorEventArgs> StoppingGenerator;
        
        [MoonSharpHidden]
        public void OnStoppingGenerator(StoppingGeneratorEventArgs ev)
        {
            StoppingGenerator?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ReceivingEffectEventArgs> ReceivingEffect;
        
        [MoonSharpHidden]
        public void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            ReceivingEffect?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<IssuingMuteEventArgs> IssuingMute;
        
        [MoonSharpHidden]
        public void OnIssuingMute(IssuingMuteEventArgs ev)
        {
            IssuingMute?.Invoke(null, ev);
        }

        [MoonSharpVisible(true)]
        public event EventHandler<RevokingMuteEventArgs> RevokingMute;
        
        [MoonSharpHidden]
        public void OnRevokingMute(RevokingMuteEventArgs ev)
        {
            RevokingMute?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsingRadioBatteryEventArgs> UsingRadioBattery;
        
        [MoonSharpHidden]
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev)
        {
            UsingRadioBattery?.Invoke(null, ev);
        }

        [MoonSharpVisible(true)]
        public event EventHandler<ChangingRadioPresetEventArgs> ChangingRadioPreset;
        
        [MoonSharpHidden]
        public void OnChangingRadioPreset(ChangingRadioPresetEventArgs ev)
        {
            ChangingRadioPreset?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UsingMicroHIDEnergyEventArgs> UsingMicroHIDEnergy;
        
        [MoonSharpHidden]
        public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev)
        {
            UsingMicroHIDEnergy?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DroppingAmmoEventArgs> DroppingAmmo;
        
        [MoonSharpHidden]
        public void OnDroppingAmmo(DroppingAmmoEventArgs ev)
        {
            DroppingAmmo?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DroppedAmmoEventArgs> DroppedAmmo;
        
        [MoonSharpHidden]
        public void OnDroppedAmmo(DroppedAmmoEventArgs ev)
        {
            DroppedAmmo?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<InteractingShootingTargetEventArgs> InteractingShootingTarget;
        
        [MoonSharpHidden]
        public void OnInteractingShootingTarget(InteractingShootingTargetEventArgs ev)
        {
            InteractingShootingTarget?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DamagingShootingTargetEventArgs> DamagingShootingTarget;
        
        [MoonSharpHidden]
        public void OnDamagingShootingTarget(DamagingShootingTargetEventArgs ev)
        {
            DamagingShootingTarget?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<FlippingCoinEventArgs> FlippingCoin;
        
        [MoonSharpHidden]
        public void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            FlippingCoin?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TogglingFlashlightEventArgs> TogglingFlashlight;
        
        [MoonSharpHidden]
        public void OnTogglingFlashlight(TogglingFlashlightEventArgs ev)
        {
            TogglingFlashlight?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<UnloadingWeaponEventArgs> UnloadingWeapon;
        
        [MoonSharpHidden]
        public void OnUnloadingWeapon(UnloadingWeaponEventArgs ev)
        {
            UnloadingWeapon?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<AimingDownSightEventArgs> AimingDownSight;
        
        [MoonSharpHidden]
        public void OnAimingDownSight(AimingDownSightEventArgs ev)
        {
            AimingDownSight?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TogglingWeaponFlashlightEventArgs> TogglingWeaponFlashlight;
        
        [MoonSharpHidden]
        public void OnTogglingWeaponFlashlight(TogglingWeaponFlashlightEventArgs ev)
        {
            TogglingWeaponFlashlight?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DryfiringWeaponEventArgs> DryfiringWeapon;
        
        [MoonSharpHidden]
        public void OnDryfiringWeapon(DryfiringWeaponEventArgs ev)
        {
            DryfiringWeapon?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<VoiceChattingEventArgs> VoiceChatting;
        
        [MoonSharpHidden]
        public void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            VoiceChatting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<MakingNoiseEventArgs> MakingNoise;
        
        [MoonSharpHidden]
        public void OnMakingNoise(MakingNoiseEventArgs ev)
        {
            MakingNoise?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<JumpingEventArgs> Jumping;
        
        [MoonSharpHidden]
        public void OnJumping(JumpingEventArgs ev)
        {
            Jumping?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<LandingEventArgs> Landing;
        
        [MoonSharpHidden]
        public void OnLanding(LandingEventArgs ev)
        {
            Landing?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TransmittingEventArgs> Transmitting;
        
        [MoonSharpHidden]
        public void OnTransmitting(TransmittingEventArgs ev)
        {
            Transmitting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingMoveStateEventArgs> ChangingMoveState;
        
        [MoonSharpHidden]
        public void OnChangingMoveState(ChangingMoveStateEventArgs ev)
        {
            ChangingMoveState?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingSpectatedPlayerEventArgs> ChangingSpectatedPlayer;
        
        [MoonSharpHidden]
        public void OnChangingSpectatedPlayer(ChangingSpectatedPlayerEventArgs ev)
        {
            ChangingSpectatedPlayer?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TogglingNoClipEventArgs> TogglingNoClip;
        
        [MoonSharpHidden]
        public void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            TogglingNoClip?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TogglingOverwatchEventArgs> TogglingOverwatch;
        
        [MoonSharpHidden]
        public void OnTogglingOverwatch(TogglingOverwatchEventArgs ev)
        {
            TogglingOverwatch?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<TogglingRadioEventArgs> TogglingRadio;
        
        [MoonSharpHidden]
        public void OnTogglingRadio(TogglingRadioEventArgs ev)
        {
            TogglingRadio?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SearchingPickupEventArgs> SearchingPickup;
        
        [MoonSharpHidden]
        public void OnSearchingPickup(SearchingPickupEventArgs ev)
        {
            SearchingPickup?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<SendingAdminChatMessageEventsArgs> SendingAdminChatMessageEventsArgs;
        
        [MoonSharpHidden]
        public void OnSendingAdminChatMessageEventsArgs(SendingAdminChatMessageEventsArgs ev)
        {
            SendingAdminChatMessageEventsArgs?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DamagingWindowEventArgs> DamageWindow;
        
        [MoonSharpHidden]
        public void OnPlayerDamageWindow(DamagingWindowEventArgs ev)
        {
            DamageWindow?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<DamagingDoorEventArgs> DamagingDoor;
        
        [MoonSharpHidden]
        public void OnDamagingDoor(DamagingDoorEventArgs ev)
        {
            DamagingDoor?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ItemAddedEventArgs> ItemAdded;
        
        [MoonSharpHidden]
        public void OnItemAdded(ItemAddedEventArgs ev)
        {
            ItemAdded?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ItemRemovedEventArgs> ItemRemoved;
        
        [MoonSharpHidden]
        public void OnItemRemoved(ItemRemovedEventArgs ev)
        {
            ItemRemoved?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<KillingPlayerEventArgs> KillingPlayer;
        
        [MoonSharpHidden]
        public void OnKillingPlayer(KillingPlayerEventArgs ev)
        {
            KillingPlayer?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EnteringEnvironmentalHazardEventArgs> EnteringEnvironmentalHazard;
        
        [MoonSharpHidden]
        public void OnEnteringEnvironmentalHazard(EnteringEnvironmentalHazardEventArgs ev)
        {
            EnteringEnvironmentalHazard?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<StayingOnEnvironmentalHazardEventArgs> StayingOnEnvironmentalHazard;
        
        [MoonSharpHidden]
        public void OnStayingOnEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
        {
            StayingOnEnvironmentalHazard?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ExitingEnvironmentalHazardEventArgs> ExitingEnvironmentalHazard;
        
        [MoonSharpHidden]
        public void OnExitingEnvironmentalHazard(ExitingEnvironmentalHazardEventArgs ev)
        {
            ExitingEnvironmentalHazard?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChangingNicknameEventArgs> ChangingNickname;
        
        [MoonSharpHidden]
        public void OnChangingNickname(ChangingNicknameEventArgs ev)
        {
            ChangingNickname?.Invoke(null, ev);
        }

        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.PreAuthenticating += OnPreAuthenticating;
            Exiled.Events.Handlers.Player.ReservedSlot += OnReservedSlotsCheck;
            Exiled.Events.Handlers.Player.Kicking += OnKicking;
            Exiled.Events.Handlers.Player.Kicked += OnKicked;
            Exiled.Events.Handlers.Player.Banning += OnBanning;
            Exiled.Events.Handlers.Player.Banned += OnBanned;
            Exiled.Events.Handlers.Player.EarningAchievement += OnEarningAchievement;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsingItemCompleted;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            Exiled.Events.Handlers.Player.CancellingItemUse += OnCancellingItemUse;
            Exiled.Events.Handlers.Player.CancelledItemUse += OnCancelledItemUse;
            Exiled.Events.Handlers.Player.Interacted += OnInteracted;
            Exiled.Events.Handlers.Player.SpawnedRagdoll += OnSpawnedRagdoll;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.ActivatingWorkstation += OnActivatingWorkstation;
            Exiled.Events.Handlers.Player.DeactivatingWorkstation += OnDeactivatingWorkstation;
            Exiled.Events.Handlers.Player.Joined += OnJoined;
            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Destroying += OnDestroying;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Hurt += OnHurt;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
            Exiled.Events.Handlers.Player.ThrowingRequest += OnThrowingRequest;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
            Exiled.Events.Handlers.Player.DroppingNothing += OnDroppingNothing;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
            Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
            Exiled.Events.Handlers.Player.RemovingHandcuffs += OnRemovingHandcuffs;
            Exiled.Events.Handlers.Player.IntercomSpeaking += OnIntercomSpeaking;
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += OnEscapingPocketDimension;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += OnFailingEscapePocketDimension;
            Exiled.Events.Handlers.Player.EnteringKillerCollision += OnEnteringKillerCollision;
            Exiled.Events.Handlers.Player.ReloadingWeapon += OnReloadingWeapon;
            Exiled.Events.Handlers.Player.Spawning += OnSpawning;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Player.ChangedItem += OnChangedItem;
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
            Exiled.Events.Handlers.Player.ChangingGroup += OnChangingGroup;
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
            Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.OpeningGenerator += OnOpeningGenerator;
            Exiled.Events.Handlers.Player.ClosingGenerator += OnClosingGenerator;
            Exiled.Events.Handlers.Player.ActivatingGenerator += OnActivatingGenerator;
            Exiled.Events.Handlers.Player.StoppingGenerator += OnStoppingGenerator;
            Exiled.Events.Handlers.Player.ReceivingEffect += OnReceivingEffect;
            Exiled.Events.Handlers.Player.IssuingMute += OnIssuingMute;
            Exiled.Events.Handlers.Player.RevokingMute += OnRevokingMute;
            Exiled.Events.Handlers.Player.UsingRadioBattery += OnUsingRadioBattery;
            Exiled.Events.Handlers.Player.ChangingRadioPreset += OnChangingRadioPreset;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += OnUsingMicroHIDEnergy;
            Exiled.Events.Handlers.Player.DroppingAmmo += OnDroppingAmmo;
            Exiled.Events.Handlers.Player.DroppedAmmo += OnDroppedAmmo;
            Exiled.Events.Handlers.Player.InteractingShootingTarget += OnInteractingShootingTarget;
            Exiled.Events.Handlers.Player.DamagingShootingTarget += OnDamagingShootingTarget;
            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            Exiled.Events.Handlers.Player.TogglingFlashlight += OnTogglingFlashlight;
            Exiled.Events.Handlers.Player.UnloadingWeapon += OnUnloadingWeapon;
            Exiled.Events.Handlers.Player.AimingDownSight += OnAimingDownSight;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight += OnTogglingWeaponFlashlight;
            Exiled.Events.Handlers.Player.DryfiringWeapon += OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Player.MakingNoise += OnMakingNoise;
            Exiled.Events.Handlers.Player.Jumping += OnJumping;
            Exiled.Events.Handlers.Player.Landing += OnLanding;
            Exiled.Events.Handlers.Player.Transmitting += OnTransmitting;
            Exiled.Events.Handlers.Player.ChangingMoveState += OnChangingMoveState;
            Exiled.Events.Handlers.Player.ChangingSpectatedPlayer += OnChangingSpectatedPlayer;
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
            Exiled.Events.Handlers.Player.TogglingOverwatch += OnTogglingOverwatch;
            Exiled.Events.Handlers.Player.TogglingRadio += OnTogglingRadio;
            Exiled.Events.Handlers.Player.SearchingPickup += OnSearchingPickup;
            Exiled.Events.Handlers.Player.SendingAdminChatMessage += OnSendingAdminChatMessageEventsArgs;
            Exiled.Events.Handlers.Player.PlayerDamageWindow += OnPlayerDamageWindow;
            Exiled.Events.Handlers.Player.DamagingDoor += OnDamagingDoor;
            Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
            Exiled.Events.Handlers.Player.ItemRemoved += OnItemRemoved;
            Exiled.Events.Handlers.Player.KillingPlayer += OnKillingPlayer;
            Exiled.Events.Handlers.Player.EnteringEnvironmentalHazard += OnEnteringEnvironmentalHazard;
            Exiled.Events.Handlers.Player.StayingOnEnvironmentalHazard += OnStayingOnEnvironmentalHazard;
            Exiled.Events.Handlers.Player.ExitingEnvironmentalHazard += OnExitingEnvironmentalHazard;
            Exiled.Events.Handlers.Player.ChangingNickname += OnChangingNickname;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<PreAuthenticatingEventArgs>();
            UserData.RegisterType<ReservedSlotsCheckEventArgs>();
            UserData.RegisterType<KickingEventArgs>();
            UserData.RegisterType<KickedEventArgs>();
            UserData.RegisterType<BanningEventArgs>();
            UserData.RegisterType<BannedEventArgs>();
            UserData.RegisterType<EarningAchievementEventArgs>();
            UserData.RegisterType<UsingItemEventArgs>();
            UserData.RegisterType<UsingItemCompletedEventArgs>();
            UserData.RegisterType<UsedItemEventArgs>();
            UserData.RegisterType<CancellingItemUseEventArgs>();
            UserData.RegisterType<CancelledItemUseEventArgs>();
            UserData.RegisterType<InteractedEventArgs>();
            UserData.RegisterType<SpawnedRagdollEventArgs>();
            UserData.RegisterType<ActivatingWarheadPanelEventArgs>();
            UserData.RegisterType<ActivatingWorkstationEventArgs>();
            UserData.RegisterType<DeactivatingWorkstationEventArgs>();
            UserData.RegisterType<JoinedEventArgs>();
            UserData.RegisterType<VerifiedEventArgs>();
            UserData.RegisterType<LeftEventArgs>();
            UserData.RegisterType<DestroyingEventArgs>();
            UserData.RegisterType<HurtingEventArgs>();
            UserData.RegisterType<HurtEventArgs>();
            UserData.RegisterType<DyingEventArgs>();
            UserData.RegisterType<DiedEventArgs>();
            UserData.RegisterType<ChangingRoleEventArgs>();
            UserData.RegisterType<ThrownProjectileEventArgs>();
            UserData.RegisterType<ThrowingRequestEventArgs>();
            UserData.RegisterType<DroppingItemEventArgs>();
            UserData.RegisterType<DroppedItemEventArgs>();
            UserData.RegisterType<DroppingNothingEventArgs>();
            UserData.RegisterType<PickingUpItemEventArgs>();
            UserData.RegisterType<HandcuffingEventArgs>();
            UserData.RegisterType<RemovingHandcuffsEventArgs>();
            UserData.RegisterType<IntercomSpeakingEventArgs>();
            UserData.RegisterType<ShotEventArgs>();
            UserData.RegisterType<ShootingEventArgs>();
            UserData.RegisterType<EnteringPocketDimensionEventArgs>();
            UserData.RegisterType<EscapingPocketDimensionEventArgs>();
            UserData.RegisterType<FailingEscapePocketDimensionEventArgs>();
            UserData.RegisterType<EnteringKillerCollisionEventArgs>();
            UserData.RegisterType<ReloadingWeaponEventArgs>();
            UserData.RegisterType<SpawningEventArgs>();
            UserData.RegisterType<SpawnedEventArgs>();
            UserData.RegisterType<ChangedItemEventArgs>();
            UserData.RegisterType<ChangingItemEventArgs>();
            UserData.RegisterType<ChangingGroupEventArgs>();
            UserData.RegisterType<InteractingDoorEventArgs>();
            UserData.RegisterType<InteractingElevatorEventArgs>();
            UserData.RegisterType<InteractingLockerEventArgs>();
            UserData.RegisterType<TriggeringTeslaEventArgs>();
            UserData.RegisterType<UnlockingGeneratorEventArgs>();
            UserData.RegisterType<OpeningGeneratorEventArgs>();
            UserData.RegisterType<ClosingGeneratorEventArgs>();
            UserData.RegisterType<ActivatingGeneratorEventArgs>();
            UserData.RegisterType<StoppingGeneratorEventArgs>();
            UserData.RegisterType<ReceivingEffectEventArgs>();
            UserData.RegisterType<IssuingMuteEventArgs>();
            UserData.RegisterType<RevokingMuteEventArgs>();
            UserData.RegisterType<UsingRadioBatteryEventArgs>();
            UserData.RegisterType<ChangingRadioPresetEventArgs>();
            UserData.RegisterType<UsingMicroHIDEnergyEventArgs>();
            UserData.RegisterType<DroppingAmmoEventArgs>();
            UserData.RegisterType<DroppedAmmoEventArgs>();
            UserData.RegisterType<InteractingShootingTargetEventArgs>();
            UserData.RegisterType<DamagingShootingTargetEventArgs>();
            UserData.RegisterType<FlippingCoinEventArgs>();
            UserData.RegisterType<TogglingFlashlightEventArgs>();
            UserData.RegisterType<UnloadingWeaponEventArgs>();
            UserData.RegisterType<AimingDownSightEventArgs>();
            UserData.RegisterType<TogglingWeaponFlashlightEventArgs>();
            UserData.RegisterType<DryfiringWeaponEventArgs>();
            UserData.RegisterType<VoiceChattingEventArgs>();
            UserData.RegisterType<MakingNoiseEventArgs>();
            UserData.RegisterType<JumpingEventArgs>();
            UserData.RegisterType<LandingEventArgs>();
            UserData.RegisterType<TransmittingEventArgs>();
            UserData.RegisterType<ChangingMoveStateEventArgs>();
            UserData.RegisterType<ChangingSpectatedPlayerEventArgs>();
            UserData.RegisterType<TogglingNoClipEventArgs>();
            UserData.RegisterType<TogglingOverwatchEventArgs>();
            UserData.RegisterType<TogglingRadioEventArgs>();
            UserData.RegisterType<SearchingPickupEventArgs>();
            UserData.RegisterType<SendingAdminChatMessageEventsArgs>();
            UserData.RegisterType<DamagingWindowEventArgs>();
            UserData.RegisterType<DamagingDoorEventArgs>();
            UserData.RegisterType<ItemAddedEventArgs>();
            UserData.RegisterType<ItemRemovedEventArgs>();
            UserData.RegisterType<KillingPlayerEventArgs>();
            UserData.RegisterType<EnteringEnvironmentalHazardEventArgs>();
            UserData.RegisterType<StayingOnEnvironmentalHazardEventArgs>();
            UserData.RegisterType<ExitingEnvironmentalHazardEventArgs>();
            UserData.RegisterType<ChangingNicknameEventArgs>();
            
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.PreAuthenticating -= OnPreAuthenticating;
            Exiled.Events.Handlers.Player.ReservedSlot -= OnReservedSlotsCheck;
            Exiled.Events.Handlers.Player.Kicking -= OnKicking;
            Exiled.Events.Handlers.Player.Kicked -= OnKicked;
            Exiled.Events.Handlers.Player.Banning -= OnBanning;
            Exiled.Events.Handlers.Player.Banned -= OnBanned;
            Exiled.Events.Handlers.Player.EarningAchievement -= OnEarningAchievement;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsingItemCompleted;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.CancellingItemUse -= OnCancellingItemUse;
            Exiled.Events.Handlers.Player.CancelledItemUse -= OnCancelledItemUse;
            Exiled.Events.Handlers.Player.Interacted -= OnInteracted;
            Exiled.Events.Handlers.Player.SpawnedRagdoll -= OnSpawnedRagdoll;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.ActivatingWorkstation -= OnActivatingWorkstation;
            Exiled.Events.Handlers.Player.DeactivatingWorkstation -= OnDeactivatingWorkstation;
            Exiled.Events.Handlers.Player.Joined -= OnJoined;
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Destroying -= OnDestroying;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Hurt -= OnHurt;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
            Exiled.Events.Handlers.Player.ThrowingRequest -= OnThrowingRequest;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
            Exiled.Events.Handlers.Player.DroppingNothing -= OnDroppingNothing;
            Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
            Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= OnRemovingHandcuffs;
            Exiled.Events.Handlers.Player.IntercomSpeaking -= OnIntercomSpeaking;
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= OnEscapingPocketDimension;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= OnFailingEscapePocketDimension;
            Exiled.Events.Handlers.Player.EnteringKillerCollision -= OnEnteringKillerCollision;
            Exiled.Events.Handlers.Player.ReloadingWeapon -= OnReloadingWeapon;
            Exiled.Events.Handlers.Player.Spawning -= OnSpawning;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Player.ChangedItem -= OnChangedItem;
            Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
            Exiled.Events.Handlers.Player.ChangingGroup -= OnChangingGroup;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.OpeningGenerator -= OnOpeningGenerator;
            Exiled.Events.Handlers.Player.ClosingGenerator -= OnClosingGenerator;
            Exiled.Events.Handlers.Player.ActivatingGenerator -= OnActivatingGenerator;
            Exiled.Events.Handlers.Player.StoppingGenerator -= OnStoppingGenerator;
            Exiled.Events.Handlers.Player.ReceivingEffect -= OnReceivingEffect;
            Exiled.Events.Handlers.Player.IssuingMute -= OnIssuingMute;
            Exiled.Events.Handlers.Player.RevokingMute -= OnRevokingMute;
            Exiled.Events.Handlers.Player.UsingRadioBattery -= OnUsingRadioBattery;
            Exiled.Events.Handlers.Player.ChangingRadioPreset -= OnChangingRadioPreset;
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= OnUsingMicroHIDEnergy;
            Exiled.Events.Handlers.Player.DroppingAmmo -= OnDroppingAmmo;
            Exiled.Events.Handlers.Player.DroppedAmmo -= OnDroppedAmmo;
            Exiled.Events.Handlers.Player.InteractingShootingTarget -= OnInteractingShootingTarget;
            Exiled.Events.Handlers.Player.DamagingShootingTarget -= OnDamagingShootingTarget;
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
            Exiled.Events.Handlers.Player.TogglingFlashlight -= OnTogglingFlashlight;
            Exiled.Events.Handlers.Player.UnloadingWeapon -= OnUnloadingWeapon;
            Exiled.Events.Handlers.Player.AimingDownSight -= OnAimingDownSight;
            Exiled.Events.Handlers.Player.TogglingWeaponFlashlight -= OnTogglingWeaponFlashlight;
            Exiled.Events.Handlers.Player.DryfiringWeapon -= OnDryfiringWeapon;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            Exiled.Events.Handlers.Player.MakingNoise -= OnMakingNoise;
            Exiled.Events.Handlers.Player.Jumping -= OnJumping;
            Exiled.Events.Handlers.Player.Landing -= OnLanding;
            Exiled.Events.Handlers.Player.Transmitting -= OnTransmitting;
            Exiled.Events.Handlers.Player.ChangingMoveState -= OnChangingMoveState;
            Exiled.Events.Handlers.Player.ChangingSpectatedPlayer -= OnChangingSpectatedPlayer;
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;
            Exiled.Events.Handlers.Player.TogglingOverwatch -= OnTogglingOverwatch;
            Exiled.Events.Handlers.Player.TogglingRadio -= OnTogglingRadio;
            Exiled.Events.Handlers.Player.SearchingPickup -= OnSearchingPickup;
            Exiled.Events.Handlers.Player.SendingAdminChatMessage -= OnSendingAdminChatMessageEventsArgs;
            Exiled.Events.Handlers.Player.PlayerDamageWindow -= OnPlayerDamageWindow;
            Exiled.Events.Handlers.Player.DamagingDoor -= OnDamagingDoor;
            Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
            Exiled.Events.Handlers.Player.ItemRemoved -= OnItemRemoved;
            Exiled.Events.Handlers.Player.KillingPlayer -= OnKillingPlayer;
            Exiled.Events.Handlers.Player.EnteringEnvironmentalHazard -= OnEnteringEnvironmentalHazard;
            Exiled.Events.Handlers.Player.StayingOnEnvironmentalHazard -= OnStayingOnEnvironmentalHazard;
            Exiled.Events.Handlers.Player.ExitingEnvironmentalHazard -= OnExitingEnvironmentalHazard;
            Exiled.Events.Handlers.Player.ChangingNickname -= OnChangingNickname;
        }

    }
}