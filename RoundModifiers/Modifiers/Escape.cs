using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class Escape : Modifier
    {
        
        public Dictionary<Side,int> EscapeTickets { get; set; }
        public Vector3 EscapePosition { get; set; }
        
        public void OnRoundStart()
        {
            EscapeTickets = new Dictionary<Side, int>();
            EscapeTickets.Add(Side.ChaosInsurgency, 0);
            EscapeTickets.Add(Side.Mtf, 0);
            EscapeTickets.Add(Side.Scp, 0);

        }

        public IEnumerator<float> CheckCustomEscape()
        {
            while (true)
            {
                foreach (Player player in Player.List.Where(p => p.Zone == ZoneType.Surface))
                {
                    if (player.Role.Type == RoleTypeId.ClassD || player.Role.Type == RoleTypeId.Scientist)
                        continue;//These players will escape through the normal escape
                    if(Vector3.Distance(EscapePosition, player.Position) > 5f)
                        continue;//too far away
                    if (player.IsCuffed)
                    {
                        //Reversed escape logic
                        if (player.Role.Team == Team.ChaosInsurgency)
                        {
                            EscapeTickets[Side.Mtf] += 1;
                            player.RoleManager.ServerSetRole(RoleTypeId.NtfPrivate,RoleChangeReason.Escaped);
                            continue;
                        } else if (player.Role.Team == Team.FoundationForces)
                        {
                            EscapeTickets[Side.ChaosInsurgency] += 1;
                            player.RoleManager.ServerSetRole(RoleTypeId.ChaosConscript,RoleChangeReason.Escaped);
                            continue;
                        }
                    }
                    else
                    {
                        if (player.Role.Type == RoleTypeId.FacilityGuard || player.Role.Type == RoleTypeId.NtfPrivate)
                        {
                            if (EscapeTickets[Side.Mtf] <= 0) continue;
                            EscapeTickets[Side.Mtf] -= 1;
                            player.RoleManager.ServerSetRole(RoleTypeId.NtfSergeant,RoleChangeReason.Escaped);
                        }
                    }
                    
                    
                    
                        
                }
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            switch (ev.EscapeScenario)
            {
                case EscapeScenario.Scientist:
                case EscapeScenario.CuffedClassD:
                    EscapeTickets[Side.Mtf] += 1;
                    break;
                case EscapeScenario.CuffedScientist:
                case EscapeScenario.ClassD:
                    EscapeTickets[Side.ChaosInsurgency] += 1;
                    break;
            }
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.Escaping += OnEscape;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.Escaping -= OnEscape;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "Escape",
            FormattedName = "<color=green>Escape</color>",
            Description = "Allows all players to escape the facility.",
            Impact = ImpactLevel.MajorGameplay
        };
    }
}