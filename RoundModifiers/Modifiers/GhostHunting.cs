using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class GhostHunting : Modifier
{
    public bool IsHuntActive { get; set; } = false;

    public void ChoosingTeams(ChoosingStartTeamQueueEventArgs ev)
    {
        // Change team selection. All SCPs stay but any other role is replaced with FoundationForces
        List<Team> newList = new List<Team>();
        foreach (var team in ev.TeamRespawnQueue)
        {
            if (team == Team.SCPs)
            {
                newList.Add(team);
            }
            else
            {
                newList.Add(Team.FoundationForces);
            }
        }
        ev.TeamRespawnQueue.Clear();
        ev.TeamRespawnQueue.AddRange(newList);
    }
    
    public void OnRoundStart()
    {
        HideScps();
    }

    public void OnJoin(JoinedEventArgs ev)
    {
        List<Player> scps = Player.List.Where(p => p.Role.Team == Team.SCPs).ToList();
        foreach (var scp in scps)
        {
            scp.ChangeAppearance(RoleTypeId.Filmmaker, Player.List.Where(player => player == ev.Player));
        }
    }

    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if(ev.Player.Role.Team == Team.SCPs)
        {
            if(RoleExtensions.GetTeam(ev.Player.Role) != Team.SCPs)
            {
                List<Player> scps = Player.List.Where(p => p.Role.Team == Team.SCPs && p!=ev.Player).ToList();
                foreach (var scp in scps)
                {
                    scp.ChangeAppearance(RoleTypeId.Filmmaker, Player.List.Where(player => player == ev.Player));
                }
            }
        }

        if (ev.NewRole == RoleTypeId.FacilityGuard)
        {
            ev.NewRole = RoleTypeId.NtfSergeant;
        }
    }

    public void HideScps()
    {
        List<Player> scps = Player.List.Where(p => p.Role.Team == Team.SCPs).ToList();
        foreach (var scp in scps)
        {
            scp.ChangeAppearance(RoleTypeId.Filmmaker, Player.List.Where(player => player.Role.Team!=Team.SCPs));
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.Joined += OnJoin;
        Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
        Exiled.Events.Handlers.Server.ChoosingStartTeamQueue += ChoosingTeams;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.Joined -= OnJoin;
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
        Exiled.Events.Handlers.Server.ChoosingStartTeamQueue -= ChoosingTeams;
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "GhostHunting",
        Aliases = new [] {"ghosthunt", "gh"},
        Description = "SCPs are ghosts, humans must hunt them down",
        FormattedName = "<color=red><size=85%>Ghost Hunting</size></color>",
        Impact = ImpactLevel.Gamemode,
        Balance = -2,
        MustPreload = false,
        Category = Category.Gamemode | Category.HumanRole | Category.ScpRole
    };
}