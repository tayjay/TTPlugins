using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;


namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public class LightFootedBoost : Boost, IMakingNoiseEvent
    {
        
        public LightFootedBoost() : base(Tier.Legendary)
        {
            
        }

        public override bool AssignBoost(Player player)
        {
            if(player.IsScp) return false;
            HasBoost[player.NetId] = true;
            return ApplyBoost(player);
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