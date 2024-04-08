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
            Script.DefaultOptions.DebugPrint = Log.Info;
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
            
            Script = new Script();
            RegisterAPI(Script);
            foreach(string file in Directory.GetFiles("Scripts/AutoLoad"))
            {
                if(file.EndsWith(".lua"))
                {
                    //Script script = new Script();
                    Script.DoFile(file);
                    //SCriPt.Instance.LoadedScripts.Add(script);
                    Log.Info("Loaded script: "+file);
                }
            }
            SCriPt.Instance.LoadedScripts.Add(Script);
        }

        public static void RegisterAPI(Script script)
        {
            script.Globals["RegisterType"] = (Action<string,string>) RegisterType;
            script.Globals["API"] = UserData.CreateStatic<LuaAPI>();
            script.Globals["Warhead"] = UserData.CreateStatic<LuaWarhead>();
            script.Globals["Decon"] = UserData.CreateStatic<LuaDecon>();
            script.Globals["Lobby"] = UserData.CreateStatic<LuaLobby>();
            script.Globals["Round"] = UserData.CreateStatic<LuaRound>();
            script.Globals["Facility"] = UserData.CreateStatic<LuaFacility>();
            script.Globals["Cassie"] = UserData.CreateStatic<LuaCassie>();
            script.Globals["Server"] = UserData.CreateStatic<LuaServer>();
            //script.Globals["Registry"] = UserData.CreateStatic<Registry>();
            script.Globals["Events"] = UserData.CreateStatic<LuaEvents>();
            script.Globals["Player"] = UserData.CreateStatic<LuaPlayer>();
            script.Globals["Coroutines"] = UserData.CreateStatic<LuaCoroutines>();
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