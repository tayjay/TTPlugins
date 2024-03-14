using System;
using System.IO;
using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Roles;
using GameCore;
using InventorySystem;
using LightContainmentZoneDecontamination;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Loaders;
using PlayerRoles;
using SCriPt.API.Lua.Helpers;
using SCriPt.API.Lua.Proxy;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace SCriPt.API.Lua
{
    public class ScriptLoader
    {
        public static void Load()
        {
            Script.DefaultOptions.DebugPrint = Log.Info;
            Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();
            //UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            RegisterTypes();
            
            if(!Directory.Exists("Scripts"))
            {
                Directory.CreateDirectory("Scripts");
            }
            foreach(string file in Directory.GetFiles("Scripts"))
            {
                if(file.EndsWith(".lua"))
                {
                    Script script = new Script();
                    RegisterAPI(script);
                    script.DoFile(file, codeFriendlyName:file.Substring(file.IndexOf("Scripts", StringComparison.Ordinal)+8));
                    SCriPt.Instance.LoadedScripts.Add(script);
                    Log.Info("Loaded script: "+file);
                }
            }
        }

        private static void RegisterAPI(Script script)
        {
            script.Globals["Cassie"] = (Action<string>) Cassie;
            script.Globals["Command"] = (Func<string, string>) Command;
            script.Globals["RegisterType"] = (Action<string>) RegisterType;
            script.Globals["API"] = UserData.CreateStatic<LuaAPI>();
        }

        private static void RegisterTypes()
        {
            UserData.RegisterType<Player>();
            UserData.RegisterType<Npc>();
            UserData.RegisterType<RoleTypeId>();
            UserData.RegisterType<Role>();
            UserData.RegisterType<Vector3>();
            //UserData.RegisterProxyType<ProxyPlayer, Player>(p => new ProxyPlayer(p));
            UserData.RegisterType<Role>();
            UserData.RegisterType<RoleTypeId>();
            UserData.RegisterType<Pickup>();
            UserData.RegisterType<ItemType>();
            UserData.RegisterType<Quaternion>();
            UserData.RegisterType<Transform>();
            UserData.RegisterType<ReferenceHub>();
            UserData.RegisterType<CharacterClassManager>();
            UserData.RegisterType<Inventory>();
            UserData.RegisterType<DecontaminationController>();
            UserData.CreateStatic<DecontaminationController>();
            //UserData.CreateStatic<RoundStart>();
            //UserData.CreateStatic<RoundSummary>();
            UserData.RegisterAssembly();

        }
        
        public static void RegisterType(string type)
        {
            UserData.RegisterType(Type.GetType($"{type},Exiled.API"));//NRE
        }
        
        public static void Cassie(string message)
        {
            Exiled.API.Features.Cassie.Message(message);
        }
        
        public static string Command(string command)
        {
            return Exiled.API.Features.Server.ExecuteCommand(command);
        }
    }
}