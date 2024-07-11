using System.Collections.Generic;
using System.ComponentModel;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;

namespace RoundModifiers.Modifiers.Nicknames;

public class Nicknames : Modifier
{
    public Dictionary<RoleTypeId, string> ClassTitles { get; set; } = new Dictionary<RoleTypeId, string>
    {
        {
            RoleTypeId.ClassD, NicknamesConfig.ClassDPrefix
        },
        {
            RoleTypeId.Scientist, NicknamesConfig.ScientistPrefix
        },
        {
            RoleTypeId.FacilityGuard, NicknamesConfig.FacilityGuardPrefix
        },
        {
            RoleTypeId.NtfCaptain, NicknamesConfig.NtfCaptainPrefix
        },
        {
            RoleTypeId.NtfPrivate, NicknamesConfig.NtfPrivatePrefix
        },
        {
            RoleTypeId.NtfSergeant, NicknamesConfig.NtfSergeantPrefix
        },
        {
            RoleTypeId.NtfSpecialist, NicknamesConfig.NtfSpecialistPrefix
        },
        {
            RoleTypeId.ChaosConscript, NicknamesConfig.ChaosConscriptPrefix
        },
        {
            RoleTypeId.ChaosMarauder, NicknamesConfig.ChaosMarauderPrefix
        },
        {
            RoleTypeId.ChaosRepressor, NicknamesConfig.ChaosRepressorPrefix
        },
        {
            RoleTypeId.ChaosRifleman, NicknamesConfig.ChaosRiflemanPrefix
        },
        {
            RoleTypeId.Scp049, NicknamesConfig.Scp049Prefix
        },
        {
            RoleTypeId.Scp0492, NicknamesConfig.Scp0492Prefix
        },
        {
            RoleTypeId.Scp079, NicknamesConfig.Scp079Prefix
        },
        {
            RoleTypeId.Scp096, NicknamesConfig.Scp096Prefix
        },
        {
            RoleTypeId.Scp106, NicknamesConfig.Scp106Prefix
        },
        {
            RoleTypeId.Scp173, NicknamesConfig.Scp173Prefix
        },
        {
            RoleTypeId.Scp939, NicknamesConfig.Scp939Prefix
        },
        {
            RoleTypeId.Scp3114, NicknamesConfig.Scp3114Prefix
        }
    };

    public string[] HumanNames => NicknameData.Nicknames;

    
    public void OnPlayerChangeRole(ChangingRoleEventArgs ev)
    {
        if(!ev.NewRole.IsAlive()) return;
        /*if(!ev.NewRole.IsHuman()) return;*/
        if(ev.NewRole == RoleTypeId.Scp3114) return;
        string newName = ClassTitles[ev.NewRole];
            
        if (ev.NewRole == RoleTypeId.ClassD)
        {
            newName += Random.Range((int)1000, (int)9999);
        } else //if (ev.NewRole.IsHuman() || ev.NewRole==RoleTypeId.Scp0492)
        {
            newName += HumanNames[Random.Range(0, HumanNames.Length)];
        }
        /*else if(ev.NewRole.IsAlive())
        {
            newName = ClassTitles[ev.NewRole];
        }*/
        ev.Player.DisplayNickname = newName;
        ev.Player.ShowHUDHint("Your name is " + ev.Player.DisplayNickname, 10f);
    }
    
    public void OnPlayerDeath(DiedEventArgs ev)
    {
        ev.Player.DisplayNickname = null;
    }
    
    public void OnPressNoClip(TogglingNoClipEventArgs ev)
    {
        if(ev.Player.IsNoclipPermitted) return;
        ev.Player.ShowHUDHint("Your name is " + ev.Player.DisplayNickname, 10f);
    }

    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.ChangingRole += OnPlayerChangeRole;
        Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
        Exiled.Events.Handlers.Player.TogglingNoClip += OnPressNoClip;
        
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.ChangingRole -= OnPlayerChangeRole;
        Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
        Exiled.Events.Handlers.Player.TogglingNoClip -= OnPressNoClip;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Nicknames",
        Description = "Adds nicknames to players",
        Aliases = new []{"nick"},
        FormattedName = "<color=green>Nicknames</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = 0,
        Category = Category.Names
    };
    
    private static NicknameData _nicknameData;
    
    public static NicknameData NicknameData
    {
        get
        {
            if (_nicknameData == null)
            {
                _nicknameData = NicknameData.GetSaved();
            }

            return _nicknameData;
        }
    }

    public static Config NicknamesConfig => RoundModifiers.Instance.Config.Nicknames;

    public class Config : ModConfig
    {
        /*[Description("A list of nicknames to give to players.")]
        public string[] HumanNames { get; set; } = new[]
        {
            "Alan", "Steeve", "Mary", "John", "Alice", "Bob", "Carol", "Davis", "Eve", "Frank",
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
        };*/
        [Description("The prefix to add to each class' nickname.")]
        public string ClassDPrefix { get; set; } = "D-";
        public string ScientistPrefix { get; set; } = "Dr. ";
        public string FacilityGuardPrefix { get; set; } = "Officer ";
        public string NtfCaptainPrefix { get; set; } = "Captain ";
        public string NtfPrivatePrefix { get; set; } = "Private ";
        public string NtfSergeantPrefix { get; set; } = "Sergeant ";
        public string NtfSpecialistPrefix { get; set; } = "Agent ";
        public string ChaosConscriptPrefix { get; set; } = "Agent of Chaos ";
        public string ChaosMarauderPrefix { get; set; } = "Agent of Chaos ";
        public string ChaosRepressorPrefix { get; set; } = "Agent of Chaos ";
        public string ChaosRiflemanPrefix { get; set; } = "Agent of Chaos ";
        public string Scp049Prefix { get; set; } = "SCP-049-";
        public string Scp0492Prefix { get; set; } = "SCP-049-2-";
        public string Scp079Prefix { get; set; } = "SCP-079-";
        public string Scp096Prefix { get; set; } = "SCP-096-";
        public string Scp106Prefix { get; set; } = "SCP-106-";
        public string Scp173Prefix { get; set; } = "SCP-173-";
        public string Scp939Prefix { get; set; } = "SCP-939-";
        public string Scp3114Prefix { get; set; } = "SCP-3114-";
            
    }
}