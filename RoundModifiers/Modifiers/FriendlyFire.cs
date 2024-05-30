using Exiled.API.Features;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class FriendlyFire : Modifier
    {
        
        private bool WasFriendlyFireEnabled = false;
        private bool WasFriendlyFireDetectionPaused = false;
        private FriendlyFireAction LifeAction = FriendlyFireAction.Noop;
        private FriendlyFireAction RespawnAction = FriendlyFireAction.Noop;
        private FriendlyFireAction RoundAction = FriendlyFireAction.Noop;
        private FriendlyFireAction WindowAction = FriendlyFireAction.Noop;
        
        public void OnRoundStart()
        {
            WasFriendlyFireEnabled = Server.FriendlyFire;
            WasFriendlyFireDetectionPaused = FriendlyFireConfig.PauseDetector;
            Server.FriendlyFire = true;
            FriendlyFireConfig.PauseDetector = true;
            ServerConfigSynchronizer.RefreshAllConfigs();
            //todo: Prevent player bans for friendly fire
            
           
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            
            Server.FriendlyFire = WasFriendlyFireEnabled;
            FriendlyFireConfig.PauseDetector = WasFriendlyFireDetectionPaused;
            ServerConfigSynchronizer.RefreshAllConfigs();
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "FriendlyFire",
            FormattedName = "<color=green>Friendly Fire</color>",
            Aliases = new []{"ff"},
            Description = "Friendly fire is enabled.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = -1
        };
    }
}