using Exiled.API.Enums;
using Exiled.API.Features;

namespace TTCore
{
    public class TTCore : Plugin<TTConfig>
    {
        private static readonly TTCore Singleton = new TTCore();
        
        public static TTCore Instance => Singleton;

        private TTCore()
        {
            
        }

        public override PluginPriority Priority { get; } = PluginPriority.First;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            Log.Info("TTCore has been enabled!");
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Log.Info("TTCore has been disabled!");
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
            Log.Info("TTCore has been reloaded!");
        }

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "TTCore";
        public override System.Version Version { get; } = new System.Version(1, 0, 0);
        public override string Prefix { get; }
    }
}