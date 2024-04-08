using System.Reflection;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MoonSharp.Interpreter;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaFacility
    {
        public static void TurnOffLights(float time)
        {
            Map.TurnOffAllLights(time);
        }
        
        public static void LockAllDoors(float duration)
        {
            Door.LockAll(duration,DoorLockType.AdminCommand);
        }
        
        public static void UnlockAllDoors()
        {
            Door.UnlockAll();
        }
        
        public static Vector3 Gravity
        {
            get => FpcMotor.Gravity;
            set
            {
                var Gravity = typeof(FpcMotor).GetField("Gravity", BindingFlags.Static | BindingFlags.NonPublic);
                Gravity?.SetValue(null, value);
            }
        }
        
        
    }
}