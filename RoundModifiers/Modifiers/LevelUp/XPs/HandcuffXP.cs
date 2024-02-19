using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class HandcuffXP : XP, IHandcuffingEvent
    {
        Dictionary<uint, List<uint>> _handcuffer = new Dictionary<uint, List<uint>>();
        
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!_handcuffer.ContainsKey(ev.Player.NetId))
            {
                _handcuffer.Add(ev.Player.NetId, new List<uint>());
            }
            if (!_handcuffer[ev.Player.NetId].Contains(ev.Target.NetId))
            {
                _handcuffer[ev.Player.NetId].Add(ev.Target.NetId);
                GiveXP(ev.Player, 30);
            }
        }
        
        public override void Reset()
        {
            _handcuffer.Clear();
        }
    }
}