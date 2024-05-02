using Exiled.API.Features;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public abstract class XP
    {
        protected static LevelUp LevelUpHandler => RoundModifiers.Instance.GetModifier<LevelUp>();
        
        protected virtual bool CanGiveXP(Player player)
        {
            return true;
        }
        
        protected void GiveXP(Player player, float amount)
        {
            if (CanGiveXP(player))
            {
                LevelUpHandler.PlayerXP[player.NetId] += amount;
                TryLevelUp(player);
            }
        }

        protected static void TryLevelUp(Player player)
        {
            //if(player.IsScp) return;
            float xp = LevelUpHandler.PlayerXP[player.NetId];
            float level = LevelUpHandler.PlayerLevel[player.NetId];
            float xpNeeded = GetXPNeeded(level);

            while (xp >= GetXPNeeded(level))
            {
                LevelUpHandler.PlayerXP[player.NetId] -= xpNeeded;
                LevelUpHandler.PlayerLevel[player.NetId] += 1;
                LevelUpHandler.OnLevelUp(player);
                xp = LevelUpHandler.PlayerXP[player.NetId];
                level = LevelUpHandler.PlayerLevel[player.NetId];
            }
        }
        
        public static float GetXPNeeded(float level)
        {
            return RoundModifiers.Instance.Config.LevelUp_BaseXP + (RoundModifiers.Instance.Config.LevelUp_XPPerLevel * level);
        }

        public virtual void Reset()
        {
            
        }

        
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}