using System.Linq;
using TTCore.Npcs.AI.Core.World.Targetables;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World.AIModules;


public class AIChaseEnemy : AIModuleBase
{
    AIPathfinder Pathfinder;

    Vector3 lastEnemyPosition;

    public override void Init()
    {
        Pathfinder = Parent.GetModule<AIPathfinder>();
        Parent.OnLostEnemy += OnLostEnemy;
        Tags = [AIBehaviorBase.AggroTag];
    }

    /*public override bool Condition()
    {
        if (Parent.Player.IsScp) return true; // SCPs don't back down
        if(Parent.Player.Items.Any(i=>i.IsWeapon)) return true; // If the AI has a weapon, it will chase the enemy
        return false; // If the AI doesn't have a weapon, it will not chase the enemy
    }*/

    private void OnLostEnemy(TargetableBase enemy, Vector3 pos)
    {
        lastEnemyPosition = pos;
    }

    public override void OnDisabled() { }

    public override void OnEnabled()
    {
        Pathfinder.SetDestination(lastEnemyPosition);
        Parent.MovementEngine.State = PlayerRoles.FirstPersonControl.PlayerMovementState.Sprinting;
    }

    public override void Tick() { }
}
