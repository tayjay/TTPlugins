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
        //public float HealthMultiplier => RoundModifiers.Instance.Config.Puppies_SCPHealthMultiplier;
        public float HealthStart => RoundModifiers.Instance.Config.Puppies_Scp939HealthStart;
        public float HumeStart => RoundModifiers.Instance.Config.Puppies_Scp939HumeStart;
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
                    ev.Player.MaxHealth = HealthStart;
                    ev.Player.Health = ev.Player.MaxHealth;
                    //ev.Player.HumeShieldStat.CurValue = HumeStart;
                    ev.Player.HumeShield = HumeStart;
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
            FormattedName = "<color=red><size=85%>Puppies</size></color>",
            Aliases = new []{"pup"},
            Description = "All SCPs are mini 939s",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = -2,
            Category = Category.ScpRole | Category.Scale | Category.Health
        };
    }
}