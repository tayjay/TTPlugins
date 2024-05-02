using Exiled.API.Features;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public abstract class ScpXP : XP
{
    protected override bool CanGiveXP(Player player)
    {
        return player.IsScp;
    }
}