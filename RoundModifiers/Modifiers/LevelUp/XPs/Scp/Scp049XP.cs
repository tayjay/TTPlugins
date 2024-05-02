using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class Scp049XP : ScpXP, IResurrectEvent
{
    public void OnResurrect(ChangingRoleEventArgs ev)
    {
        foreach(Player p in Player.Get(p=>p.Role==RoleTypeId.Scp049))
            GiveXP(p, 100);
    }
}