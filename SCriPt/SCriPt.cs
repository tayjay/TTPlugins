using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;
using MoonSharp.Interpreter;
using SCriPt.API.Lua;
using SCriPt.Handlers;



namespace SCriPt
{
    public class SCriPt : Plugin<Config>
    {
        private static readonly SCriPt Singleton = new SCriPt();
        public static SCriPt Instance => Singleton;
        
        
        public PlayerEvents PlayerEvents { get; private set; }
        public ServerEvents ServerEvents { get; private set; }
        public WarheadEvents WarheadEvents { get; private set; }
        
        public ItemEvents ItemEvents { get; private set; }
        
        public MapEvents MapEvents { get; private set; }
        
        public Scp049Events Scp049Events { get; private set; }
        
        public Scp0492Events Scp0492Events { get; private set; }
        
        public Scp079Events Scp079Events { get; private set; }
        
        public Scp096Events Scp096Events { get; private set; }
        
        public Scp106Events Scp106Events { get; private set; }
        
        public Scp173Events Scp173Events { get; private set; }
        
        public Scp244Events Scp244Events { get; private set; }
        
        public Scp330Events Scp330Events { get; private set; }
        
        public Scp914Events Scp914Events { get; private set; }
        
        public Scp939Events Scp939Events { get; private set; }
        
        public Scp3114Events Scp3114Events { get; private set; }
        
        public CommandEvents CommandEvents { get; private set; }
        
        
        public Dictionary<string,Script> LoadedScripts { get; private set; } = new Dictionary<string,Script>();

        private SCriPt()
        {
            
        }
        
        public override PluginPriority Priority { get; } = PluginPriority.Higher;
        
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

        public void Register()
        {
            PlayerEvents = new PlayerEvents();
            ServerEvents = new ServerEvents();
            WarheadEvents = new WarheadEvents();
            ItemEvents = new ItemEvents();
            MapEvents = new MapEvents();
            Scp049Events = new Scp049Events();
            Scp0492Events = new Scp0492Events();
            Scp079Events = new Scp079Events();
            Scp096Events = new Scp096Events();
            Scp106Events = new Scp106Events();
            Scp173Events = new Scp173Events();
            Scp244Events = new Scp244Events();
            Scp330Events = new Scp330Events();
            Scp914Events = new Scp914Events();
            Scp939Events = new Scp939Events();
            Scp3114Events = new Scp3114Events();
            CommandEvents = new CommandEvents();
            
            
            PlayerEvents.RegisterEvents();
            PlayerEvents.RegisterEventTypes();
            ServerEvents.RegisterEvents();
            ServerEvents.RegisterEventTypes();
            WarheadEvents.RegisterEvents();
            WarheadEvents.RegisterEventTypes();
            ItemEvents.RegisterEvents();
            ItemEvents.RegisterEventTypes();
            MapEvents.RegisterEvents();
            MapEvents.RegisterEventTypes();
            Scp049Events.RegisterEvents();
            Scp049Events.RegisterEventTypes();
            Scp0492Events.RegisterEvents();
            Scp0492Events.RegisterEventTypes();
            Scp079Events.RegisterEvents();
            Scp079Events.RegisterEventTypes();
            Scp096Events.RegisterEvents();
            Scp096Events.RegisterEventTypes();
            Scp106Events.RegisterEvents();
            Scp106Events.RegisterEventTypes();
            Scp173Events.RegisterEvents();
            Scp173Events.RegisterEventTypes();
            Scp244Events.RegisterEvents();
            Scp244Events.RegisterEventTypes();
            Scp330Events.RegisterEvents();
            Scp330Events.RegisterEventTypes();
            Scp914Events.RegisterEvents();
            Scp914Events.RegisterEventTypes();
            Scp939Events.RegisterEvents();
            Scp939Events.RegisterEventTypes();
            Scp3114Events.RegisterEvents();
            Scp3114Events.RegisterEventTypes();
            CommandEvents.RegisterEvents();
            CommandEvents.RegisterEventTypes();
            Log.Info("Events registered!");
        }
        
        public void Unregister()
        {
            PlayerEvents.UnregisterEvents();
            ServerEvents.UnregisterEvents();
            WarheadEvents.UnregisterEvents();
            ItemEvents.UnregisterEvents();
            MapEvents.UnregisterEvents();
            Scp049Events.UnregisterEvents();
            Scp0492Events.UnregisterEvents();
            Scp079Events.UnregisterEvents();
            Scp096Events.UnregisterEvents();
            Scp106Events.UnregisterEvents();
            Scp173Events.UnregisterEvents();
            Scp244Events.UnregisterEvents();
            Scp330Events.UnregisterEvents();
            Scp914Events.UnregisterEvents();
            Scp939Events.UnregisterEvents();
            Scp3114Events.UnregisterEvents();
            CommandEvents.UnregisterEvents();
            
            PlayerEvents = null;
            ServerEvents = null;
            WarheadEvents = null;
            ItemEvents = null;
            MapEvents = null;
            Scp049Events = null;
            Scp0492Events = null;
            Scp079Events = null;
            Scp096Events = null;
            Scp106Events = null;
            Scp173Events = null;
            Scp244Events = null;
            Scp330Events = null;
            Scp914Events = null;
            Scp939Events = null;
            Scp3114Events = null;
            CommandEvents = null;
        }

        public void Load()
        {
            Register();
            LoadedScripts = new Dictionary<string, Script>();
            try
            {
                ScriptLoader.AutoLoad();
            } catch (IOException e)
            {
                Log.Error("Error reading script file: "+e.Message);
            }
            
            //EventHandler = new EventHandler();
            //EventHandler.RegisterEvents();
        }

        
        
        
        
        
        
        
        public void Unload()
        {
            //EventHandler.UnregisterEvents();
            Unregister();
            /*LoadedScripts.Clear();
            
            LuaScriptExecutor = null;
            LuaScriptLoader = null;*/
            LoadedScripts.Clear();
            //EventHandler = null;
        }
        
        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "SCriPt";
        public override System.Version Version { get; } = new System.Version(0, 2, 0);
        
        
    }
}