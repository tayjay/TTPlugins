using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using TTCore.API;


namespace TTAddons.Handlers
{
    public class CombatLogHandler : IRegistered
    {
        public Dictionary<Player, List<CombatLog>> CombatLogs { get; private set;}

        public void OnPlayerHurt(HurtEventArgs ev)
        {
            CombatLog log = new CombatLog
            {
                Attacker = ev.Attacker?.Nickname ?? "N/A",
                AttackerHealth = ev.Attacker?.Health ?? 0,
                Target = ev.Player.Nickname,
                TargetHealth = ev.Player.Health,
                Damage = ev.Amount,
                DamageType = ev.DamageHandler.Type.ToString(),
                Time = Round.ElapsedTime.TotalSeconds
            };
            if (!CombatLogs.ContainsKey(ev.Player))
            {
                CombatLogs.Add(ev.Player, new List<CombatLog>());
            }
            CombatLogs[ev.Player].Add(log);
            
            
            if (ev.Attacker != null && ev.Attacker != ev.Player)
            {
                if(!CombatLogs.ContainsKey(ev.Attacker))
                {
                    CombatLogs.Add(ev.Attacker, new List<CombatLog>());
                    CombatLogs[ev.Attacker].Add(log);
                }
            }
            
        }
        
        public void OnPlayerDeath(DiedEventArgs ev)
        {
            
        }

        public void OnPlayerSpawn(SpawningEventArgs ev)
        {
            if (ev.Player.IsDead) return;
            //Clear the combat log for the player
        }
        
        public void OnRoundRestart()
        {
            //Clear the combat log
            CombatLogs.Clear();
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Player.Hurt += OnPlayerHurt;
            Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
            Exiled.Events.Handlers.Player.Spawning += OnPlayerSpawn;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
            CombatLogs = DictionaryPool<Player, List<CombatLog>>.Pool.Get();
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Player.Hurt -= OnPlayerHurt;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            Exiled.Events.Handlers.Player.Spawning -= OnPlayerSpawn;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            DictionaryPool<Player, List<CombatLog>>.Pool.Return(CombatLogs);
        }

        public struct CombatLog
        {
            public string Attacker;
            public float AttackerHealth;
            public string Target;
            public float TargetHealth;
            public float Damage;
            public string DamageType;
            public double Time;
        }
    }
}