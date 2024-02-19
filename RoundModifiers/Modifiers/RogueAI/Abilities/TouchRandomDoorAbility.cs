using Exiled.API.Enums;
using Exiled.API.Features.Doors;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class TouchRandomDoorAbility : Ability
    {
        public Door door;
        public TouchRandomDoorAbility() : base("Touch Random Door", "", Side.None, 2)
        {
        }

        public override bool Setup()
        {
            door = Door.Random();
            if(door == null) return false;
            if(!door.AllowsScp106) return false;
            return true;
        }

        public override void Start()
        {
            base.Start();
            door.IsOpen = !door.IsOpen;
        }

        public override void End()
        {
            base.End();
            door = null;
        }

        public override bool Update()
        {
            return true;
        }
    }
}