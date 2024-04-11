using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MoonSharp.Interpreter;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyRoom
    {
        public Room Room { get; }
        
        [MoonSharpHidden]
        public ProxyRoom(Room room)
        {
            Room = room;
        }
        
        public string Name => Room.Name;
        public string Type => Room.Type.ToString();
        public string Zone => Room.Zone.ToString();
        public Vector3 Position => Room.Position;
        public Quaternion Rotation => Room.Rotation;
        public List<Camera> Cameras => Room.Cameras.ToList();
        public Exiled.API.Features.TeslaGate TeslaGate => Room.TeslaGate;
        public List<Door> Doors => Room.Doors.ToList();
        public Color LightColor => Room.Color;
        public bool AreLightsOff => Room.AreLightsOff;
        public List<Player> Players => Room.Players.ToList();
        
        public void SetLightColor(float r, float g, float b)
        {
            Room.Color = new Color(r,g,b);
        }
        
        public void TurnLightsOff(int duration)
        {
            Room.TurnOffLights(duration);
        }
        
        public void TurnLightsOn()
        {
            if(AreLightsOff)
                Room.TurnOffLights(0);
        }
        
        
    }
}