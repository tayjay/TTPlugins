using Exiled.API.Enums;
using Exiled.API.Features;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class ElevatorAbility : Ability
    {
        public Lift lift;
        public ElevatorAbility(int lifetime = 10) : base("Elevator", "Move an elevator", Side.None, 1, lifetime)
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
            lift.TryStart((lift.CurrentLevel!=1)?1:0, true);
            
        }
        public override bool Update()
        {
            return true;
        }
    }
}