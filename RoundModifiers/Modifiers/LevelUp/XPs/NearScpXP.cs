using System.Linq;
using Exiled.API.Features;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs;

public class NearScpXP : XP, IGameTickEvent
{
    public void OnGameTick()
    {
        foreach(Player scpPlayer in Player.List.Where(p => p.IsScp))
        {
            foreach(Player player in scpPlayer.CurrentRoom.Players.Where(p=>!p.IsScp))
            {
                GiveXP(player, 2);
            }
        }
    }
}