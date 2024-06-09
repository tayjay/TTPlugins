using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
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
            //todo: Prevent player bans for friendly fire
        }

        public void OnBanningPlayer(BanningEventArgs ev)
        {
            ev.IsAllowed = false;
        }
        
        protected override void RegisterModifier()
        {
            WasFriendlyFireEnabled = Server.FriendlyFire;
            WasFriendlyFireDetectionPaused = FriendlyFireConfig.PauseDetector;
            Server.FriendlyFire = true;
            FriendlyFireConfig.PauseDetector = true;
            ServerConfigSynchronizer.RefreshAllConfigs();
            
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.Banning += OnBanningPlayer;
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
            Balance = -1,
            Category = Category.Combat
        };
    }
}