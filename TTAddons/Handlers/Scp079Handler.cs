using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp079;
using TTCore.API;

namespace TTAddons.Handlers
{
    public class Scp079Handler : IRegistered
    {
        public bool IsEnabled => TTAddons.Instance.Config.EnableScp079XpScaling;

        public int BasePlayerCount => TTAddons.Instance.Config.Scp079BasePlayerCount;

        
        public void Scp079GainExperience(GainingExperienceEventArgs ev)
        {
            if(!IsEnabled) return;
            int currentPlayerCount = Player.List.Count;
            //Want to scale experience based on player count. More players mean less experience. Less players mean more experience.
            //This is to balance the game for SCP-079, as it can be very difficult to level up when there are few players.
            ev.Amount *= (BasePlayerCount / currentPlayerCount);
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Scp079.GainingExperience += Scp079GainExperience;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Scp079.GainingExperience -= Scp079GainExperience;
        }
    }
}