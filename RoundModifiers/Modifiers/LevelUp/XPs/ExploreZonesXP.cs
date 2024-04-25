using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using RoundModifiers.Modifiers.LevelUp.Interfaces;


namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class ExploreZonesXP : XP, IGameTickEvent
    {
        private Dictionary<uint,List<ZoneType>> _zones = new Dictionary<uint, List<ZoneType>>();
        
        
        public void OnGameTick()
        {
            foreach(Player player in Player.List)
            {
                if (/*player.Role.Team == Team.SCPs || */player.Role.Team == Team.Dead) continue;
                if (!_zones.ContainsKey(player.NetId))
                    _zones[player.NetId] = new List<ZoneType>();
                
                if (player.CurrentRoom.Zone != ZoneType.Unspecified && !_zones[player.NetId].Contains(player.CurrentRoom.Zone))
                {
                    _zones[player.NetId].Add(player.CurrentRoom.Zone);
                    GiveXP(player, 50);
                }
            }
        }
        
        public override void Reset()
        {
            _zones.Clear();
        }
        
        public override string Name { get; set; } = "Explore Zones";
        public override string Description { get; set; } = "Gain XP for exploring new zones.";
    }
}