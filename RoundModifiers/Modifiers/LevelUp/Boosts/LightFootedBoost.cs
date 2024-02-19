using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using TayTaySCPSL.Modifiers.LevelUp.Interfaces;

namespace TayTaySCPSL.Modifiers.LevelUp.Boost
{
    public class LightFootedBoost : Boost, IMakingNoiseEvent
    {
        
        public LightFootedBoost() : base(Tier.Legendary)
        {
            
        }

        public override bool AssignBoost(Player player)
        {
            HasBoost[player.NetId] = true;
            return true;
        }

        public override bool ApplyBoost(Player player)
        {
            return true;
        }

        public override string GetName()
        {
            return Name;
        }

        public override string GetDescription()
        {
            return Description;
        }

        public void OnMakingNoise(MakingNoiseEventArgs ev)
        {
            if (HasBoost.ContainsKey(ev.Player.NetId))
            {
                ev.IsAllowed = false;
            }
        }
        
        public string Name { get; set; } = "Light Footed";
        public string Description { get; set; } = "You make no noise when walking.";
    }
}