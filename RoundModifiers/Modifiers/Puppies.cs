using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Extensions;

namespace RoundModifiers.Modifiers
{
    public class Puppies : Modifier
    {
        public float Size => RoundModifiers.Instance.Config.Puppies_SCPScale;
        public float HealthMultiplier => RoundModifiers.Instance.Config.Puppies_SCPHealthMultiplier;
        public bool AffectScp079 => RoundModifiers.Instance.Config.Puppies_AffectScp079;
        public bool AffectScp3114 => RoundModifiers.Instance.Config.Puppies_AffectScp3114;
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            
            if (ev.NewRole == RoleTypeId.Scp049 || (ev.NewRole == RoleTypeId.Scp079 && AffectScp079) || ev.NewRole == RoleTypeId.Scp096 ||
                ev.NewRole == RoleTypeId.Scp106 || ev.NewRole == RoleTypeId.Scp173 || (ev.NewRole == RoleTypeId.Scp3114 && AffectScp3114))
            {
                ev.NewRole = RoleTypeId.Scp939;
            }

            if (ev.NewRole == RoleTypeId.Scp939)
            {
                Timing.CallDelayed(1f, () =>
                {
                    Log.Info("Setting " + ev.Player.Nickname + " size to "+Size);
                    ev.Player.MaxHealth *= HealthMultiplier;
                    ev.Player.Health = ev.Player.MaxHealth;
                    ev.Player.ChangeSize(Size);
                });
            }
            
        }
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
        }

        public override ModInfo ModInfo { get; } = new ModInfo
        {
            Name = "Puppies",
            Description = "All SCPs are 939s",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
    }
}