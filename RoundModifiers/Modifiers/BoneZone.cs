using System.Collections.Generic;
using System.ComponentModel;
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
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using RoundModifiers.API;
using RoundModifiers.Modifiers.Nicknames;
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
                RoleTypeId.ClassD, Nicknames.Nicknames.NicknamesConfig.ClassDPrefix
            },
            {
                RoleTypeId.Scientist, Nicknames.Nicknames.NicknamesConfig.ScientistPrefix
            },
            {
                RoleTypeId.FacilityGuard, Nicknames.Nicknames.NicknamesConfig.FacilityGuardPrefix
            },
            {
                RoleTypeId.NtfCaptain, Nicknames.Nicknames.NicknamesConfig.NtfCaptainPrefix
            },
            {
                RoleTypeId.NtfPrivate, Nicknames.Nicknames.NicknamesConfig.NtfPrivatePrefix
            },
            {
                RoleTypeId.NtfSergeant, Nicknames.Nicknames.NicknamesConfig.NtfSergeantPrefix
            },
            {
                RoleTypeId.NtfSpecialist, Nicknames.Nicknames.NicknamesConfig.NtfSpecialistPrefix
            },
            {
                RoleTypeId.ChaosConscript, Nicknames.Nicknames.NicknamesConfig.ChaosConscriptPrefix
            },
            {
                RoleTypeId.ChaosMarauder, Nicknames.Nicknames.NicknamesConfig.ChaosMarauderPrefix
            },
            {
                RoleTypeId.ChaosRepressor, Nicknames.Nicknames.NicknamesConfig.ChaosRepressorPrefix
            },
            {
                RoleTypeId.ChaosRifleman, Nicknames.Nicknames.NicknamesConfig.ChaosRiflemanPrefix
            },
            {
                RoleTypeId.Scp049, Nicknames.Nicknames.NicknamesConfig.Scp049Prefix
            },
            {
                RoleTypeId.Scp0492, Nicknames.Nicknames.NicknamesConfig.Scp0492Prefix
            },
            {
                RoleTypeId.Scp079, Nicknames.Nicknames.NicknamesConfig.Scp079Prefix
            },
            {
                RoleTypeId.Scp096, Nicknames.Nicknames.NicknamesConfig.Scp096Prefix
            },
            {
                RoleTypeId.Scp106, Nicknames.Nicknames.NicknamesConfig.Scp106Prefix
            },
            {
                RoleTypeId.Scp173, Nicknames.Nicknames.NicknamesConfig.Scp173Prefix
            },
            {
                RoleTypeId.Scp939, Nicknames.Nicknames.NicknamesConfig.Scp939Prefix
            },
            {
                RoleTypeId.Scp3114, Nicknames.Nicknames.NicknamesConfig.Scp3114Prefix
            }
        };

        public string[] HumanNames => NicknameData.Nicknames;
        
        public NicknameData NicknameData => Nicknames.Nicknames.NicknameData;
    
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
                int num = Random.Range(1, 9999);
                string extended = "";
                if(num < 10)
                {
                    extended = "000" + num;
                } else if(num < 100)
                {
                    extended = "00" + num;
                } else if(num < 1000)
                {
                    extended = "0" + num;
                } else
                {
                    extended = num.ToString();
                }
                newName += extended;
            } else if (ev.NewRole.IsHuman())
            {
                if (Nicknames.Nicknames.NicknamesConfig.UseCurrentPlayerNames)
                {
                    newName += Player.List.GetRandomValue().Nickname;
                }
                else
                {
                    newName += HumanNames[Random.Range(0, HumanNames.Length)];
                }
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
                    ev.Player.MaxHealth *= Scp3114HealthScale;
                    ev.Player.Health = ev.Player.MaxHealth;
                });
            }
        }

        public void OnPressNoClip(TogglingNoClipEventArgs ev)
        {
            if(ev.Player.IsNoclipPermitted) return;
            ev.Player.ShowHUDHint("Your name is " + ev.Player.DisplayNickname, 10f);
        }
        
        [PluginEvent(ServerEventType.PlayerGameConsoleCommandExecuted)]
        public void OnPlayerGameConsoleCommandExecutedEvent(PlayerGameConsoleCommandExecutedEvent ev)
        {
            if(ev.Command == "name" || ev.Command == "nick")
                ev.Response = "Your name is " + ev.Player.DisplayNickname;
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
            Exiled.Events.Handlers.Player.TogglingNoClip += OnPressNoClip;

            PluginAPI.Events.EventManager.RegisterEvents(this);
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
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnPressNoClip;
            
            PluginAPI.Events.EventManager.UnregisterEvents(this);
            
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "BoneZone",
            FormattedName = "<color=red>Bone Zone</color>",
            Aliases = new []{"bz","skeletons"},
            Description = "All Scps are skeletons.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = -3,
            Category = Category.ScpRole | Category.Health
        };
        
        public Config BoneZoneConfig => RoundModifiers.Instance.Config.BoneZone;
        public float Scp3114HealthScale => BoneZoneConfig.Scp3114HealthScale;
        
        public class Config
        {
            [Description("How much health to give Scp-3114s during BoneZone. Default is 0.5f.")]
            public float Scp3114HealthScale { get; set; } = 0.5f;
        }
    }
}