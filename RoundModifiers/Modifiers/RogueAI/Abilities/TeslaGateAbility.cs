using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class TeslaGateAbility : Ability
    {
        public bool ShouldDisable { get; set; }
        public Exiled.API.Features.TeslaGate Gate { get; set; }
        public Color OriginalRoomColor { get; set; }
        
        public TeslaGateAbility(Side helpingSide = Side.Scp) : base("Tesla Gate", "Activates or deactivates a tesla gate", helpingSide, 8, 10)
        {
        }

        public override bool Setup()
        {
            //Check for if there are players in a room with a tesla gate
            foreach(Room room in Room.List)
            {
                if (room.TeslaGate != null)
                {
                    if (HelpingSide == Side.Scp)
                    {
                        if (room.Players.Count() < 1)
                        {
                            Gate = room.TeslaGate;
                            return true;
                        }
                    }
                    else
                    {
                        // if not helping SCPS, let it touch them with no one nearby
                        Gate = room.TeslaGate;
                        return true;
                    }
                    
                }
            }
            return false;
        }


        public override void Start()
        {
            base.Start();
            OriginalRoomColor = Gate.Room.RoomLightController.NetworkOverrideColor;
            if (ShouldDisable)
            {
                //Set room lights to Green to show it's safe
                Gate.Room.RoomLightController.NetworkOverrideColor = new Color(0f, 2f, 0f);
                Gate.CooldownTime = Lifetime * 60;
            }
            else
            {
                //Set room lights to Red to show it's dangerous
                Gate.Room.RoomLightController.NetworkOverrideColor = new Color(2f, 0f, 0f);
            }
                
        }

        public override bool Update()
        {
            if (!ShouldDisable)
            {
                foreach (Player player in Gate.Room.Players)
                {
                    if (Gate.IsPlayerInHurtRange(player))
                    {
                        Gate.ForceTrigger();
                        return false;
                        //Only want to kill one person
                    }
                }
            }

            if (HelpingSide != Side.Scp)
            {
                if (UnityEngine.Random.Range(0, 1) < 0.2)
                {
                    Gate.ForceTrigger();
                    return false;
                }
            }
            return true;
        }
        
        public override void End()
        {
            base.End();
            //Set lights back to normal color
            Gate.Room.RoomLightController.NetworkOverrideColor = OriginalRoomColor;
        }
    }
}