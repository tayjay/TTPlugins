using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Interfaces;
using PlayerRoles.PlayableScps.Scp049.Zombies;

namespace TTCore.Events.EventArgs;

public class ZombieAttackEventArgs : IExiledEvent
{
    public ZombieRole Role;
    private ReferenceHub _attacker;
    public Player Attacker => Player.Get(_attacker);
    public bool IsAllowed { get; set; } = true;
    
    public ZombieAttackEventArgs(ZombieRole role, ReferenceHub attacker)
    {
        Role = role;
        _attacker = attacker;
    }




}