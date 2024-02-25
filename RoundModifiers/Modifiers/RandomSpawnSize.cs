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
        
        public void OnSpawned(SpawnedEventArgs ev)
        {
            Log.Debug("Setting " + ev.Player.Nickname + " size to random.");
            if (ev.Player.Role.Type != RoleTypeId.Scp079 && ev.Player.Role.Type != RoleTypeId.Spectator && ev.Player.Role.Type != RoleTypeId.None && ev.Player.Role.Type != RoleTypeId.Filmmaker && !ev.Player.Role.IsDead)
            {
                float random = Random.Range(RoundModifiers.Instance.Config.RandomSpawnSize_SizeMin, RoundModifiers.Instance.Config.RandomSpawnSize_SizeMax);
                ev.Player.ChangeSize(random);
            }
        }

        public void OnSpawnItem(SpawningItemEventArgs ev)
        {
            ev.Pickup.Scale = Vector3.one * Random.Range(RoundModifiers.Instance.Config.RandomSpawnSize_SizeMin, RoundModifiers.Instance.Config.RandomSpawnSize_SizeMax);
        }
        
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            ev.Pickup.Scale = Vector3.one * Random.Range(RoundModifiers.Instance.Config.RandomSpawnSize_SizeMin, RoundModifiers.Instance.Config.RandomSpawnSize_SizeMax);
        }
        
        public void OnDropItem(DroppedItemEventArgs ev)
        {
            ev.Pickup.Scale = Vector3.one * Random.Range(RoundModifiers.Instance.Config.RandomSpawnSize_SizeMin, RoundModifiers.Instance.Config.RandomSpawnSize_SizeMax);
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Map.SpawningItem += OnSpawnItem;
            Exiled.Events.Handlers.Map.FillingLocker += OnFillingLocker;
            Exiled.Events.Handlers.Player.DroppedItem += OnDropItem;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Map.SpawningItem -= OnSpawnItem;
            Exiled.Events.Handlers.Map.FillingLocker -= OnFillingLocker;
            Exiled.Events.Handlers.Player.DroppedItem -= OnDropItem;
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