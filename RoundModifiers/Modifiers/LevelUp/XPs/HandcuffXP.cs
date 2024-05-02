using System.Collections.Generic;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs
{
    public class HandcuffXP : XP, IHandcuffingEvent
    {
        private Dictionary<uint, List<uint>> _handcuffer { get; set; }
        
        public HandcuffXP() : base()
        {
            _handcuffer = DictionaryPool<uint, List<uint>>.Pool.Get();
        }
        
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (!_handcuffer.ContainsKey(ev.Player.NetId))
            {
                _handcuffer.Add(ev.Player.NetId, ListPool<uint>.Pool.Get());
            }
            if (!_handcuffer[ev.Player.NetId].Contains(ev.Target.NetId))
            {
                _handcuffer[ev.Player.NetId].Add(ev.Target.NetId);
                GiveXP(ev.Player, 30);
            }
        }
        
        public override void Reset()
        {
            foreach (List<uint> handcuffers in _handcuffer.Values)
            {
                ListPool<uint>.Pool.Return(handcuffers);
            }
            DictionaryPool<uint, List<uint>>.Pool.Return(_handcuffer);
        }
    }
}