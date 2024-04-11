using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Extensions;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class RandomSpawnSize : Modifier
    {
        
        public float SizeMin => RoundModifiers.Instance.Config.RandomSpawnSize_SizeMin;
        public float SizeMax => RoundModifiers.Instance.Config.RandomSpawnSize_SizeMax;
        public bool AffectPickups => RoundModifiers.Instance.Config.RandomSpawnSize_AffectPickups;
        public bool UniformSize => RoundModifiers.Instance.Config.RandomSpawnSize_Uniform;
        
        public void OnSpawned(SpawnedEventArgs ev)
        {
            Log.Debug("Setting " + ev.Player.Nickname + " size to random.");
            if (ev.Player.Role.Type != RoleTypeId.Scp079 && ev.Player.Role.Type != RoleTypeId.Spectator && ev.Player.Role.Type != RoleTypeId.None && ev.Player.Role.Type != RoleTypeId.Filmmaker && !ev.Player.Role.IsDead)
            {
                float random = Random.Range(SizeMin, SizeMax);
                float x = Random.Range(SizeMin, SizeMax);
                float y = Random.Range(SizeMin, SizeMax);
                float z = Random.Range(SizeMin, SizeMax);
                if(UniformSize)
                    ev.Player.ChangeSize(random);
                else
                    ev.Player.ChangeSize(new Vector3(x, y, z));
            }
        }

        public void OnSpawnItem(SpawningItemEventArgs ev)
        {
            if(!AffectPickups) return;
            float x = Random.Range(SizeMin, SizeMax);
            float y = Random.Range(SizeMin, SizeMax);
            float z = Random.Range(SizeMin, SizeMax);
            if(UniformSize)
                x=y=z;
            ev.Pickup.Scale = new Vector3(x,y,z);
            ev.Pickup.Weight *= ev.Pickup.Scale.magnitude;
        }
        
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            if(!AffectPickups) return;
            float x = Random.Range(SizeMin, SizeMax);
            float y = Random.Range(SizeMin, SizeMax);
            float z = Random.Range(SizeMin, SizeMax);
            if(UniformSize)
                x=y=z;
            ev.Pickup.Scale = new Vector3(x,y,z);
            ev.Pickup.Weight *= ev.Pickup.Scale.magnitude;
        }
        
        public void OnDropItem(DroppedItemEventArgs ev)
        {
            if(!AffectPickups) return;
            float x = Random.Range(SizeMin, SizeMax);
            float y = Random.Range(SizeMin, SizeMax);
            float z = Random.Range(SizeMin, SizeMax);
            if(UniformSize)
                x=y=z;
            ev.Pickup.Scale = new Vector3(x,y,z);
            ev.Pickup.Weight *= ev.Pickup.Scale.magnitude;
        }

        public void OnThrowProjectile(ThrownProjectileEventArgs ev)
        {
            if(!AffectPickups) return;
            float x = Random.Range(SizeMin, SizeMax);
            float y = Random.Range(SizeMin, SizeMax);
            float z = Random.Range(SizeMin, SizeMax);
            if(UniformSize)
                x=y=z;
            ev.Projectile.Scale = new Vector3(x,y,z);
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Map.SpawningItem += OnSpawnItem;
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Player.DroppedItem += OnDropItem;
            Exiled.Events.Handlers.Player.ThrownProjectile += OnThrowProjectile;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Map.SpawningItem -= OnSpawnItem;
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Player.DroppedItem -= OnDropItem;
            Exiled.Events.Handlers.Player.ThrownProjectile -= OnThrowProjectile;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "RandomSpawnSize",
            FormattedName = "<color=yellow>Random Spawn Size</color>",
            Aliases = new []{"rss"},
            Description = "Randomly changes the size of spawned players.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = true
        };
    }
}