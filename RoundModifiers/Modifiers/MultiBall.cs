using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class MultiBall : Modifier
    {

        public float BallScale => RoundModifiers.Instance.Config.MultiBall_BallScale;
        public int ExtraBallsMin => RoundModifiers.Instance.Config.MultiBall_ExtraBallsMin;
        public int ExtraBallsMax => RoundModifiers.Instance.Config.MultiBall_ExtraBallsMax;
        public float LockerSpawnChance => RoundModifiers.Instance.Config.MultiBall_LockerSpawnChance;
        
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
                int count = Random.Range(ExtraBallsMin, ExtraBallsMax);
                Log.Debug("Spawning an extra "+count+" balls");
                for (int i = 0; i < count; i++)
                {
                    Pickup pickup = ev.Pickup.Clone();
                    pickup.Spawn(ev.Pickup.Position, ev.Pickup.Rotation);
                    pickup.Scale = Vector3.one * BallScale;
                    pickup.PhysicsModule.Rb.velocity = ev.Pickup.PhysicsModule.Rb.velocity;
                }
            }
        }

        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrownProjectile;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrownProjectile;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "MultiBall",
            Description = "All SCP Lockers are filled with SCP018s, and SCP018s spawn extra balls when thrown.",
            Aliases = new []{"018","balls"},
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = true
        };
    }
}