using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using GameCore;
using LightContainmentZoneDecontamination;
using MoonSharp.Interpreter;
using SCriPt.Handlers;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaAPI
    {
        public static void Player_GodMode(Player player)
        {
            player.IsGodModeEnabled = !player.IsGodModeEnabled;
        }

        public static void Decontamination_Disable()
        {
            DecontaminationController.Singleton.NetworkDecontaminationOverride =
                DecontaminationController.DecontaminationStatus.Disabled;
        }
        
        public static void Lobby_Lock()
        {
            RoundStart.LobbyLock = true;
        }
        
        public static void Lobby_Unlock()
        {
            RoundStart.LobbyLock = false;
        }
        
        public static void Round_Lock()
        {
            RoundSummary.RoundLock = true;
        }
        
        public static void Round_Unlock()
        {
            RoundSummary.RoundLock = false;
        }
        
        public static void Round_Start()
        {
            RoundSummary.singleton.Start();
        }
        
        public static void Round_Restart()
        {
            
        }
        
        public static void Warhead_Lock()
        {
            Warhead.IsLocked = true;
        }
        
        public static void Warhead_Unlock()
        {
            Warhead.IsLocked = false;
        }
        
        public static void Warhead_Start()
        {
            Warhead.Start();
        }
        
        public static void Warhead_Stop()
        {
            Warhead.Stop();
        }




    }
}