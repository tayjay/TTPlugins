using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using MapGeneration;
using MEC;
using PlayerRoles;
using TTCore.Npcs.AI.Core.Management;
using TTCore.Npcs.AI.Core.World.AIModules;
using TTCore.Npcs.AI.Pathing;
using UnityEngine;

namespace TTCore.Utilities;

public static class NpcUtilities
{
    public static readonly Dictionary<ZoneType, Room[]> ZoneElevatorRooms = [];

        public static readonly Dictionary<RoleTypeId, int> ClassWeights = new()
        {
            { RoleTypeId.None, int.MinValue },
            { RoleTypeId.Spectator, int.MinValue },
            { RoleTypeId.Filmmaker, int.MinValue },
            { RoleTypeId.Overwatch, int.MinValue },
            { RoleTypeId.CustomRole, int.MinValue },
            { RoleTypeId.Scp079, int.MinValue },
            { RoleTypeId.Tutorial, 0 },
            { RoleTypeId.ClassD, 0 },
            { RoleTypeId.Scientist, 0 },
            { RoleTypeId.FacilityGuard, 1 },
            { RoleTypeId.NtfPrivate, 2 },
            { RoleTypeId.NtfSergeant, 3 },
            { RoleTypeId.NtfSpecialist, 3 },
            { RoleTypeId.NtfCaptain, 4 },
            { RoleTypeId.ChaosRifleman, 2 },
            { RoleTypeId.ChaosConscript, 2 },
            { RoleTypeId.ChaosMarauder, 3 },
            { RoleTypeId.ChaosRepressor, 4 },
            { RoleTypeId.Scp0492, 0 },
            { RoleTypeId.Scp049, 3 },
            { RoleTypeId.Scp096, 3 },
            { RoleTypeId.Scp173, 3 },
            { RoleTypeId.Scp106, 3 },
            { RoleTypeId.Scp939, 3 },
            { RoleTypeId.Scp3114, 3 },
        };

        public static Quaternion SmoothDampQuaternion(this Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
        {
            Vector3 c = current.eulerAngles;
            Vector3 t = target.eulerAngles;
            return Quaternion.Euler(
              Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
              Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
              Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
            );
        }

        public static bool TryGetComponentInParent<T>(this Component go, out T component) where T : Component
        {
            component = go.GetComponentInParent<T>();
            return component != null;
        }

        public static AIPlayerProfile CreateBasicAI(RoleTypeId role, Vector3 position, float enemyDistance = 50f, float followDistance = 10f, float startFollowDistance = 3f, float stopFollowDistance = 1f)
        {
            Log.Debug("Creating AI Player.");
            AIPlayerProfile prof = new AIDataProfileBase("Bot").CreateAIPlayer(role);
            Log.Debug("Profile created.");
            
            Log.Debug("Setting Role.");

            Timing.CallDelayed(1f, () =>
            {
                prof.DisplayNickname = "Bot " + prof.Player.Id;
                prof.Player.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;
                prof.Player.RankName = "NPC";
                prof.Player.RankColor = "white";
            });
    
            prof.ReferenceHub.roleManager.ServerSetRole(role, RoleChangeReason.None, RoleSpawnFlags.None);
            prof.Position = position;
            Log.Debug("Role set.");
            AIScanner i = prof.WorldPlayer.ModuleRunner.AddModule<AIScanner>();
            AIPathfinder p = prof.WorldPlayer.ModuleRunner.AddModule<AIPathfinder>();
            AIFollow f = prof.WorldPlayer.ModuleRunner.AddModule<AIFollow>();
            AIWander w = prof.WorldPlayer.ModuleRunner.AddModule<AIWander>();
            AIChaseEnemy ce = prof.WorldPlayer.ModuleRunner.AddModule<AIChaseEnemy>();
            AIFirearmShoot s = prof.WorldPlayer.ModuleRunner.AddModule<AIFirearmStrafeShoot>();
            AIGrenadeStrafeThrow g = prof.WorldPlayer.ModuleRunner.AddModule<AIGrenadeStrafeThrow>();
            AIItemConsume c = prof.WorldPlayer.ModuleRunner.AddModule<AIItemConsume>();
            AIItemPickup ip = prof.WorldPlayer.ModuleRunner.AddModule<AIItemPickup>();
            ip.Enabled = false;
            prof.WorldPlayer.ModuleRunner.AddModule<AIZombieModule>();
            prof.WorldPlayer.ModuleRunner.AddModule<AIScp049Module>();
            prof.WorldPlayer.ModuleRunner.AddModule<AIScp106Module>();
            prof.WorldPlayer.ModuleRunner.AddModule<AIScp939Module>();
            Log.Debug("Modules added.");

            c.Enabled = false;
            prof.WorldPlayer.ModuleRunner.AddModule<AIBehaviorBase>();
            Log.Debug("Behavior Enabled.");
            i.SearchRadiusEnemy = 70f;
            i.SearchRadiusFollow = 20f;
            
            Log.Debug("Created AI Player.");
                
            
            Log.Debug("Returning Profile");
            return prof;
        }

        public static AIPlayerProfile CreateZombieAI(Vector3 position)
        {
            Log.Debug("Creating AI Zombie.");
            AIPlayerProfile prof = new AIDataProfileBase("Bot").CreateAIPlayer(RoleTypeId.Scp0492);
            Log.Debug("Profile created.");
            
            Log.Debug("Setting Role.");

            Timing.CallDelayed(1f, () =>
            {
                prof.DisplayNickname = "Zombie " + prof.Player.Id;
                prof.Player.RemoteAdminPermissions = PlayerPermissions.AFKImmunity;
                prof.Player.RankName = "NPC";
                prof.Player.RankColor = "white";
            });
    
            prof.ReferenceHub.roleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.None, RoleSpawnFlags.None);
            prof.Position = position;
            Log.Debug("Role set.");
            AIScanner i = prof.WorldPlayer.ModuleRunner.AddModule<AIScanner>();
            AIPathfinder p = prof.WorldPlayer.ModuleRunner.AddModule<AIPathfinder>();
            AIFollow f = prof.WorldPlayer.ModuleRunner.AddModule<AIFollow>();
            AIWander w = prof.WorldPlayer.ModuleRunner.AddModule<AIWander>();
            AIChaseEnemy ce = prof.WorldPlayer.ModuleRunner.AddModule<AIChaseEnemy>();
            
            prof.WorldPlayer.ModuleRunner.AddModule<AIZombieModule>();
            Log.Debug("Modules added.");

            prof.WorldPlayer.ModuleRunner.AddModule<AIBehaviorBase>();
            Log.Debug("Behavior Enabled.");
            i.SearchRadiusEnemy = 70f;
            i.SearchRadiusFollow = 20f;
            
            Log.Debug("Created AI Zombie.");
            
            Log.Debug("Returning Profile");
            return prof;
        }

        public static AIPlayerProfile CreateShootingDummy(RoleTypeId role, Vector3 position)
        {
            AIPlayerProfile p = CreateStaticAI(role, position);
            p.Player.Health = 999999f;
            return p;
        }

        public static AIPlayerProfile CreateStaticAI(RoleTypeId role, Vector3 position, float enemyDistance = 50f)
        {
            AIPlayerProfile prof = new AIDataProfileBase("Static Bot").CreateAIPlayer();
            prof.DisplayNickname = "Static Bot " + prof.Player.Id;

            prof.ReferenceHub.roleManager.ServerSetRole(role, RoleChangeReason.None, RoleSpawnFlags.None);
            prof.Position = position;

            AIScanner i = prof.WorldPlayer.ModuleRunner.AddModule<AIScanner>();
            AIFirearmShoot s = prof.WorldPlayer.ModuleRunner.AddModule<AIFirearmShoot>();
            AIGrenadeThrow g = prof.WorldPlayer.ModuleRunner.AddModule<AIGrenadeThrow>();
            AIItemConsume c = prof.WorldPlayer.ModuleRunner.AddModule<AIItemConsume>();
            prof.WorldPlayer.ModuleRunner.AddModule<AIBehaviorBase>();

            g.InfiniteGrenades = true;

            return prof;
        }

        public static AIPlayerProfile CreatePathAI(RoleTypeId role, Vector3 position, Path p)
        {
            AIPlayerProfile prof = new AIDataProfileBase("Path Bot").CreateAIPlayer();
            prof.DisplayNickname = "Path Bot " + prof.Player.Id;

            prof.ReferenceHub.roleManager.ServerSetRole(role, RoleChangeReason.None, RoleSpawnFlags.None);
            prof.Position = position;

            AIFollowPath f = prof.WorldPlayer.ModuleRunner.AddModule<AIFollowPath>();
            f.Path = p;
            f.InitPath();

            return prof;
        }

        public static AIPlayerProfile CreateGrenadeAI(RoleTypeId role, Vector3 position, float enemyDistance = 50f, float followDistance = 10f, float startFollowDistance = 3f, float stopFollowDistance = 1f)
        {
            AIPlayerProfile prof = new AIDataProfileBase("Grenade Bot").CreateAIPlayer();
            prof.DisplayNickname = "Grenade Bot " + prof.Player.Id;

            prof.ReferenceHub.roleManager.ServerSetRole(role, RoleChangeReason.None, RoleSpawnFlags.None);
            prof.Position = position;

            AIScanner i = prof.WorldPlayer.ModuleRunner.AddModule<AIScanner>();
            AIPathfinder p = prof.WorldPlayer.ModuleRunner.AddModule<AIPathfinder>();
            AIFollow f = prof.WorldPlayer.ModuleRunner.AddModule<AIFollow>();
            AIGrenadeThrow s = prof.WorldPlayer.ModuleRunner.AddModule<AIGrenadeThrow>();
            AIItemConsume c = prof.WorldPlayer.ModuleRunner.AddModule<AIItemConsume>();
            prof.WorldPlayer.ModuleRunner.AddModule<AIBehaviorBase>();

            return prof;
        }
}