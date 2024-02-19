using Exiled.API.Enums;
using Exiled.API.Features;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class BlackoutAbility : Ability
    {
        public BlackoutAbility(Side helpingSide = Side.None, int aggressionLevel = 10) : base("Blackout", "Turns off the lights", helpingSide, aggressionLevel, lifetime: 10)
        {
        }
        
        public override bool Setup()
        {
            RogueAI Handler = RoundModifiers.Instance.GetModifier<RogueAI>();
            if (HelpingSide == Side.Scp)
            {
                if(Handler.CurrentSide == Side.Scp)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public override void Start()
        {
            base.Start();
            Map.TurnOffAllLights(10);
        }

        public override bool Update()
        {
            return true;
        }
    }
}