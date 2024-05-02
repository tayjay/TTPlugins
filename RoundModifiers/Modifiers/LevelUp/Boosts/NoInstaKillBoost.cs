using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RoundModifiers.Modifiers.LevelUp.Interfaces;
using UnityEngine;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    /**
     * When a player levels up, they will not be able to be insta-killed by any means.
     * This includes SCP-049's melee attack, SCP-096's rage, grenades, and SCP-173's snap.
     * Anything that does damage ticks prior to lowering health to 0 will still work.
     */
    public class NoInstaKillBoost : Boost, IHurtingEvent, IHurtEvent, IDyingEvent
    {
        private Dictionary<Player,float> _lastHit = new Dictionary<Player, float>();
        
        private const float VaulnerabilityTime = 20f;

        public NoInstaKillBoost(Tier tier) : base(tier)
        {
        }

        public override bool AssignBoost(Player player)
        {
            if(player.IsScp) return false;
            if(HasBoost.ContainsKey(player.NetId)) return false;
            HasBoost.Add(player.NetId, true);
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            _lastHit.Add(player, Time.time);
            return true;
        }

        public override string GetName()
        {
            return "No Insta-Kill";
        }

        public override string GetDescription()
        {
            return "You cannot be insta-killed.";
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            // If the player has the boost, and the damage is enough to kill them, cancel the event and set hp to 1

            if (_lastHit.ContainsKey(ev.Player))
            {
                if (Time.time - _lastHit[ev.Player] > VaulnerabilityTime || ev.Player.Health > 1)
                {
                    //Want to save the player from dying
                    if (ev.Amount >= ev.Player.Health)
                    {
                        ev.Amount = ev.Player.Health - 1;
                    }
                }
            }
        }

        public void OnHurt(HurtEventArgs ev)
        {
            //Update last hit timer
            if (_lastHit.ContainsKey(ev.Player))
            {
                Timing.CallDelayed(Timing.WaitForOneFrame, () => _lastHit[ev.Player] = Time.time);
            }
        }
        
        public override void Reset()
        {
            _lastHit.Clear();
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (_lastHit.ContainsKey(ev.Player))
            {
                if(Time.time - _lastHit[ev.Player] < VaulnerabilityTime)
                {
                    ev.IsAllowed = true;
                }
                else
                {
                    ev.IsAllowed = false;
                    _lastHit[ev.Player] = Time.time;
                }
            }
                
        }
    }
}