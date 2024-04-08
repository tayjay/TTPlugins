using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Doors;
using PluginAPI.Core;

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
            door = Exiled.API.Features.Player.List.GetRandomValue()?.CurrentRoom.Doors.GetRandomValue();
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