﻿using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Disarming;
using InventorySystem.Items;
using InventorySystem.Items.Keycards;
using MapGeneration;
using PlayerRoles;
using PlayerStatsSystem;
using TTCore.Npcs.AI.Core.Management;
using TTCore.Npcs.AI.Core.World.AIModules;
using TTCore.Npcs.AI.Core.World.Targetables;
using TTCore.Utilities;
using UnityEngine;
using CheckpointDoor = Interactables.Interobjects.CheckpointDoor;
using Random = UnityEngine.Random;

namespace TTCore.Npcs.AI.Core.World;

public class AIModuleRunner : AIAddon
{
    public static int PlayerFollowWeight = 2;
    public static bool Frozen => false;//ServerVariableManager.TryGetVar(FrozenVar, out ServerVariable v) && bool.TryParse(v.Value, out bool b) && b;

        public const string FrozenVar = "freezenpcs";
        public const float RetargetTime = 0.25f;

        public float DotViewMinimum = 0.25f;
        public float FollowDistanceMax = 200f;
        public float ItemDistance = 30f;

        public readonly List<AIModuleBase> Modules = [];

        public TargetableBase EnemyTarget;
        public TargetableBase FollowTarget;

        public Room Room => Player.CurrentRoom;

        public AIMovementEngine MovementEngine => Core.MovementEngine;

        public Inventory Inventory => ReferenceHub.inventory;

        public Item CurrentItem => Player.CurrentItem;

        public PlayerRoleBase RoleBase => ReferenceHub.roleManager.CurrentRole;

        public Vector3 Position => ReferenceHub.transform.position;
        public Vector3 CameraPosition => ReferenceHub.PlayerCameraReference.position;

        public RoleTypeId Role => ReferenceHub.roleManager.CurrentRole.RoleTypeId;

        public HealthStat Health => GetStat<HealthStat>();
        public AhpStat ArtificialHealth => GetStat<AhpStat>();
        public StaminaStat Stamina => GetStat<StaminaStat>();

        public T GetStat<T>() where T : StatBase
        {
            if (!ReferenceHub.playerStats.TryGetModule(out T stat))
                return null;
            return stat;
        }

        public float RetargetTimer;
        public bool CanStealth = false;
        public bool CanWallhack = false;
        public bool DeleteOnDeath = false;

        protected float AimOffset;

        private void Start()
        {
            foreach (AIModuleBase module in Modules)
            {
                module.Parent = this;
                module.Init();
            }

            Core.OnDamage -= OnDamage;
            Core.OnRoleChange -= RoleChange;
            Core.OnDamage += OnDamage;
            Core.OnRoleChange += RoleChange;

            AimOffset = Random.Range(0.18f, 0.31f);
        }

        private void FixedUpdate()
        {
            if (Frozen)
                return;

            if (RetargetTimer > 0f)
                RetargetTimer -= Time.fixedDeltaTime;

            HasEnemyTarget = UpdateHasEnemyTarget();

            foreach (AIModuleBase module in Modules)
                module.Tick();
        }

        private void OnDestroy()
        {
            Core.OnDamage -= OnDamage;
            Core.OnRoleChange -= RoleChange;
        }

        public virtual void OnDamage(Player attacker)
        {
            if (RetargetTimer <= 0f && attacker != null)
            {
                EnemyTarget = attacker;
                RetargetTimer = RetargetTime;
            }
        }

        protected virtual void RoleChange(RoleTypeId role)
        {
            FollowTarget = null;
            EnemyTarget = null;
            RetargetTimer = 0f;

            OnRoleChange?.Invoke(role);

            if ((role == RoleTypeId.None || role == RoleTypeId.Spectator) && DeleteOnDeath)
                Core.Profile.Delete();
        }

        public T AddModule<T>() where T : AIModuleBase
        {
            T obj = Activator.CreateInstance<T>();
            Modules.Add(obj);
            obj.Parent = this;
            obj.Init();
            return obj;
        }

        public T GetModule<T>() where T : AIModuleBase
        {
            foreach (AIModuleBase obj in Modules)
                if (obj is T t)
                    return t;

            return null;
        }

        public bool TryGetModule<T>(out T output) where T : AIModuleBase
        {
            output = GetModule<T>();
            return output != null;
        }

        public void RemoveModule(AIModuleBase module)
        {
            Modules.Remove(module);
        }

        public void ChangeModule(bool status)
        {
            foreach (AIModuleBase mod in Modules)
                ChangeModule(mod, status);
        }

        public bool ChangeModule(int index, bool status)
        {
            if (index < Modules.Count && index >= 0)
                return ChangeModule(Modules[index], status);
            return false;
        }

        public bool ChangeModule(AIModuleBase module, bool status)
        {
            if (status && !module.Condition())
                return false;
            module.Enabled = status;
            return true;
        }

        public void ChangeModule(string tag, bool status)
        {
            foreach (AIModuleBase mod in Modules)
                if (mod.HasTag(tag))
                    ChangeModule(mod, status);
        }

        public AIModuleBase[] GetModulesByTag(string tag) => [.. Modules.FindAll((a) => a.HasTag(tag))];

        public AIModuleBase[] GetModulesByTagRandom(string tag) => GetModulesByTag(tag).OrderBy(_ => Random.Range(int.MinValue, int.MaxValue)).ToArray();

        public AIModuleBase GetRandomModuleByTag(string tag) => Modules.FindAll((a) => a.HasTag(tag)).RandomItem();

        public bool ActivateRandomModuleByTag(string tag, out AIModuleBase mod, bool single = false)
        {
            AIModuleBase[] mods = GetModulesByTagRandom(tag);
            if (single)
                ChangeModule(tag, false);

            foreach (AIModuleBase m in mods)
                if (ChangeModule(m, true))
                {
                    mod = m;
                    return true;
                }
            mod = null;
            return false;
        }

        public bool HasItemOfCategory(ItemCategory cat)
        {
            foreach (ItemBase item in Inventory.UserInventory.Items.Values)
                if (item.Category == cat)
                    return true;
            return false;
        }

        public bool CheckLOS(Vector3 pos, out bool hasCollider, bool canWallHack = false)
        {
            //todo: Change to nonalloc
            RaycastHit[] hits = Physics.RaycastAll(pos, (CameraPosition - pos).normalized, Vector3.Distance(CameraPosition, pos), AIPlayer.MapLayerMask, QueryTriggerInteraction.Ignore);

            if (hits.Length <= 0)
            {
                hasCollider = false;
                return true;
            }

            foreach (RaycastHit hit in hits)
            {
                if ((!hit.collider.TryGetComponentInParent(out DoorVariant door) && !hit.collider.TryGetComponent(out door)) || !door.CanSeeThrough)
                {
                    hasCollider = true;
                    return canWallHack;
                }
            }

            hasCollider = true;
            return true;
        }

        public bool HasLOS(TargetableBase p, out Vector3 position, out bool hasCollider, bool prioritizeHead = false, bool allowWallHack = false)
        {
            if (p == null)
            {
                position = Vector3.zero;
                hasCollider = true;
                return false;
            }

            if (!prioritizeHead)
            {
                if (CheckLOS(p.GetPosition(this), out hasCollider, allowWallHack && CanWallhack))
                {
                    position = p.GetPosition(this) + Vector3.up * AimOffset;
                    return true;
                }
                else if (CheckLOS(p.GetHeadPosition(this), out hasCollider, allowWallHack && CanWallhack))
                {
                    position = p.GetHeadPosition(this) + Vector3.down * AimOffset;
                    return true;
                }
            }
            else
            {
                if (CheckLOS(p.GetHeadPosition(this), out hasCollider, allowWallHack && CanWallhack))
                {
                    position = p.GetHeadPosition(this) + Vector3.down * AimOffset;
                    return true;
                }
                else if (CheckLOS(p.GetPosition(this), out hasCollider, allowWallHack && CanWallhack))
                {
                    position = p.GetPosition(this) + Vector3.up * AimOffset;
                    return true;
                }
            }

            position = Vector3.zero;
            return false;
        }

        public bool IsInView(TargetableBase p) => !CanStealth || GetDotProduct(p.GetPosition(this)) >= DotViewMinimum;

        public bool EquipItem<T>(Predicate<T> filter) where T : Item
        {
            if (!HasItem(out T t, filter))
                return false;

            EquipItem(t.Serial);
            return true;
        }

        public bool EquipItem<T>() where T : Item => EquipItem<T>(null);

        public void EquipItem(ushort serial)
        {
            /*Inventory.CurInstance = null;
            Inventory.ServerSelectItem(serial);*/
            Player.CurrentItem = Item.Get(serial);
        }

        public bool HasItem<T>(out T it, Predicate<T> filter) where T : Item
        {
            foreach (Item item in Core.Player.Items)
                if (item is T t && (filter == null || filter.Invoke(t)))
                {
                    it = t;
                    return true;
                }
            it = null;
            return false;
        }

        public bool HasItem<T>(out T it) where T : Item => HasItem(out it, null);

        public bool HasItem(ushort serial)
        {
            foreach (ItemBase item in Inventory.UserInventory.Items.Values)
                if (item.ItemSerial == serial)
                    return true;
            return false;
        }

        public bool HasItem<T>() where T : Item => HasItem<T>(out _);

        public T GetItem<T>() where T : Item
        {
            if (CurrentItem is T t)
                return t;
            return null;
        }

        public bool TryGetItem<T>(out T item) where T : Item
        {
            item = GetItem<T>();
            return item != null;
        }

        public List<T> GetAllItems<T>() where T : Item
        {
            List<T> list = [];
            foreach (Item item in Core.Player.Items)
                if (item is T t)
                    list.Add(t);
            return list;
        }

        /// <summary>
        /// Using Vector3.Dot().
        /// </summary>
        /// <param name="position"></param>
        /// <returns>1f if looking exactly at the object, 0f if perpendicular and -1f if opposite.</returns>
        public float GetDotProduct(Vector3 position) => MovementEngine.GetDotProduct(position);

        public bool TrySetDoor(Door door, bool state)
        {
            try
            {
                if (door.Base is CheckpointDoor checkpoint || door.Base.TryGetComponentInParent(out checkpoint))
                    door = Door.Get(checkpoint);

                if (!CanAccessDoor(door) && TryGetKeycardForDoor(door, out Keycard item))
                {
                    EquipItem(item.Serial);
                    Log.Debug("Attempting to hold keycard");
                }
                
                float st = door.Base.GetExactState();
                if (!CanAccessDoor(door) || door.Base.NetworkTargetState == state || (st > 0f && st < 1f))
                {
                    return false;
                }
                if (door.Base.NetworkTargetState != state)
                {
                    door.Base.ServerInteract(ReferenceHub, 0);
                    Log.Debug("Interacting with door.");
                }
                return true;
            } catch (Exception e)
            {
                Log.Error($"Error setting door: {e}");
                return false;
            }
        }

        public bool CanAccessDoor(Door door) => CheckPermissions(CurrentItem, door);

        public bool CheckPermissions(Item item, Door door) => door.RequiredPermissions.CheckPermissions(item?.Base, ReferenceHub);

        public Keycard GetKeycardForDoor(Door door)
        {
            List<Keycard> keycards = GetAllItems<Keycard>();
            foreach (Keycard item in keycards)
                if (CheckPermissions(item, door))
                    return item;
            return null;
        }

        public bool TryGetKeycardForDoor(Door door, out Keycard item)
        {
            item = GetKeycardForDoor(door);
            return item != null;
        }

        public bool HasFollowTarget
        {
            get
            {
                if (!CanFollow(FollowTarget))
                {
                    if (FollowTarget != null)
                    {
                        OnLostFollow?.Invoke(FollowTarget, FollowTarget.GetPosition(this));
                        FollowTarget = null;
                    }

                    return false;
                }

                return true;
            }
        }

        private bool prevHasCollider;

        public bool HasEnemyTarget { get; private set; }

        private bool UpdateHasEnemyTarget()
        {
            if (!CanTarget(EnemyTarget, out bool hasCollider))
            {
                if (EnemyTarget != null)
                {
                    OnLostEnemy?.Invoke(EnemyTarget, EnemyTarget.IsAlive ? EnemyTarget.GetPosition(this) : Position);
                    EnemyTarget = null;
                }

                return false;
            }
            else if (hasCollider && hasCollider != prevHasCollider)
                OnLostEnemy?.Invoke(EnemyTarget, EnemyTarget.GetPosition(this));

            prevHasCollider = hasCollider;

            return true;
        }

        public bool HasLOSOnEnemy(out Vector3 pos, out bool hasCollider, bool prioritizeHead = false) { pos = Vector3.zero; hasCollider = true; return HasEnemyTarget && HasLOS(EnemyTarget, out pos, out hasCollider, prioritizeHead, true); }

        public const string NoKOS = "npckos";

        public static bool DisableKOS => true;//ServerVariableManager.TryGetVar(NoKOS, out ServerVariable svar) && bool.TryParse(svar.Value, out bool v) && !v;

        public bool CanTarget(TargetableBase p, out bool hasCollider)
        {
            hasCollider = false;
            return !IsDisarmed(out _)
            //&& !HasEffect<Blinded>()
            && p != null
            && p.CanTarget(this, out hasCollider);
        }

        public bool CanFollow(TargetableBase p)
        {
            return //!HasEffect<Blinded>()
            p != null
            && p.CanFollow(this);
        }

        public bool HasEffect<T>() where T : StatusEffectBase => ReferenceHub.playerEffectsController.TryGetEffect(out T effect) && effect.IsEnabled;

        public int GetFollowWeight(Player p)
        {
            if (p == null)
                return -1;

            int weight = 0;

            if (!p.IsAI())
                weight += PlayerFollowWeight;

            if (NpcUtilities.ClassWeights.ContainsKey(p.Role))
                weight += NpcUtilities.ClassWeights[p.Role];

            if (NpcUtilities.ClassWeights.ContainsKey(Role))
                weight -= NpcUtilities.ClassWeights[Role];

            return weight;
        }

        public float GetDistance(TargetableBase p) 
        {
            if (p == null)
                return Mathf.Infinity;
            return Vector3.Distance(Position, p.GetPosition(this)); 
        }

        public bool WithinDistance(TargetableBase p, float dist) => GetDistance(p) <= dist;

        public bool IsDisarmed(out Player disarmer)
        {
            bool isDisarmed = ReferenceHub.inventory.IsDisarmed();
            disarmer = isDisarmed ? Player.Get(DisarmedPlayers.Entries.Find((DisarmedPlayers.DisarmedEntry x) => x.DisarmedPlayer == ReferenceHub.netId).Disarmer) : null;
            return isDisarmed;
        }

        public event Action<TargetableBase, Vector3> OnLostEnemy;
        public event Action<TargetableBase, Vector3> OnLostFollow;
        public event Action<RoleTypeId> OnRoleChange;
}