using System;
using System.IO;
using System.Reflection;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Player;
using GameCore;
using InventorySystem;
using InventorySystem.Items.Pickups;
using LightContainmentZoneDecontamination;
using MEC;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Loaders;
using PlayerRoles;
using SCriPt.API.Lua.Proxy;
using SCriPt.API.Lua.Proxy.Events;
using SCriPt.API.Lua.Globals;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace SCriPt.API.Lua
{
    public class ScriptLoader
    {
        
        public static Script Script = new Script();
        
        public static void AutoLoad()
        {
            Script.DefaultOptions.DebugPrint = s => Log.Send("[Lua] " + s, Discord.LogLevel.Debug, ConsoleColor.Green);;
            Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();
            //UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            RegisterTypes();
            
            if(!Directory.Exists("Scripts"))
            {
                Directory.CreateDirectory("Scripts");
            }
            if(!Directory.Exists("Scripts/AutoLoad"))
            {
                Directory.CreateDirectory("Scripts/AutoLoad");
            }
            if(!Directory.Exists("Scripts/Globals"))
            {
                Directory.CreateDirectory("Scripts/Globals");
            }
            
            Script = new Script();
            RegisterAPI(Script);
            foreach(string file in Directory.GetFiles("Scripts/AutoLoad"))
            {
                if(file.EndsWith(".lua"))
                {
                    try
                    {
                        //Script script = new Script();
                        Script.DoFile(file);
                        //SCriPt.Instance.LoadedScripts.Add(script);
                        Log.Info("Loaded script: " + file);
                    }
                    catch (ScriptRuntimeException e)
                    {
                        Log.Error(e.DecoratedMessage);
                    }
                }
            }
            SCriPt.Instance.LoadedScripts.Add(Script);
        }

        public static void CustomAPIs(Script script)
        {
            foreach(string file in Directory.GetFiles("Scripts/Globals"))
            {
                if(file.EndsWith(".lua"))
                {
                    script.DoFile(file);
                    Log.Info("Loaded Custom Global: "+file);
                }
            }
        }

        private static DynValue API;
        private static DynValue Warhead;
        private static DynValue Decon;
        private static DynValue Lobby;
        private static DynValue Round;
        private static DynValue Facility;
        private static DynValue Cassie;
        private static DynValue Server;
        private static DynValue Events;
        private static DynValue Player;
        private static DynValue Coroutines;
        private static DynValue Role;

        public static void RegisterAPI(Script script)
        {
            
            if(API == null) API = UserData.CreateStatic<LuaAPI>();
            if(Warhead == null) Warhead = UserData.CreateStatic<LuaWarhead>();
            if(Decon == null) Decon = UserData.CreateStatic<LuaDecon>();
            if(Lobby == null) Lobby = UserData.CreateStatic<LuaLobby>();
            if(Round == null) Round = UserData.CreateStatic<LuaRound>();
            if(Facility == null) Facility = UserData.CreateStatic<LuaFacility>();
            if(Cassie == null) Cassie = UserData.CreateStatic<LuaCassie>();
            if(Server == null) Server = UserData.CreateStatic<LuaServer>();
            if(Events == null) Events = UserData.CreateStatic<LuaEvents>();
            if(Player == null) Player = UserData.CreateStatic<LuaPlayer>();
            if(Coroutines == null) Coroutines = UserData.CreateStatic<LuaCoroutines>();
            if(Role == null) Role = UserData.CreateStatic<LuaRole>();
            
            script.Globals["RegisterType"] = (Action<string,string>) RegisterType;
            script.Globals["API"] = API;
            script.Globals["Warhead"] = Warhead;
            script.Globals["Decon"] = Decon;
            script.Globals["Lobby"] = Lobby;
            script.Globals["Round"] = Round;
            script.Globals["Facility"] = Facility;
            script.Globals["Cassie"] = Cassie;
            script.Globals["Server"] = Server;
            script.Globals["Events"] = Events;
            script.Globals["Player"] = Player;
            script.Globals["Timing"] = Coroutines;
            script.Globals["Role"] = Role;
            CustomAPIs(script); 
        }

        private static void RegisterTypes()
        {
            //UserData.RegisterType<Player>();
            UserData.RegisterProxyType<ProxyPlayer, Player>(p => new ProxyPlayer(p));
            UserData.RegisterProxyType<ProxyPickup, Pickup>(p => new ProxyPickup(p));
            UserData.RegisterProxyType<ProxyRoom, Room>(r => new ProxyRoom(r));
            UserData.RegisterProxyType<ProxyTeslaGate, Exiled.API.Features.TeslaGate>(t => new ProxyTeslaGate(t));
            UserData.RegisterProxyType<ProxyItem, Item>(i => new ProxyItem(i));
            UserData.RegisterProxyType<ProxyDoor, Door>(d => new ProxyDoor(d));
            UserData.RegisterProxyType<ProxyNpc, Npc>(n => new ProxyNpc(n));
            //UserData.RegisterType<Npc>();
            UserData.RegisterType<RoleTypeId>();
            UserData.RegisterType<DoorLockType>();
            UserData.RegisterType<DoorType>();
            UserData.RegisterType<Role>();
            UserData.RegisterType<Vector3>();
            UserData.RegisterType<RoleTypeId>();
            UserData.RegisterType<ItemType>();
            UserData.RegisterType<Quaternion>();
            UserData.RegisterType<Transform>();
            UserData.RegisterType<ReferenceHub>();
            UserData.RegisterType<CharacterClassManager>();
            UserData.RegisterType<Inventory>();
            UserData.RegisterType<Team>();
            UserData.RegisterType<Side>();
            UserData.RegisterType<IExiledEvent>();
            UserData.RegisterType<IDeniableEvent>();
            UserData.RegisterType<IPlayerEvent>();
            UserData.RegisterType<EventArgs>();
            UserData.RegisterType<ItemPickupBase>();
            UserData.RegisterType<PickupSyncInfo>();
            UserData.RegisterType<CoroutineHandle>();
            UserData.RegisterAssembly();
        }
        
        public static void RegisterType(string type, string assembly = "Exiled.API")
        {
            //UserData.RegisterType(Type.GetType($"{type},Exiled.API"));//NRE
            Type classType = Type.GetType($"{type}, {assembly}");
            if (classType != null)
            {
                // Successfully retrieved the Type object for Ammo class
                // You can now use 'ammoType' to create instances, invoke methods, etc.
                //Log.Info("Type: "+classType.ToString());
                try
                {
                    if (UserData.IsTypeRegistered(classType))
                    {
                        Log.Error(classType + " is already registered.");
                        return;
                    }
                    UserData.RegisterType(classType);
                    Log.Info("Registered: "+classType.ToString());
                } catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            else
            {
                // Failed to retrieve the Type
                // Check the assembly-qualified name and assembly availability
                Log.Info("Type: null");
            }
        }
        
    }
}