using System.Reflection;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using PlayerRoles.FirstPersonControl;
using TTCore.Handlers;
using TTCore.HUDs;
using TTCore.Npcs;
using TTCore.Npcs.AI.Pathing;
using UnityEngine;

namespace TTCore
{
    public class TTCore : Plugin<TTConfig>
    {
        private static readonly TTCore Singleton = new TTCore();
        
        public static TTCore Instance => Singleton;

        private Harmony _harmony;
        
        public PlayerSizeManager PlayerSizeManager { get; private set; }
        public NpcManager NpcManager { get; private set; }
        public NavMeshBuilder NavMeshBuilder { get; private set; }

        private TTCore()
        {
            _harmony = new Harmony("ca.taytay.ttcore");
            
        }

        public override PluginPriority Priority { get; } = PluginPriority.Higher;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            Setup();
            Log.Info("TTCore has been enabled!");
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Shutdown();
            Log.Info("TTCore has been disabled!");
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
            Shutdown();
            
            Setup();
            Log.Info("TTCore has been reloaded!");
        }

        private void Setup()
        {
            _harmony.PatchAll();
            HUD.Register();
            PlayerSizeManager = new PlayerSizeManager();
            NpcManager = new NpcManager();
            //NavMeshBuilder = new NavMeshBuilder();
            
            //NavMeshBuilder.Register();
            

        }

        private void Shutdown()
        {
            _harmony.UnpatchAll();
            HUD.Unregister();
            //NavMeshBuilder.Unregister();
            PlayerSizeManager = null;
            NpcManager = null;
            //NavMeshBuilder = null;
        }

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "TTCore";
        public override System.Version Version { get; } = new System.Version(0, 2, 0);
        
    }
}