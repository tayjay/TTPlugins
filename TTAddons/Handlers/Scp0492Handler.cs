using CustomPlayerEffects;
using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Enums;
using TTCore.API;

namespace TTAddons.Handlers
{
    public class Scp0492Handler : IRegistered
    {
        public void OnZombieSpawned(SpawnedEventArgs ev)
        {
            if(ev.Player.Role != RoleTypeId.Scp0492) return;
            if (TTAddons.Instance.Config.Scp0492SpawnProtection > 0)
                ev.Player.EnableEffect<SpawnProtected>(TTAddons.Instance.Config.Scp0492SpawnProtection);
        }

        public void Register()
        {
            Exiled.Events.Handlers.Player.Spawned += OnZombieSpawned;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnZombieSpawned;
        }
    }
}