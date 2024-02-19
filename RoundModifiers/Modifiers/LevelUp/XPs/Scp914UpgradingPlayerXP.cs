using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XP
{
    public class Scp914UpgradingPlayerXP : XP, IUpgradingPlayerEvent
    {
        
        public List<uint> HasUpgraded = new List<uint>();
        
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
            HasUpgraded.Clear();
        }
    }
}