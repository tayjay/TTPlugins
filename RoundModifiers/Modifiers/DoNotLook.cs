using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Extensions;

namespace RoundModifiers.Modifiers;

public class DoNotLook : Modifier
{
    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if(ev.NewRole.GetSide()==Side.Scp && ev.NewRole != RoleTypeId.Scp096)
            ev.NewRole = RoleTypeId.Scp096;
        if (ev.NewRole == RoleTypeId.Scp096)
        {
            Timing.CallDelayed(1f, () =>
            {
                //ev.Player.MaxHealth = HealthStart;
                //ev.Player.Health = ev.Player.MaxHealth;
                //ev.Player.HumeShieldStat.CurValue = HumeStart;
                //ev.Player.HumeShield = HumeStart;
                ev.Player.ChangeSize(0.6f);
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
        Name = "DoNotLook",
        FormattedName = "<color=red><size=85%>Don't Look</size></color>",
        Aliases = new []{"dontlook","096"},
        Description = "All SCPs are 096",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = -2,
        Category = Category.ScpRole
    };
}