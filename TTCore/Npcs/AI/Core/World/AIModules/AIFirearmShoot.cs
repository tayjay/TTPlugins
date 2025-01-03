﻿using Exiled.API.Features.Items;
using InventorySystem;
using InventorySystem.Items.Firearms.BasicMessages;
using TTCore.Npcs.AI.Core.World.Targetables;
using UnityEngine;
using Utils.Networking;

namespace TTCore.Npcs.AI.Core.World.AIModules;

/*public class AIFirearmShoot : AIModuleBase
    {
        public override float Duration => 4f;

        public TargetableBase Target
        {
            get => Parent.EnemyTarget;
            set => Parent.EnemyTarget = value;
        }

        public bool HasLOS(out Vector3 pos, out bool hasCollider) => Parent.HasLOSOnEnemy(out pos, out hasCollider, Headshots);
        public bool HasTarget => Parent.HasEnemyTarget;

        public bool IsAiming
        {
            get
            {
                if (Parent.TryGetItem(out Firearm f))
                    return f.Base.AdsModule.ServerAds;
                return false;
            }
            set
            {
                if (Parent.TryGetItem(out Firearm f))
                {
                    if (value == f.Base.AdsModule.ServerAds)
                        return;

                    if (value)
                        new RequestMessage(f.Base.ItemSerial, RequestType.AdsIn).SendToAuthenticated();
                    else
                        new RequestMessage(f.Base.ItemSerial, RequestType.AdsOut).SendToAuthenticated();

                    f.Base.AdsModule.ServerAds = value;
                }
            }
        }

        public bool HasAmmo { get; protected set; }

        public bool Headshots;
        public bool InfiniteAmmo = true;

        public float ShootDotMinimum = 0.6f;
        public float HipfireRange = 7f;
        public float RandomAimRangeFar = 2f;
        public float RandomAimRangeFarDistance = 50f;
        public float RandomAimRangeClose = 0.4f;
        public float RandomAimRangeCloseDistance = 10f;
        public float RandomAimTimer = 0.5f;
        public float RandomAimChance = 0.95f;

        protected float Timer;

        protected FirearmState State;

        Vector3 randomAim;
        float randomAimTimer;

        public override bool Condition() => HasTarget && Parent.HasItemOfCategory(ItemCategory.Firearm) && HasLOS(out _, out bool hasCollider) && !hasCollider;

        public override void OnDisabled()
        {
            IsAiming = false;
        }

        public override void OnEnabled() { }

        public override void Init()
        {
            Tags = [AIBehaviorBase.AttackerTag];
            Headshots = Random.Range(0, 2) == 0;
        }

        public override void Tick()
        {
            if (!Enabled)
                return;

            if (!HasTarget)
            {
                IsAiming = false;
                return;
            }

            if (randomAimTimer > 0f)
                randomAimTimer -= Time.fixedDeltaTime;
            else
            {
                randomAimTimer = RandomAimTimer;
                randomAim = (Random.Range(0f, 1f) < RandomAimChance) ? Random.insideUnitSphere * Mathf.Lerp(RandomAimRangeClose, RandomAimRangeFar, Mathf.InverseLerp(RandomAimRangeCloseDistance, RandomAimRangeFarDistance, Parent.GetDistance(Target))) : Vector3.zero;
            }

            bool hasLOS = HasLOS(out Vector3 pos, out bool hasCollider);

            if (hasLOS)
                Parent.MovementEngine.LookPos = pos + randomAim;

            if (Parent.TryGetItem(out Firearm f))
            {
                if (Timer > 0f)
                    Timer -= Time.fixedDeltaTime;
                else
                    State = FirearmState.Standby;

                HasAmmo = f.Base.Status.Ammo > 0;

                if (!HasAmmo)
                    StartReload(f);
                else if (hasLOS && !hasCollider && Parent.GetDotProduct(pos) >= ShootDotMinimum)
                    Shoot(f);

                if (!HasTarget && State == FirearmState.Standby)
                    IsAiming = false;
            }
            else
                Parent.EquipItem<Firearm>(f=>f != null);
        }

        public bool StartReload(Firearm f)
        {
            IsAiming = false;

            if (f == null || State != FirearmState.Standby)
                return false;

            HasAmmo = false;

            if (InfiniteAmmo && Parent.Inventory.GetCurAmmo(f.Base.AmmoType) < f.Base.AmmoManagerModule.MaxAmmo)
                Parent.Inventory.ServerAddAmmo(f.Base.AmmoType, f.Base.AmmoManagerModule.MaxAmmo);

            if (f.Base.AmmoManagerModule.ServerTryReload())
            {
                new RequestMessage(f.Base.ItemSerial, RequestType.Reload).SendToAuthenticated();
                return true;
            }

            return false;
        }

        public bool Shoot(Firearm f)
        {
            if (f.Base.Status.Ammo <= 0 || State != FirearmState.Standby || !f.Base.EquipperModule.Standby || !f.Base.ActionModule.Standby || !f.Base.AdsModule.Standby || !f.Base.AmmoManagerModule.Standby || !f.Base.HitregModule.Standby)
                return false;

            IsAiming = HasTarget && (Vector3.Distance(Target.GetPosition(Parent), Parent.CameraPosition) > HipfireRange);

            State = FirearmState.Shooting;
            Timer = 1f / f.Base.ActionModule.CyclicRate;

            if (f.Base.ActionModule.ServerAuthorizeShot() && f.Base.HitregModule.ClientCalculateHit(out ShotMessage shot))
            {
                f.Base.HitregModule.ServerProcessShot(shot);
                f.Base.UpdateAnims();
                return true;
            }

            return false;
        }

        public enum FirearmState
        {
            Standby,
            Shooting
        }
    }*/