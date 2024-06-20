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
            RoleTypeId.Scp049, "The Doctor"
        },
        {
            RoleTypeId.Scp0492, "Zombie"
        },
        {
            RoleTypeId.Scp079, "Computer"
        },
        {
            RoleTypeId.Scp096, "Shy Guy"
        },
        {
            RoleTypeId.Scp106, "Larry"
        },
        {
            RoleTypeId.Scp173, "Peanut"
        },
        {
            RoleTypeId.Scp939, "Dog"
        },
        {
            RoleTypeId.Scp3114, "Skeleton"
        }
    };

    public string[] HumanNames => RoundModifiers.Instance.Config.Nicknames_HumanNames;

    
    public void OnPlayerChangeRole(ChangingRoleEventArgs ev)
    {
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
        else if(ev.NewRole.IsAlive())
        {
            newName = ClassTitles[ev.NewRole];
        }
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
}