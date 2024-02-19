using Exiled.API.Enums;
using Exiled.API.Features;
using Scp914;
using UnityEngine;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class Scp914Ability : Ability
    {
        public Scp914Ability() : base("Interact Scp914", "", Side.None, 1, 10)
        {
        }

        public override bool Setup()
        {
            foreach (Player player in Player.List)
            {
                if (player.Zone == ZoneType.LightContainment)
                    return true;
            }

            return false;
        }

        public override void Start()
        {
            base.Start();
            int aggression = Random.Range(0, 6);
            switch(aggression)
            {
                case 0:
                case 1:
                case 2:
                    Exiled.API.Features.Scp914.Start();
                    break;
                case 3:
                    Exiled.API.Features.Scp914.KnobStatus = Scp914KnobSetting.Fine;
                    break;
                case 4:
                    Exiled.API.Features.Scp914.KnobStatus = Scp914KnobSetting.VeryFine;
                    break;
                case 5:
                    Exiled.API.Features.Scp914.KnobStatus = Scp914KnobSetting.Rough;
                    break;
            }
            
        }

        public override bool Update()
        {
            return true;
        }
    }
}