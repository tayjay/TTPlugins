using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Scp914;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class Scp914UpgradingPlayerXP : XP, IUpgradingPlayerEvent
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

        public void OnScp914UpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            GiveXP(ev.Player, 100);
        }
        
        public override void Reset()
        {
            ListPool<uint>.Pool.Return(HasUpgraded);
        }
    }
}