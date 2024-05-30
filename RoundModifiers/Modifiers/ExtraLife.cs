using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using RoundModifiers;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class ExtraLife : Modifier
{

    // For the first 5-10 minutes of the round, when a player dies they are revived as their original role a few seconds later. Giving enough time for doctor to resurrect them.
    // This will allow SCPs to rack up kills and not have the round end too quickly.
    // There is still a reason for the SCPs to be involved though, as they still need to stop the players from escaping.

    public double MinimumRoundTime => RoundModifiers.Instance.Config.ExtraLife_MinimumRoundTime;
    public float RespawnDelay => RoundModifiers.Instance.Config.ExtraLife_RespawnDelay;
    public LeadingTeam EarlyWin { get; set; } = LeadingTeam.Draw;

    public void OnDeath(DiedEventArgs ev)
    {
        if(Round.ElapsedTime.TotalSeconds > MinimumRoundTime) return;
        
        Timing.CallDelayed(RespawnDelay, () =>
        {
            if(ev.Player.IsAlive) return;
            ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, RoleChangeReason.Respawn);
        });
    }

    public void OnEndingRound(EndingRoundEventArgs ev)
    {
        if (Round.ElapsedTime.TotalSeconds <= MinimumRoundTime)
        {
            if (ev.IsForceEnded) return;
            if (!ev.IsRoundEnded) return;
            EarlyWin = ev.LeadingTeam;
            ev.IsAllowed = false;
            Respawn.ForceWave(Respawn.NextKnownTeam, true);
        }
        else
        {
            if(EarlyWin == LeadingTeam.Draw) return;
            if (ev.LeadingTeam == LeadingTeam.Draw)
            {
                ev.LeadingTeam = EarlyWin;
            } else if(ev.LeadingTeam != LeadingTeam.Draw)
            {
                ev.LeadingTeam = LeadingTeam.Draw;
            }
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Died += OnDeath;
        Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;

        EarlyWin = LeadingTeam.Draw;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Died -= OnDeath;
        Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "ExtraLife",
        FormattedName = "<color=green><size=85%>Extra Life</size></color>",
        Aliases = new []{"extralife"},
        Description = "Players are revived after death for a short time",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 1
    };
}