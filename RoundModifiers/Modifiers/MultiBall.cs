using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using MapGeneration.Distributors;
using MEC;
using Mirror;
using RoundModifiers.API;
using TTCore.API;
using TTCore.Components;
using TTCore.Events.EventArgs;
using TTCore.Utilities;
using UnityEngine;
using Utils.NonAllocLINQ;
using Random = UnityEngine.Random;

namespace RoundModifiers.Modifiers
{
    public class MultiBall : Modifier
    {

        public static List<LockerPosition> NewLockerPositions = new List<LockerPosition>();
        
        public bool Generated { get; private set; }
        public void OnGenerated()
        {
            NewLockerPositions = new List<LockerPosition>()
            {
                new LockerPosition(new RoomPosition(RoomType.EzIntercom, new Vector3(-0.09283455f, -5.81958f, -0.5506745f)), Quaternion.Euler(0f, -90f, 0f)),
                new LockerPosition(new RoomPosition(RoomType.Hcz106, new Vector3(19.02904f, -0.06542969f, -10.41412f)), Quaternion.Euler(0f, 90f, 0f)),
                new LockerPosition(new RoomPosition(RoomType.EzShelter, new Vector3(-2.326324f, 0f, 6.645569f)), Quaternion.Euler(0f, 90f, 0f)),
                new LockerPosition(new RoomPosition(RoomType.Hcz079, new Vector3(-7.343576f, -5.235657f, -4.533012f)), Quaternion.Euler(0f, 90f, 0f)),
                new LockerPosition(new RoomPosition(RoomType.Surface, new Vector3(132.3266f, -12.16785f, 24.56247f)), Quaternion.Euler(0f, 180f, 0f)),
            };
            Log.Debug("Replacing SCP018 lockers");
            List<Transform> lockerTransforms = new List<Transform>();
            foreach (Locker locker in Map.Lockers)
            {
                if (locker.GetType() == typeof(PedestalScpLocker))
                {
                    Log.Debug("Replacing SCP018 locker");
                    lockerTransforms.Add(locker.transform);
                    NetworkServer.UnSpawn(locker.gameObject);
                }
                Log.Debug("Found other locker");
            }
            foreach (Transform lockerTransform in lockerTransforms)
            {
                Log.Debug("Placing SCP018 pedestal");
                PrefabHelper.Spawn(PrefabType.Scp018PedestalStructure, lockerTransform.position, lockerTransform.rotation);
            }
            foreach(LockerPosition pos in NewLockerPositions)
            {
                Log.Debug("Placing Extra SCP018 locker");
                PrefabHelper.Spawn(PrefabType.Scp018PedestalStructure, pos.RoomPosition.GlobalPosition, pos.Rotation*pos.RoomPosition.Room.Rotation);
            }
        }
        
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            ItemType spawningItem = ev.Pickup.Type;
            if (spawningItem == ItemType.SCP207
                || spawningItem == ItemType.SCP244a
                || spawningItem == ItemType.SCP268
                || spawningItem == ItemType.SCP500
                || spawningItem == ItemType.SCP1576
                || spawningItem == ItemType.SCP1853
                || spawningItem == ItemType.SCP2176
                || spawningItem == ItemType.AntiSCP207)
            {
                //Pickup.Create(ItemType.SCP018).Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ev.Pickup.UnSpawn();
                ev.IsAllowed = false;
                //Log.Debug("Attempting to spawn SCP018 instead of "+ev.Pickup.Info.ItemId);
            }
            else
            {
                float random = Random.Range(0f, 1f);
                if (random <= LockerSpawnChance)
                {
                    Pickup.Create(ItemType.SCP018).Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                    ev.Pickup.UnSpawn();
                    ev.IsAllowed = false;
                    Log.Debug("Attempting to spawn SCP018 instead of "+ev.Pickup.Info.ItemId);
                }
            }
            
        }

        public void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (ev.Item.Type == ItemType.SCP018)
            {
                ev.Pickup.Scale = Vector3.one * BallScale;
                ev.Projectile.GameObject.AddComponent<ProjectileCollisionHandler>().Init((ev.Player ?? Exiled.API.Features.Server.Host).GameObject, ev.Projectile.Base);
                /*Log.Debug("Spawning an extra "+count+" balls");
                for (int i = 0; i < count; i++)
                {
                    Pickup pickup = ev.Pickup.Clone();
                    pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                    pickup.Scale = Vector3.one * BallScale;
                    pickup.PhysicsModule.Rb.velocity = ev.Pickup.PhysicsModule.Rb.velocity;
                }*/
            }
        }

        public void OnBounce(Scp018BounceEventArgs ev)
        {
            if(Random.Range(0f,1f) <= ExtraBallSpawnChance)
                SpawnExtraBall(ev.Projectile, ev.Owner);
        }
        
        public void SpawnExtraBall(ThrownProjectile oldProjectile, GameObject owner)
        {
            Projectile projectile = Projectile.CreateAndSpawn(ProjectileType.Scp018, oldProjectile.Position, oldProjectile.Rotation, true,
                Player.Get(oldProjectile.PreviousOwner));
            projectile.PhysicsModule.Rb.velocity = Vector3Utils.Random(5f,10f);
            projectile.Scale = Vector3.one * BallScale;
            //(projectile as Scp018Projectile).FuseTime = (Projectile.Get(ev.Projectile) as Scp018Projectile).FuseTime;
            if(Recursive)
                projectile.GameObject.AddComponent<ProjectileCollisionHandler>().Init(owner, projectile.Base);
        }

        public void OnOpenLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Locker is not PedestalScpLocker) return;
            Timing.CallDelayed(0.5f, () =>
            {
                double chance = Math.Min((Round.ElapsedTime.TotalMinutes * 0.03), 0.25f);
                if(!ev.Chamber.IsOpen) return;
                if (ev.Chamber._content.Any(i => i.Info.ItemId == ItemType.SCP018))
                {
                    if (Random.Range(0, 1) <= chance)
                    {
                        var oldItem = ev.Chamber._content.First(i => i.Info.ItemId == ItemType.SCP018);
                        Projectile projectile = Projectile.CreateAndSpawn(ProjectileType.Scp018, oldItem.Position, oldItem.Rotation, true, ev.Player);
                        projectile.PhysicsModule.Rb.velocity = Vector3Utils.Random(-2f, 3f);
                        Pickup.Get(oldItem).UnSpawn();
                    }
                }
            });
        }

        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
            Exiled.Events.Handlers.Player.InteractingLocker += OnOpenLocker;
            TTCore.Events.Handlers.Custom.Scp018Bounce += OnBounce;
            Generated = false;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnOpenLocker;
            TTCore.Events.Handlers.Custom.Scp018Bounce -= OnBounce;
            Generated = false;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "MultiBall",
            FormattedName = "<color=red>MultiBall</color>",
            Description = "All SCP Lockers are filled with SCP018s, and SCP018s spawn extra balls when thrown.",
            Aliases = new []{"018","balls"},
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = true,
            Balance = 3,
            Category = Category.ScpItem
        };
        
        public static MultiBall.Config MultiBallConfig => RoundModifiers.Instance.Config.MultiBall;
        public static float BallScale => MultiBallConfig.BallScale;
        public static float ExtraBallSpawnChance => MultiBallConfig.ExtraBallsChance;
        public static float LockerSpawnChance => MultiBallConfig.LockerSpawnChance;
        public static bool Recursive => MultiBallConfig.Recursive;

        public class Config
        {
            [Description("The scale of the MultiBall balls when thrown. Default is 3.")]
            public float BallScale { get; set; } = 3;
            [Description("The chance of a locker spawning SCP-018 instead of the original item. Default is 0.01. (0-1)f")]
            public float LockerSpawnChance { get; set; } = 0.01f;
        
            [Description("The chance of an SCP-018 ball spawning extra balls when it bounces. Default is 0.01. (0-1)f")]
            public float ExtraBallsChance { get; set; } = 0.01f;
            [Description("Do the bonus balls from SCP-018 spawn more balls? Default is false.")]
            public bool Recursive { get; set; } = false;
        }

        public struct LockerPosition
        {
            public RoomPosition RoomPosition;
            public Quaternion Rotation;
            
            public LockerPosition(RoomPosition roomPosition, Quaternion rotation)
            {
                RoomPosition = roomPosition;
                Rotation = rotation;
            }
        }
    }
}