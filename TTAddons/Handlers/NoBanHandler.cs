using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using TTCore.API;

namespace TTAddons.Handlers
{
    public class NoBanHandler : IRegistered
    {
        public void OnBanning(BanningEventArgs ev)
        {
            if(!TTAddons.Instance.Config.NoAutoBan) return;
            if(ev.Player != Server.Host) return;
            ev.IsAllowed = false;
        }

        public void OnKicking(KickingEventArgs ev)
        {
            if(!TTAddons.Instance.Config.NoAutoKick) return;
            if(ev.Player != Server.Host) return;
            ev.IsAllowed = false;
        }

        public void Register()
        {
            Exiled.Events.Handlers.Player.Banning += OnBanning;
            Exiled.Events.Handlers.Player.Kicking += OnKicking;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Player.Banning -= OnBanning;
            Exiled.Events.Handlers.Player.Kicking -= OnKicking;
        }
    }
}