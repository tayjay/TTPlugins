using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class Nicknames : Modifier
{
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
            RoleTypeId.NtfCaptain, "Commander "
        },
        {
            RoleTypeId.NtfPrivate, "Cadet "
        },
        {
            RoleTypeId.NtfSergeant, "Lieutenant "
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
        {
            RoleTypeId.Scp3114, "SCP-3114"
        }
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
        "Quentin", "Ruth", "Spencer", "Tiffany", "Uma", "Vincent", "Wallace", "Xena", "Yvette", "Zion"
    };
    
    public void OnPlayerChangeRole(ChangingRoleEventArgs ev)
    {
        if(!ev.NewRole.IsAlive()) return;
        ev.Player.DisplayNickname = ClassTitles[ev.NewRole];
        if (ev.NewRole == RoleTypeId.ClassD)
        {
            ev.Player.DisplayNickname += Random.Range((int)1000, (int)9999);
        } else if (ev.NewRole.IsHuman())
        {
            ev.Player.DisplayNickname += HumanNames[Random.Range(0, HumanNames.Length)];
        }
        ev.Player.ShowHUDHint("Your name is " + ev.Player.DisplayNickname, 10f);
    }
    
    public void OnPlayerDeath(DiedEventArgs ev)
    {
        ev.Player.DisplayNickname = null;
    }

    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.ChangingRole += OnPlayerChangeRole;
        Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.ChangingRole -= OnPlayerChangeRole;
        Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Nicknames",
        Description = "Adds nicknames to players",
        Aliases = new []{"nick"},
        FormattedName = "<color=green>Nicknames</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
}