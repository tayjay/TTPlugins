using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Extensions;

namespace RoundModifiers.Modifiers;

public class Peanuts : Modifier
{
    
    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if(ev.NewRole.GetSide()==Side.Scp && ev.NewRole != RoleTypeId.Scp173)
            ev.NewRole = RoleTypeId.Scp173;
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
        Name = "Peanuts",
        FormattedName = "<color=red><size=85%>Peanuts</size></color>",
        Aliases = new []{"nut","173"},
        Description = "All SCPs are 173",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = -2,
        Category = Category.ScpRole
    };
}