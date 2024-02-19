using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using UnityEngine;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class ChangeLightAbility : Ability
    {
        public Player target;
        public Room targetRoom;
        public Color RoomColor;
        public ChangeLightAbility(int lifetime = 10) : base("Change Lights", "", Exiled.API.Enums.Side.None, 0, lifetime)
        {
            target = null;
            targetRoom = null;
            RoomColor = Color.gray;
        }

        public override bool Setup()
        {
            RogueAI handler = RoundModifiers.Instance.GetModifier<RogueAI>();
            if (handler == null)
            {
                Log.Error("RougeAIHandler is null");
                return false;
            }

            if (handler.CurrentSide == Exiled.API.Enums.Side.None)
            {
                target = Player.List.GetRandomValue();
            }
            else
            {
                target = Player.Get(handler.CurrentSide).GetRandomValue();
            }
            
            if (target == null)
            {
                Log.Error("Target is null");
                return false;
            }

            if (target.CurrentRoom == null)
            {
                Log.Error("Target room is null");
                return false;
            }
            targetRoom = target.CurrentRoom;
            RoomColor = targetRoom.Color;
            Log.Info("Setting up ability for " + target.Nickname + " to change lights.");
            return true;
        }

        public override void Start()
        {
            base.Start();
            Color color = Color.magenta;
            switch (target.Role.Side)
            {
                case Side.Mtf:
                    color = Color.blue;
                    break;
                case Side.ChaosInsurgency:
                    color = Color.green;
                    break;
                case Side.Scp:
                    color = Color.red;
                    break;
            }

            target.CurrentRoom.Color = color;
        }
        public override bool Update()
        {
            return true;
        }
        
        public override void End()
        {
            base.End();
            targetRoom.Color = RoomColor;
        }
    }
}