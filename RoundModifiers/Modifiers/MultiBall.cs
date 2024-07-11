using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using RoundModifiers.API;
using TTCore.Components;
using TTCore.Events.EventArgs;
using TTCore.Utilities;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class MultiBall : Modifier
    {

        
        
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            ItemType spawningItem = ev.Pickup.Type;
            if (spawningItem == ItemType.SCP207
                || spawningItem == ItemType.SCP244a
                || spawningItem == ItemType.SCP268
                || spawningItem == ItemType.SCP500
                || spawningItem == ItemType.SCP1576
                || spawningItem == ItemType.SCP1853
                || spawningItem == ItemType.SCP2176)
            {
                Pickup.Create(ItemType.SCP018).Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                ev.Pickup.UnSpawn();
                ev.IsAllowed = false;
                Log.Debug("Attempting to spawn SCP018 instead of "+ev.Pickup.Info.ItemId);
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

        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
            TTCore.Events.Handlers.Custom.Scp018Bounce += OnBounce;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
            TTCore.Events.Handlers.Custom.Scp018Bounce -= OnBounce;
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
    }
}