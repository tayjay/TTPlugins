﻿using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using TTAddons.Handlers;
using TTCore.Utilities;

namespace TTAddons
{
    public class TTAddons : Plugin<Config>
    {
        private static readonly TTAddons Singleton = new TTAddons();
        
        public static TTAddons Instance => Singleton;
        
        private Harmony _harmony;
        public Unstuck Unstuck { get; private set; }
        public Scp3114Handler Scp3114Handler { get; private set; }
        
        public ClassDHandler ClassDHandler { get; private set; }
        public SpectatorHandler SpectatorHandler { get; private set; }
        //public LightFixHandler LightFixHandler { get; private set; }
        public Scp0492Handler Scp0492Handler { get; private set; }
        public Scp079Handler Scp079Handler { get; private set; }
        
        public NoBanHandler NoBanHandler { get; private set; }
        public SavedVoicesHandler SavedVoicesHandler { get; private set; }
        
        public RoleSelectorHandler RoleSelectorHandler { get; private set; }
        
        public LateRespawnHandler LateRespawnHandler { get; private set; }
        //public MapChangeHandler MapChangeHandler { get; private set; }
        
        public CombatLogHandler CombatLogHandler { get; private set; }
        

        private TTAddons()
        {
            _harmony = new Harmony("ca.taytay.ttaddons");
        }

        public override PluginPriority Priority { get; } = PluginPriority.Last;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            SetupPlugin();
            
            Log.Info("TTAddons has been enabled!");
        }

        private void SetupPlugin()
        {
            _harmony.PatchAll();
            //Initialize objects
            Unstuck = new Unstuck();
            Scp3114Handler = new Scp3114Handler();
            ClassDHandler = new ClassDHandler();
            SpectatorHandler = new SpectatorHandler();
            //LightFixHandler = new LightFixHandler();
            Scp0492Handler = new Scp0492Handler();
            Scp079Handler = new Scp079Handler();
            NoBanHandler = new NoBanHandler();
            SavedVoicesHandler = new SavedVoicesHandler();
            RoleSelectorHandler = new RoleSelectorHandler();
            LateRespawnHandler = new LateRespawnHandler();
            //MapChangeHandler = new MapChangeHandler();
            CombatLogHandler = new CombatLogHandler();
            
            
            //Register objects
            Unstuck.Register();
            Scp3114Handler.Register();
            if(Config.EnableSpectatorGame)
                SpectatorHandler.Register();
            //ClassDHandler.Register();
            /*if(Config.EnableWeaponStats)
                WeaponStats.Register();*/
            //LightFixHandler.Register();
            Scp3114Handler.Register();
            Scp0492Handler.Register();
            Scp079Handler.Register();
            NoBanHandler.Register();
            SavedVoicesHandler.Register();
            RoleSelectorHandler.Register();
            LateRespawnHandler.Register();
            //MapChangeHandler.Register();
            CombatLogHandler.Register();
            
        }
        
        private void ShutdownPlugin()
        {
            _harmony.UnpatchAll();
            //Unregister objects
            Unstuck.Unregister();
            Scp3114Handler.Unregister();
            if(Config.EnableSpectatorGame)
                SpectatorHandler.Unregister();
            //ClassDHandler.Unregister();
            /*if(Config.EnableWeaponStats)
                WeaponStats.Unregister();*/
            //LightFixHandler.Unregister();
            Scp3114Handler.Unregister();
            Scp0492Handler.Unregister();
            Scp079Handler.Unregister();
            NoBanHandler.Unregister();
            SavedVoicesHandler.Unregister();
            RoleSelectorHandler.Unregister();
            LateRespawnHandler.Unregister();
            //MapChangeHandler.Unregister();
            CombatLogHandler.Unregister();
            
            //Dispose objects
            Unstuck = null;
            Scp3114Handler = null;
            ClassDHandler = null;
            SpectatorHandler = null;
            //LightFixHandler = null;
            Scp0492Handler = null;
            Scp079Handler = null;
            NoBanHandler = null;
            SavedVoicesHandler = null;
            RoleSelectorHandler = null;
            LateRespawnHandler = null;
            //MapChangeHandler = null;
            CombatLogHandler = null;
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            ShutdownPlugin();
            Log.Info("TTAddons has been disabled!");
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
            ShutdownPlugin();
            SetupPlugin();
            Log.Info("TTAddons has been reloaded!");
        }

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "TTAddons";
        public override System.Version Version { get; } = new System.Version(0, 8, 0);

    }
}