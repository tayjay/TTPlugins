using Exiled.API.Features;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface ILevelUpEvent
    {
        void OnLevelUp(Player player);
    }
}