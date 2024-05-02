using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp3114;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;
using VoiceChat;

namespace RoundModifiers.Modifiers
{
    public class BoneZone : Modifier
    {
        private RoomType[] scpSpawnRooms = new[] { RoomType.Hcz049, RoomType.Hcz939, RoomType.HczNuke };
        public Dictionary<RoleTypeId, string> ClassTitles { get; set; } = new Dictionary<RoleTypeId, string>
    {
        {
            RoleTypeId.ClassD, "D-"
        },
        {
            RoleTypeId.Scientist, "Dr. "
        },
        {
            RoleTypeId.FacilityGuard, "Security Officer "
        },
        {
            RoleTypeId.NtfCaptain, "Captain "
        },
        {
            RoleTypeId.NtfPrivate, "Private "
        },
        {
            RoleTypeId.NtfSergeant, "Sergeant "
        },
        {
            RoleTypeId.NtfSpecialist, "Field Agent "
        },
        {
            RoleTypeId.ChaosConscript, "Agent of Chaos "
        },
        {
            RoleTypeId.ChaosMarauder, "Agent of Chaos "
        },
        {
            RoleTypeId.ChaosRepressor, "Agent of Chaos "
        },
        {
            RoleTypeId.ChaosRifleman, "Agent of Chaos "
        },
        {
            RoleTypeId.Scp049, "SCP-049"
        },
        {
            RoleTypeId.Scp0492, "SCP-049-2"
        },
        {
            RoleTypeId.Scp079, "SCP-079"
        },
        {
            RoleTypeId.Scp096, "SCP-096"
        },
        {
            RoleTypeId.Scp106, "SCP-106"
        },
        {
            RoleTypeId.Scp173, "SCP-173"
        },
        {
            RoleTypeId.Scp939, "SCP-939"
        },
        /*{
            RoleTypeId.Scp3114, "SCP-3114"
        }*/
    };

    public string[] HumanNames { get; set; } = new[]
    {
        "Alan", "Steeve", "Mary", "John", "Alice", "Bob", "Carol", "David", "Eve", "Frank",
        "Grace", "Helen", "Ian", "Judy", "Kevin", "Linda", "Mike", "Nora", "Oliver", "Patricia",
        "Quinn", "Rachel", "Sam", "Tina", "Ursula", "Vince", "Wendy", "Xavier", "Yvonne", "Zack",
        "Amber", "Bruce", "Cindy", "Derek", "Elena", "Felix", "Gina", "Harry", "Isla", "Justin",
        "Kara", "Leo", "Mona", "Nathan", "Oscar", "Penny", "Quincy", "Rose", "Seth", "Tara",
        "Ulysses", "Victor", "Willa", "Xander", "Yasmin", "Zeke", "April", "Blaine", "Claire",
        "Dante", "Elise", "Frederick", "Gloria", "Howard", "Ingrid", "Joel", "Krista", "Luke",
        "Megan", "Neil", "Opal", "Paul", "Queenie", "Roger", "Susan", "Thomas", "Una", "Vernon",
        "Whitney", "Xenia", "Yuri", "Zara", "Aaron", "Beth", "Carter", "Deanna", "Elliott", "Faye",
        "George", "Hannah", "Ivan", "Jean", "Kyle", "Leslie", "Mitchell", "Nadia", "Owen", "Paula",
        "Quentin", "Ruth", "Spencer", "Tiffany", "Uma", "Vincent", "Wallace", "Xena", "Yvette", "Zion",
        "Taylar", "Ely", "Jason", "Kevin", "Chance", "Vivian"
    };
    
    //public Dictionary<Player, bool> CanReveal { get; set; }
        
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (RoleExtensions.GetTeam(ev.NewRole) == Team.SCPs && ev.NewRole != RoleTypeId.Scp3114)
            {
                ev.NewRole = RoleTypeId.Scp3114;
                return;
            }
            if(!ev.NewRole.IsAlive()) return;
            if(!ev.NewRole.IsHuman()) return;
            if(ev.NewRole == RoleTypeId.Scp3114) return;
            string newName = ClassTitles[ev.NewRole];
            
            if (ev.NewRole == RoleTypeId.ClassD)
            {
                newName += Random.Range((int)1000, (int)9999);
            } else if (ev.NewRole.IsHuman())
            {
                newName += HumanNames[Random.Range(0, HumanNames.Length)];
            }
            ev.Player.DisplayNickname = newName;
            ev.Player.ShowHUDHint("Your name is " + ev.Player.DisplayNickname, 10f);
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            if(ev.Player.Role.Type==RoleTypeId.Scp3114)
                ev.Position = Room.Get(scpSpawnRooms.RandomItem()).Position + Vector3.up;
        }
        
        public void OnPlayerDeath(DiedEventArgs ev)
        {
            ev.Player.DisplayNickname = null;
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp3114)
            {
                Timing.CallDelayed(1f,() =>
                {
                    Ragdoll.CreateAndSpawn(RoleTypeId.FacilityGuard, ClassTitles[RoleTypeId.FacilityGuard]+HumanNames[Random.Range(0, HumanNames.Length)], "ded", ev.Player.Position+Vector3.up, ev.Player.Rotation);
                    Pickup.CreateAndSpawn(ItemType.GunFSP9, ev.Player.Position+Vector3.up, ev.Player.Rotation);
                    ev.Player.MaxHealth *= RoundModifiers.Instance.Config.BoneZone_Scp3114HealthScale;
                    ev.Player.Health = ev.Player.MaxHealth;
                });
            }
        }
        
        

        public void OnDisguised(DisguisedEventArgs ev)
        {
            //Get a new name for the character
            string newName = ClassTitles[ev.Ragdoll.Role];
            if (ev.Ragdoll.Role == RoleTypeId.ClassD)
            {
                newName += Random.Range((int)1000, (int)9999);
            } else if (ev.Ragdoll.Role.IsHuman())
            {
                newName += HumanNames[Random.Range(0, HumanNames.Length)];
            }

            ev.Player.DisplayNickname = newName;
            ev.Player.ShowHUDHint("Your name is " + ev.Player.DisplayNickname, 10f);
            ev.Player.VoiceChannel = VoiceChatChannel.Proximity;

            //ev.Player.ReferenceHub.interCoordinator._blockers.Clear();
        }
        
        public void OnRevealed(RevealedEventArgs ev)
        {
            ev.Player.VoiceChannel = VoiceChatChannel.ScpChat;
        }

        public void OnHurt(HurtEventArgs ev)
        {
            if(ev.Player.Role!=RoleTypeId.Scp3114) return;
            if(ev.Attacker == null) return;
            if(ev.Attacker.Role == RoleTypeId.Scp3114) return;
            //CanReveal[ev.Player] = true;
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.Spawning += OnSpawning;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurt += OnHurt;
            Exiled.Events.Handlers.Scp3114.Disguised += OnDisguised;
            Exiled.Events.Handlers.Scp3114.Revealed += OnRevealed;

            
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.Spawning -= OnSpawning;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            Exiled.Events.Handlers.Player.Hurt -= OnHurt;
            Exiled.Events.Handlers.Scp3114.Disguised -= OnDisguised;
            Exiled.Events.Handlers.Scp3114.Revealed -= OnRevealed;
            
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