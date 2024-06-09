using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Respawning;
using RoundModifiers;
using RoundModifiers.API;
using Utils.NonAllocLINQ;

namespace RoundModifiers.Modifiers;

public class ExtraLife : Modifier
{

    // For the first 5-10 minutes of the round, when a player dies they are revived as their original role a few seconds later. Giving enough time for doctor to resurrect them.
    // This will allow SCPs to rack up kills and not have the round end too quickly.
    // There is still a reason for the SCPs to be involved though, as they still need to stop the players from escaping.

    public double MinimumRoundTime => RoundModifiers.Instance.Config.ExtraLife_MinimumRoundTime;
    public float RespawnDelay => RoundModifiers.Instance.Config.ExtraLife_RespawnDelay;
    public LeadingTeam EarlyWin { get; set; } = LeadingTeam.Draw;
    
    public int RespawnWave { get; set; } = 0;
    
    public List<uint> ToBeKilledPlayers { get; set; }

    public void OnDeath(DiedEventArgs ev)
    {
        if(Round.ElapsedTime.TotalSeconds > MinimumRoundTime) return;
        if(ToBeKilledPlayers.Contains(ev.Player.NetId))
        {
            ToBeKilledPlayers.Remove(ev.Player.NetId);
            if(ToBeKilledPlayers.Count == 0)
            {
                if(EarlyWin == LeadingTeam.Draw)
                    EarlyWin = LeadingTeam.Anomalies;
                else
                {
                    EarlyWin = LeadingTeam.Draw;
                }
            }
        }

        if (ev.TargetOldRole.GetTeam() == Team.SCPs && ev.Attacker!=null && ev.Attacker.Role.Team != Team.SCPs)
        {
            if (EarlyWin == LeadingTeam.Draw)
                EarlyWin = ev.Attacker.LeadingTeam;
            else
            {
                EarlyWin = LeadingTeam.Draw;
            }
        }
        
        Timing.CallDelayed(RespawnDelay, () =>
        {
            if(ev.Player.IsAlive) return;
            ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, RoleChangeReason.Respawn);
            ev.Player.EnableEffect(EffectType.SpawnProtected, 1, 15f);
            if(ev.TargetOldRole.GetTeam() == Team.SCPs)
            {
                ev.Player.Health = ev.Player.MaxHealth/2;
            }
        });
    }

    public void OnRoundStart()
    {
        foreach (Player player in Player.List)
        {
            if(player.Role.Team == Team.SCPs) continue;
            ToBeKilledPlayers.Add(player.NetId);
        }
    }

    
    public void OnRespawn(RespawningTeamEventArgs ev)
    {
        RespawnWave++;
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
            if(EarlyWin == ev.LeadingTeam) return;
            if (ev.LeadingTeam == LeadingTeam.Draw)
            {
                ev.LeadingTeam = EarlyWin;
            } else if(ev.LeadingTeam != LeadingTeam.Draw)
            {
                float foundationPoints = 0;
                float chaosPoints = 0;
                float scpPoints = 0;
                foundationPoints += RoundSummary.EscapedScientists;
                scpPoints += (float)RoundSummary.KilledBySCPs / (RespawnWave+1);
                scpPoints += RoundSummary.SurvivingSCPs;
                chaosPoints += RoundSummary.EscapedClassD;

                switch (EarlyWin)
                {
                    case LeadingTeam.Anomalies:
                        scpPoints += 5;
                        break;
                    case LeadingTeam.ChaosInsurgency:
                        chaosPoints += 5;
                        break;
                    case LeadingTeam.FacilityForces:
                        foundationPoints += 5;
                        break;
                }

                switch (ev.LeadingTeam)
                {
                    case LeadingTeam.Anomalies:
                        scpPoints += 5;
                        break;
                    case LeadingTeam.ChaosInsurgency:
                        chaosPoints += 5;
                        break;
                    case LeadingTeam.FacilityForces:
                        foundationPoints += 5;
                        break;
                }
                
                if (scpPoints > chaosPoints && scpPoints > foundationPoints)
                {
                    ev.LeadingTeam = LeadingTeam.Anomalies;
                } else if (chaosPoints > scpPoints && chaosPoints > foundationPoints)
                {
                    ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
                } else if (foundationPoints > scpPoints && foundationPoints > chaosPoints)
                {
                    ev.LeadingTeam = LeadingTeam.FacilityForces;
                }
                else
                {
                    ev.LeadingTeam = LeadingTeam.Draw;
                }





            }
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Died += OnDeath;
        Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawn;

        EarlyWin = LeadingTeam.Draw;
        RespawnWave = 0;
        ToBeKilledPlayers = ListPool<uint>.Pool.Get();
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Died -= OnDeath;
        Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawn;
        
        ListPool<uint>.Pool.Return(ToBeKilledPlayers);
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "ExtraLife",
        FormattedName = "<color=green><size=85%>Extra Life</size></color>",
        Aliases = new []{"extralife"},
        Description = "Players are revived after death for a short time",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 1,
        Category = Category.OnDeath
    };
}