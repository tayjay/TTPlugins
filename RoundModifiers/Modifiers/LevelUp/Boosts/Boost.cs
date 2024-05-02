using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public abstract class Boost
    {
        public Dictionary<uint, bool> HasBoost { get; set; }
        
        public Boost(Tier tier)
        {
            Tier = tier;
            HasBoost = DictionaryPool<uint, bool>.Pool.Get();
        }
        
        public virtual Tier Tier { get; set; }
        
        //If you only want a person to get a boost once at levelup do not put them in "HasBoost"
        
        //Ran as part of leveling up, if it returns false, the boost will not be given
        public abstract bool AssignBoost(Player player);
        
        //Ran after the boost is assigned, and when player respawns
        public abstract bool ApplyBoost(Player player);

        public virtual void Reset()
        {
            DictionaryPool<uint,bool>.Pool.Return(HasBoost);
        }


        public abstract string GetName();

        public virtual string GetColouredName()
        {
            string colour = "white";
            switch (Tier)
            {
                case Tier.Common:
                    colour = "white";
                    break;
                case Tier.Uncommon:
                    colour = "green";
                    break;
                case Tier.Rare:
                    colour = "blue";
                    break;
                case Tier.Epic:
                    colour = "purple";
                    break;
                case Tier.Legendary:
                    colour = "orange";
                    break;
                default:
                    colour = "white";
                    break;
            }
            return $"<color={colour}>{GetName()}</color>";
        
        }
        public abstract string GetDescription();
    }
    
    public enum Tier
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}