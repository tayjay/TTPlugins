﻿using Exiled.API.Features;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class AliveXP : XP, IGameTickEvent
    {
        int tick = 0;
        public void OnGameTick()
        {
            tick++;
            if (tick % 1 == 0)
            {
                foreach (Player player in Player.List)
                {
                    if(player.IsAlive/* && !player.IsScp*/)
                        GiveXP(player, 1);
                }
            }
        }
        
        public override void Reset()
        {
            tick = 0;
        }
    }
}