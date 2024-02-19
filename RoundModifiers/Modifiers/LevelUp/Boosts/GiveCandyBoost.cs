using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp330;

namespace TayTaySCPSL.Modifiers.LevelUp.Boost
{
    public class GiveCandyBoost : Boost
    {
        private CandyKindID _kind;
        
        public GiveCandyBoost(CandyKindID kind, Tier tier=Tier.Common) : base(tier)
        {
            _kind = kind;
        }

        public override bool AssignBoost(Player player)
        {
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            return player.TryAddCandy(_kind);
        }

        public override string GetName()
        {
            return "Give Candy: "+_kind.ToString();
        }

        public override string GetDescription()
        {
            return Description;
        }
        
        public string Name { get; set; } = "Give Candy";
        public string Description { get; set; } = "Receive a random candy when you level up.";
        
    }
}