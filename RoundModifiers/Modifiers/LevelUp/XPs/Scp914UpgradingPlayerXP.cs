using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Scp914;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class Scp914UpgradingPlayerXP : XP, IUpgradingPickupEvent
    {
        
        public List<uint> HasUpgraded { get; set; }
        
        public Scp914UpgradingPlayerXP() : base()
        {
            HasUpgraded = ListPool<uint>.Pool.Get();
        }
        
        protected override bool CanGiveXP(Player player)
        {
            return HasUpgraded.Contains(player.NetId);
        }
        
        public override void Reset()
        {
            ListPool<uint>.Pool.Return(HasUpgraded);
        }

        public void OnScp914UpgradingPickup(UpgradingPickupEventArgs ev)
        {
            List<Player> playerIn914 = Room.Get(RoomType.Lcz914).Players.ToList();
            foreach (Player p in playerIn914)
            {
                GiveXP(p, 100);
                HasUpgraded.Add(p.NetId);
            }
        }
    }
}