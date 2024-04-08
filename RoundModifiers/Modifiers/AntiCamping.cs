using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Hazards;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp939;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
using TTCore.Extensions;
using UnityEngine;
using Scp939Role = Exiled.API.Features.Roles.Scp939Role;

namespace RoundModifiers.Modifiers;

public class AntiCamping : Modifier
{
    CoroutineHandle _scp939Tick, _tantrumTick;
    public void OnRoundStart()
    {
        _scp939Tick = Timing.RunCoroutine(OnScp939Tick());
        _tantrumTick = Timing.RunCoroutine(OnTantrumTick());
    }
    
    //------------Scp049
    //Give Scp049-02 ability to chip away at doors to stop people from camping in rooms for long
    
    //Check zombie hitting door
    public void OnZombieAttack(ZombieAttackEventArgs ev)
    {
        //Check if looking at door
        if(!ev.IsAllowed) return;
        //if(!ev.Attacker.TryGetDoorOnSight(1f, out var door)) return;
        Door closest = Door.GetClosest(ev.Attacker.Position, out float distance);
        if(distance > 2f) return;
        if (closest is IDamageableDoor damageableDoor)
        {
            damageableDoor.Damage(1f);
            ev.Attacker.ShowHitMarker();
        }
    }
    
    
    //-----------Scp173
    //Give DoT ticks to tantrum when placed under a door.
    
    //Check placing tantrum, then check over time
    public void OnPlaceTantrum(PlacingTantrumEventArgs ev)
    {
        
    }
    
    public IEnumerator<float> OnTantrumTick()
    {
        while (true)
        {
            foreach (var hazard in TantrumHazard.List)
            {
                var tantrumHazard = (TantrumHazard)hazard;
                Door closest = Door.GetClosest(tantrumHazard.Position, out float distance);
                Log.Debug(distance);
                if (tantrumHazard.Base.IsInArea(tantrumHazard.Position, closest.Position+Vector3.up))
                {
                    if(closest is IDamageableDoor damageableDoor)
                    {
                        if(damageableDoor.IsDestroyed) continue;
                        damageableDoor.Damage(10f);
                        foreach (Player p in Player.Get(RoleTypeId.Scp173))
                        {
                            p.ShowHitMarker();
                        }
                    }
                }
                yield return Timing.WaitForOneFrame;
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }
    
    
    //-------------Scp939
    //Pouncing at a door a few times will eventually open it
    
    //Check hitting a door when pouncing
    public void OnLunge(LungingEventArgs ev)
    {
        if(!ev.Player.GameObject.TryGetComponent<Collider>(out var collider)) return;
        
    }

    public IEnumerator<float> OnScp939Tick()
    {
        while (true)
        {
            foreach (Player player in Player.List.Where(p => p.Role.Type == RoleTypeId.Scp939))
            {
                Scp939Role scp939 = (Scp939Role)player.Role;
                Log.Debug(scp939.LungeState);
                if(scp939.LungeState!=Scp939LungeState.LandHit) continue;
                Door closest = Door.GetClosest(player.Position, out float distance);
                if(distance > 2f) continue;
                if (closest is IDamageableDoor damageableDoor)
                {
                    damageableDoor.Damage(1000f);
                    player.ShowHitMarker();
                    Log.Debug("Damaged door");
                }

                yield return Timing.WaitForOneFrame;
            }

            yield return Timing.WaitForSeconds(0.1f);
        }
    }
    
    
    protected override void RegisterModifier()
    {
        TTCore.Events.Handlers.Custom.ZombieAttack += OnZombieAttack;
        Exiled.Events.Handlers.Scp173.PlacingTantrum += OnPlaceTantrum;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Scp939.Lunging += OnLunge;
    }

    protected override void UnregisterModifier()
    {
        TTCore.Events.Handlers.Custom.ZombieAttack -= OnZombieAttack;
        Exiled.Events.Handlers.Scp173.PlacingTantrum -= OnPlaceTantrum;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Scp939.Lunging -= OnLunge;
        
        Timing.KillCoroutines(_scp939Tick);
        Timing.KillCoroutines(_tantrumTick);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "AntiCamping",
        Description = "Prevent players from camping",
        Aliases = new []{"camping"},
        FormattedName = "Anti Camping",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
    
    
    //Scp079
    //Already has ability to unlock doors
    
    //Scp096
    //Can already break through doors when enraged
    
    //Scp106
    //Can already walk through doors
}