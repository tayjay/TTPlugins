using Exiled.API.Features;
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
            if (ev.Player.Role.Type != RoleTypeId.Scp079 && ev.Player.Role.Type != RoleTypeId.Spectator && ev.Player.Role.Type != RoleTypeId.None && ev.Player.Role.Type != RoleTypeId.Filmmaker)
            {
                float random = Random.Range(RoundModifiers.Instance.Config.RandomSpawnSize_SizeMin, RoundModifiers.Instance.Config.RandomSpawnSize_SizeMax);
                ev.Player.ChangeSize(random);
            }
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "RandomSpawnSize",
            Aliases = new []{"rss"},
            Description = "Randomly changes the size of spawned items.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}