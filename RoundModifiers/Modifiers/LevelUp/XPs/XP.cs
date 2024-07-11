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
                player.SessionVariables["levelup_xp"] = (float) player.SessionVariables["levelup_xp"] + amount;
                //LevelUpHandler.PlayerXP[player.NetId] += amount;
                TryLevelUp(player);
            }
        }

        protected static void TryLevelUp(Player player)
        {
            //if(player.IsScp) return;
            float xp = (float) player.SessionVariables["levelup_xp"];
            int level = (int) player.SessionVariables["levelup_level"];
            /*float xp = LevelUpHandler.PlayerXP[player.NetId];
            float level = LevelUpHandler.PlayerLevel[player.NetId];*/
            float xpNeeded = GetXPNeeded(level);

            while (xp >= GetXPNeeded(level))
            {
                player.SessionVariables["levelup_xp"] = (float) player.SessionVariables["levelup_xp"] - xpNeeded;
                player.SessionVariables["levelup_level"] = (int) player.SessionVariables["levelup_level"] + 1;
                LevelUpHandler.OnLevelUp(player);
                xp = (float) player.SessionVariables["levelup_xp"];
                level = (int) player.SessionVariables["levelup_level"];
                /*xp = LevelUpHandler.PlayerXP[player.NetId];
                level = LevelUpHandler.PlayerLevel[player.NetId];*/
            }
        }
        
        public static float GetXPNeeded(float level)
        {
            return LevelUp.Config.BaseXP + (LevelUp.Config.XPPerLevel * level);
        }

        public virtual void Reset()
        {
            
        }

        
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}