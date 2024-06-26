﻿using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIScp939Module : AIMeleeScpModuleBase<Scp939Role, Scp939ClawAbility>
{
    public override RoleTypeId[] Roles => [RoleTypeId.Scp939];

    public override bool CanAttack() =>
        Parent.GetDistance(Parent.EnemyTarget) <= TryAttackRange
        && Attacker.Cooldown.IsReady;
    
    
    
}