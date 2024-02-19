using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using TTCore.Handlers;

namespace TTCore
{
    public class TTCore : Plugin<TTConfig>
    {
        private static readonly TTCore Singleton = new TTCore();
        
        public static TTCore Instance => Singleton;

        private Harmony _harmony;
        
        public PlayerSizeManager PlayerSizeManager { get; private set; }
        public NpcManager NpcManager { get; private set; }

        private TTCore()
        {
            _harmony = new Harmony("ca.taytay.ttcore");
            PlayerSizeManager = new PlayerSizeManager();
        }

        public override PluginPriority Priority { get; } = PluginPriority.First;
        
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
            
            PlayerSizeManager = new PlayerSizeManager();
            NpcManager = new NpcManager();
            
        }

        private void Shutdown()
        {
            _harmony.UnpatchAll();
            
            PlayerSizeManager = null;
            NpcManager = null;
        }

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "TTCore";
        public override System.Version Version { get; } = new System.Version(1, 0, 0);
        
    }
}