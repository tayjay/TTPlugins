using Exiled.API.Enums;
using Exiled.API.Features;

namespace RoundModifiers
{
    public class RoundModifiers : Plugin<Config>
    {
        private static readonly RoundModifiers Singleton = new RoundModifiers();
        
        public static RoundModifiers Instance => Singleton;

        private RoundModifiers()
        {
            
        }

        public override PluginPriority Priority { get; } = PluginPriority.Last;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            Log.Info("RoundModifiers has been enabled!");
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Log.Info("RoundModifiers has been disabled!");
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
            Log.Info("RoundModifiers has been reloaded!");
        }

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "RoundModifiers";
        public override System.Version Version { get; } = new System.Version(1, 0, 0);
        public override string Prefix { get; }
    }
}