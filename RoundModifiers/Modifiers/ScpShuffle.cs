using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class ScpShuffle : Modifier
{
    

    public void OnRespawnWave(RespawningTeamEventArgs ev)
    {
        if(!ev.IsAllowed) return;
        /*Timing.CallDelayed(2f, () =>
        {
            foreach (var scp in Player.List.Where(p => p.Role.Team == Team.SCPs))
            {
                if(scp.Role.Type==RoleTypeId.Scp079) continue;
                RoleTypeId[] scps = {RoleTypeId.Scp049,RoleTypeId.Scp0492, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939, RoleTypeId.Scp3114};
                RoleTypeId newRole = scps[UnityEngine.Random.Range(0, scps.Length)];
                float oldHealth = scp.Health;
                float oldMaxHealth = scp.MaxHealth;
                scp.RoleManager.ServerSetRole(newRole,RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
                //Set health to be the same percentage of max health as before
                scp.Health = oldHealth / oldMaxHealth * scp.MaxHealth;
            }
        } );*/
        Timing.RunCoroutine(ChangeSCPs());
    }

    public IEnumerator<float> ChangeSCPs()
    {
        yield return Timing.WaitForSeconds(1f);
        RoleTypeId[] scps = {RoleTypeId.Scp049,/*RoleTypeId.Scp0492,*/ RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939, RoleTypeId.Scp3114};
        foreach (var scp in Player.List.Where(p => p.Role.Team == Team.SCPs))
        {
            yield return Timing.WaitForOneFrame;
            if(scp.Role.Type==RoleTypeId.Scp079) continue;
            if(scp.Role.Type==RoleTypeId.Scp0492) continue; //Temp removing 049-2 from shuffle
            
            RoleTypeId newRole = scps.GetRandomValue(id => id != scp.Role.Type);
            float oldHealth = scp.Health;
            float oldMaxHealth = scp.MaxHealth;
            scp.RoleManager.ServerSetRole(newRole,RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
            //Set health to be the same percentage of max health as before
            yield return Timing.WaitForOneFrame;
            scp.Health = oldHealth / oldMaxHealth * scp.MaxHealth;
        }
    }
    
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnWave;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "ScpShuffle",
        FormattedName = "<color=red><size=85%>Scp Shuffle</size></color>",
        Aliases = new []{"shuffle"},
        Description = "All SCPs are shuffled",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = -2,
        Category = Category.ScpRole
    };
}