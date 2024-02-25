using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class NoKOS : Modifier
    {
        
        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Player.Role.Type == RoleTypeId.ClassD || ev.Player.Role.Type == RoleTypeId.Scientist)
            {
                if(ev.Attacker.Role.Team == Team.SCPs)
                {
                    return;
                }
                if(ev.Attacker.Role.Team == ev.Player.Role.Team)
                {
                    return;
                }
                if(ev.DamageHandler.IsFriendlyFire)
                {
                    return;
                }
                ev.Player.Handcuff(ev.Attacker);
                if (ev.Player.IsCuffed)
                {
                    ev.Amount = 0;
                    ev.IsAllowed = false;
                }
            }
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "NoKOS",
            FormattedName = "<color=green>No KOS</color>",
            Aliases = new[] { "kos" },
            Description = "Class-D and Scientists cannot be killed and are arrested when shot, unless holding an illegal item",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}