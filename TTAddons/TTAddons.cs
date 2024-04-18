using Exiled.API.Enums;
using Exiled.API.Features;
using TTAddons.Handlers;

namespace TTAddons
{
    public class TTAddons : Plugin<Config>
    {
        private static readonly TTAddons Singleton = new TTAddons();
        
        public static TTAddons Instance => Singleton;
        
        public Unstuck Unstuck { get; private set; }
        public Scp3114Handler Scp3114Handler { get; private set; }
        
        public ClassDHandler ClassDHandler { get; private set; }
        public SpectatorHandler SpectatorHandler { get; private set; }
        //public LightFixHandler LightFixHandler { get; private set; }
        

        private TTAddons()
        {
            
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
            //Initialize objects
            Unstuck = new Unstuck();
            Scp3114Handler = new Scp3114Handler();
            ClassDHandler = new ClassDHandler();
            SpectatorHandler = new SpectatorHandler();
            //LightFixHandler = new LightFixHandler();
            
            
            //Register objects
            Unstuck.Register();
            Scp3114Handler.Register();
            if(Config.EnableSpectatorGame)
                SpectatorHandler.Register();
            //ClassDHandler.Register();
            if(Config.EnableWeaponStats)
                WeaponStats.Register();
            //LightFixHandler.Register();
        }
        
        private void ShutdownPlugin()
        {
            //Unregister objects
            Unstuck.Unregister();
            Scp3114Handler.Unregister();
            if(Config.EnableSpectatorGame)
                SpectatorHandler.Unregister();
            //ClassDHandler.Unregister();
            if(Config.EnableWeaponStats)
                WeaponStats.Unregister();
            //LightFixHandler.Unregister();
            
            //Dispose objects
            Unstuck = null;
            Scp3114Handler = null;
            ClassDHandler = null;
            SpectatorHandler = null;
            //LightFixHandler = null;
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
        public override System.Version Version { get; } = new System.Version(0, 2, 1);

    }
}