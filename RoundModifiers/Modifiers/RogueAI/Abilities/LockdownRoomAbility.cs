using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using UnityEngine;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class LockdownRoomAbility : Ability
    {
        private Room room;
        private Color color;
        private int lockdownTime;
        public LockdownRoomAbility(Side helpingSide = Side.Scp) : base("Lockdown", "Closes and locks all doors in a room", helpingSide, 8)
        {
            Lifetime = 10;
        }

        public override bool Setup()
        {
            Dictionary<Room,int> playerCounts = new Dictionary<Room, int>();
            Side leadingSide =
                (RoundModifiers.Instance.GetModifier<RogueAI>())
                .GetLeadingSide();
            foreach(Player player in Player.List)
            {
                if(player.Role.Side == leadingSide)
                {
                    if (playerCounts.ContainsKey(player.CurrentRoom))
                    {
                        playerCounts[player.CurrentRoom]++;
                    }
                    else
                    {
                        playerCounts[player.CurrentRoom] = 1;
                    }
                }
            }
            //Get the Room with the most players
            KeyValuePair<Room, int> max = new KeyValuePair<Room, int>(null, 0);
            foreach (KeyValuePair<Room, int> pair in playerCounts)
            {
                if (pair.Value > max.Value)
                {
                    max = pair;
                }
            }
            if (max.Key == null)
            {
                return false;
            }
            this.room = max.Key;
            this.color = room.RoomLightController.NetworkOverrideColor;
            return true;
        }

        public override void Start()
        {
            base.Start();
            lockdownTime = 0;
            
            foreach (Door door in room.Doors)
            {
                door.IsOpen = false;
                door.Lock(10, DoorLockType.Lockdown079);
                if(HelpingSide != Side.Scp)
                    door.AllowsScp106 = false;
            }
            room.RoomLightController.NetworkOverrideColor = new Color(2f, 0f, 0f);
        }

        public override bool Update()
        {
            if (lockdownTime++ == 0)
            {
                
            } else if (lockdownTime >= 10)
            {
                
                return false;
            }

            return true;
        }

        public override void End()
        {
            base.End();
            room.RoomLightController.NetworkOverrideColor = color;
            foreach (Door door in room.Doors)
            {
                door.Unlock();
                if(HelpingSide != Side.Scp)
                    door.AllowsScp106 = true;
            }
        }
    }
}