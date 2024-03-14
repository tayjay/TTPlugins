using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;
using MoonSharp.Interpreter;
using SCriPt.API.Lua;
using EventHandler = SCriPt.Handlers.EventHandler;


namespace SCriPt
{
    public class SCriPt : Plugin<Config>
    {
        private static readonly SCriPt Singleton = new SCriPt();
        public static SCriPt Instance => Singleton;
        
        public EventHandler EventHandler { get; private set; }
        
        public List<Script> LoadedScripts { get; private set; }

        private SCriPt()
        {
            
        }
        
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            try
            {
                Load();
            } catch (IOException e)
            {
                Log.Error("Error loading: "+e.Message);
            }
            Log.Info("SCriPt has been enabled!");
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Unload();
            Log.Info("SCriPt has been disabled!");
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
            Unload();
            
            Load();
        }

        public void Load()
        {
            LoadedScripts = new List<Script>();
            try
            {
                
                //ModApiImpl.Setup();
                Log.Info(rtnHelloWorld());
                Log.Info("Global sum: "+globalSum(5, 5));
                Log.Info(CallbackTest("Testing2"));
                ScriptLoader.Load();
            } catch (IOException e)
            {
                Log.Error("Error reading script file: "+e.Message);
            }
            
            EventHandler = new EventHandler();
            EventHandler.RegisterEvents();
        }

        public string rtnHelloWorld()
        {
            string script = @"
                return 'Hello, World!'
            ";
            DynValue res = Script.RunString(script);
            return res.String;
        }
        
        public static double globalSum(int a, int b)
        {
            string scriptCode = @"
                function sum(a, b)
                    return a + b
                end

                return sum(globalA, globalB)
            ";
            Script script = new Script();
            script.Globals["globalA"] = a;
            script.Globals["globalB"] = b;
            DynValue res = script.DoString(scriptCode);
            return res.Number;
        }

        private static string CallbackTest(string message)
        {
            string scriptCode = @"
                function log (message)
                    print(message)
                end
            ";
            Script script = new Script();
            script.Options.DebugPrint = s => Exiled.API.Features.Log.Info(s);
            script.Globals["LogC"] = (Func<string,string>) LogC;
            script.DoString(scriptCode);
            DynValue res = script.Call(script.Globals["log"], message);
            return res.String;
        }

        private static string LogC(string msg)
        {
            Exiled.API.Features.Log.Info(msg);
            return msg;
        }
        
        
        
        
        
        
        public void Unload()
        {
            EventHandler.UnregisterEvents();
            /*LoadedScripts.Clear();
            
            LuaScriptExecutor = null;
            LuaScriptLoader = null;*/
            LoadedScripts.Clear();
            EventHandler = null;
        }
        
        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "SCriPt";
        public override System.Version Version { get; } = new System.Version(0, 1, 1);
        
        
    }
}