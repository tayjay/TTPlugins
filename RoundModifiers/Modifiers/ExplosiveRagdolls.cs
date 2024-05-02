using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using RoundModifiers.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoundModifiers.Modifiers;

public class ExplosiveRagdolls : Modifier
{

    CoroutineHandle _ragdollTick;
    private List<Ragdoll> explodedRagdolls { get; set; }
    
    
    
    public void OnSpawnRagdoll(SpawnedRagdollEventArgs ev)
    {
        if (ev.Ragdoll == null) return;
        var pickup = Pickup.CreateAndSpawn(ItemType.GrenadeHE, ev.Position+Vector3.up, Quaternion.identity);
        if (pickup is GrenadePickup grenadePickup)
        {
            grenadePickup.FuseTime *= Random.Range(2f, 5f);
            grenadePickup.Explode();
        }
        Log.Debug("Boom at " + ev.Position);
    }

    public IEnumerator<float> RagdollTick()
    {
        while (true)
        {
            Log.Debug("Checking ragdolls");
            if (Ragdoll.List == null)
            {
                yield return Timing.WaitForSeconds(1f);
                continue;
            }
            if (Ragdoll.List.Count == 0)
            {
                yield return Timing.WaitForSeconds(1f);
                continue;
            }
            foreach (Ragdoll ragdoll in Ragdoll.List)
            {
                try
                {
                    if (ragdoll == null) continue;
                    if(explodedRagdolls.Contains(ragdoll)) continue;
                    Log.Debug(ragdoll.ExistenceTime);
                    if (ragdoll.ExistenceTime > 5)
                    {
                        try
                        {
                            explodedRagdolls.Add(ragdoll);
                            //Map.Explode(ragdoll.Position, ProjectileType.FragGrenade);
                            
                        }
                        catch (Exception e)
                        {
                            Log.Error(e);
                        }
                    }
                } catch {}
            }
            Log.Debug("Done checking ragdolls");
            yield return Timing.WaitForSeconds(1f);
        }
    }

    public void OnRoundStart()
    {
        explodedRagdolls.Clear();
        //_ragdollTick = Timing.RunCoroutine(RagdollTick());
    }

    public void OnRoundEnded(RoundEndedEventArgs ev)
    {
        foreach (Ragdoll ragdoll in Ragdoll.List)
        {
            try
            {
                var pickup = Pickup.CreateAndSpawn(ItemType.GrenadeHE, ragdoll.Position+Vector3.up, Quaternion.identity);
                if (pickup is GrenadePickup grenadePickup)
                {
                    grenadePickup.FuseTime *= Random.Range(0.2f, 0.5f);
                    grenadePickup.Explode();
                }
            } catch {}
        }
    }
    
    public void OnHurting(HurtingEventArgs ev)
    {
        if (ev.Player == null) return;
        
        //Stop 049 from taking explosive damage from ragdolls
        if (ev.Player.Role == RoleTypeId.Scp049 && ev.DamageHandler.Type == DamageType.Explosion)
        {
            if(ev.Attacker != null) return;
            ev.Amount = 0;
            ev.IsAllowed = false;
        }
    }
    
    protected override void RegisterModifier()
    {
        //Exiled.Events.Handlers.Player.SpawningRagdoll += OnSpawnRagdoll;
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Player.SpawnedRagdoll += OnSpawnRagdoll;
        explodedRagdolls = Exiled.API.Features.Pools.ListPool<Ragdoll>.Pool.Get();
    }

    protected override void UnregisterModifier()
    {
        //Exiled.Events.Handlers.Player.SpawningRagdoll -= OnSpawnRagdoll;
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Player.SpawnedRagdoll -= OnSpawnRagdoll;
        Exiled.API.Features.Pools.ListPool<Ragdoll>.Pool.Return(explodedRagdolls);
        Timing.KillCoroutines(_ragdollTick);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "ExplosiveRagdolls",
        Aliases = new []{"boom", "gruntbdayparty"},
        Description = "Ragdolls explode after 10 seconds",
        FormattedName = "Birthday Party",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false
    };
}