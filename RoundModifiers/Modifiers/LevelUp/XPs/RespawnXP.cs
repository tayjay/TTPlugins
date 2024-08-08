using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;
using UnityEngine;

namespace RoundModifiers.Modifiers.LevelUp.XPs;

public class RespawnXP : XP, IDiedEvent, ISpawnedEvent
{
    public Dictionary<Player, float> DeathTimes { get; set; } = DictionaryPool<Player, float>.Pool.Get();
    
    
    public void OnSpawned(SpawnedEventArgs ev)
    {
        if(DeathTimes.TryGetValue(ev.Player, out float time))
        {
            GiveXP(ev.Player, (Time.time-time)/3);
        }
        DeathTimes.Remove(ev.Player);
    }
    
    public void OnDied(DiedEventArgs ev)
    {
        if(ev.Attacker == null) return;

        DeathTimes[ev.Player] = Time.time;
    }

    public override void Reset()
    {
        base.Reset();
        DictionaryPool<Player, float>.Pool.Return(DeathTimes);
    }
}