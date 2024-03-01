using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class BoneZone : Modifier
    {
        private RoomType[] scpSpawnRooms = new[] { RoomType.Hcz049, RoomType.Hcz939, RoomType.HczNuke };
        public void OnChoosingStartTeam(ChoosingStartTeamQueueEventArgs ev)
        {
            
        }
        
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (RoleExtensions.GetTeam(ev.NewRole) == Team.SCPs && ev.NewRole != RoleTypeId.Scp3114)
            {
                ev.NewRole = RoleTypeId.Scp3114;
            }
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            if(ev.Player.Role.Type==RoleTypeId.Scp3114)
                ev.Position = Room.Get(scpSpawnRooms.RandomItem()).Position + Vector3.up;
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role is Scp3114Role scp3114Role)
            {
                Timing.CallDelayed(1f,() =>
                {
                    Ragdoll.CreateAndSpawn(RoleTypeId.FacilityGuard, ev.Player.Nickname, "ded", ev.Player.Position+Vector3.up, ev.Player.Rotation);
                    Pickup.CreateAndSpawn(ItemType.GunFSP9, ev.Player.Position+Vector3.up, ev.Player.Rotation);
                });
            }
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.Spawning += OnSpawning;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.Spawning -= OnSpawning;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "BoneZone",
            FormattedName = "<color=red>Bone Zone</color>",
            Aliases = new []{"bz","skeletons"},
            Description = "All Scps are skeletons.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
    }
}