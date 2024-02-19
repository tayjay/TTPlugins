using Exiled.API.Enums;
using Exiled.API.Features;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class ElevatorAbility : Ability
    {
        public Lift lift;
        public ElevatorAbility(string name, string description, Side helpingSide, int aggressionLevel, int lifetime = 10) : base(name, description, helpingSide, aggressionLevel, lifetime)
        {
        }

        public override bool Setup()
        {
            lift = Lift.Random;
            if(lift == null) return false;
            return true;
        }

        public override void Start()
        {
            base.Start();
            int destination = 0;
            
        }
        public override bool Update()
        {
            return true;
        }
    }
}