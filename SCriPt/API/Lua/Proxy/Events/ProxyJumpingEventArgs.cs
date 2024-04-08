using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MoonSharp.Interpreter;
using UnityEngine;

namespace SCriPt.API.Lua.Proxy.Events
{
    public class ProxyJumpingEventArgs
    {
        private JumpingEventArgs ev  { get; }
        
        [MoonSharpHidden]
        public ProxyJumpingEventArgs(JumpingEventArgs ev)
        {
            this.ev = ev;
        }
        
        public Player Player => ev.Player;
        public Vector3 Direction
        {
            get => ev.Direction;
            set => ev.Direction = value;
        }
        public float Speed
        {
            get => ev.Speed;
            set => ev.Speed = value;
        }
        public bool IsAllowed
        {
            get => ev.IsAllowed;
            set => ev.IsAllowed = value;
        }
        
    }
}