using Exiled.API.Features;
using MoonSharp.Interpreter;
using UnityEngine;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyTeslaGate
    {
        public Exiled.API.Features.TeslaGate TeslaGate { get; }
        
        [MoonSharpHidden]
        public ProxyTeslaGate(Exiled.API.Features.TeslaGate teslaGate)
        {
            TeslaGate = teslaGate;
        }
        
        public Room Room => TeslaGate.Room;
        public bool IsShocking => TeslaGate.IsShocking;

        public bool IsIdling
        {
            get => TeslaGate.IsIdling;
            set => TeslaGate.IsIdling = value;
        }
        
        public float InactiveTime
        {
            get => TeslaGate.InactiveTime;
            set => TeslaGate.InactiveTime = value;
        }
        
        public Vector3 HurtRange
        {
            get => TeslaGate.HurtRange;
            set => TeslaGate.HurtRange = value;
        }
        
        public void ForceTrigger()
        {
            TeslaGate.ForceTrigger();
        }

        public bool IsPlayerInTriggerRange(Player player)
        {
            return TeslaGate.IsPlayerInTriggerRange(player);
        }
        
        
    }
}