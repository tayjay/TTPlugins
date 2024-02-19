using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class TattleAbility : Ability
    {
        private string leadingTeam, targetZone;
        public TattleAbility(int lifetime = 10) : base("Tattle", "", Exiled.API.Enums.Side.None, 5, lifetime)
        {
        }

        public override bool Setup()
        {
            RogueAI Handler = RoundModifiers.Instance.GetModifier<RogueAI>();
            Side leadingSide = Handler.GetLeadingSide();
            Dictionary<ZoneType,int> zoneCounts = new Dictionary<ZoneType, int>();
            zoneCounts[ZoneType.Surface] = 0;
            zoneCounts[ZoneType.Entrance] = 0;
            zoneCounts[ZoneType.HeavyContainment] = 0;
            zoneCounts[ZoneType.LightContainment] = 0;
            foreach(Player player in Player.List)
            {
                if (player.Role.Side == leadingSide)
                {
                    zoneCounts[player.CurrentRoom.Zone] += 1;
                }
            }
            KeyValuePair<ZoneType, int> max = new KeyValuePair<ZoneType, int>(ZoneType.Unspecified, 0);
            foreach (KeyValuePair<ZoneType, int> pair in zoneCounts)
            {
                if (pair.Value > max.Value)
                {
                    max = pair;
                }
            }
            if (max.Key == ZoneType.Unspecified)
            {
                return false;
            }
            switch(max.Key)
            {
                case ZoneType.LightContainment:
                    targetZone = "in .g5 Light Containment";
                    break;
                case ZoneType.HeavyContainment:
                    targetZone = "in .g5 Heavy Containment";
                    break;
                case ZoneType.Entrance:
                    targetZone = "in .g5 Entrance Zone";
                    break;
                case ZoneType.Surface:
                    targetZone = ".g5 on Surface";
                    break;
            }
            switch(leadingSide)
            {
                case Side.Scp:
                    leadingTeam = "SCP";
                    break;
                case Side.Mtf:
                    leadingTeam = "Foundation";
                    break;
                case Side.ChaosInsurgency:
                    leadingTeam = "Chaos Insurgency";
                    break;
            }
            return true;
        }
        
        public override void Start()
        {
            base.Start();
            Cassie.Message($"most .g1 {leadingTeam} are {targetZone}", true, true);
        }

        public override bool Update()
        {
            return true;
        }
    }
}