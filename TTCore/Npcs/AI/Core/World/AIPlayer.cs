using Exiled.API.Features;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using TTCore.Npcs.AI.Core.Management;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Core.World;
public delegate void OnAIPlayerDamage(Player attacker);
public delegate void OnAIPlayerChangeRole(RoleTypeId role);
public class AIPlayer : MonoBehaviour
{
    /// <summary>
    /// Used for raycasting to check for walls, ground, etc. 
    /// This is just a more accessible and less confusing way of accessing FpcStateProcessor.Mask.
    /// </summary>
    public static LayerMask MapLayerMask => FpcStateProcessor.Mask;

    public Player Player { get; set; }
    
    public AIPlayerProfile Profile { get; set; }

    public PlayerRoleBase CurrentRole => ReferenceHub.roleManager.CurrentRole;

    public ReferenceHub ReferenceHub => Profile.ReferenceHub;

    public IFpcRole FirstPersonController
    {
        get
        {
            if (CurrentRole is IFpcRole fpc)
                return fpc;
            else
                return null;
        }
    }

    public OnAIPlayerDamage OnDamage;
    public OnAIPlayerChangeRole OnRoleChange;

    public AIMovementEngine MovementEngine;
    public AIModuleRunner ModuleRunner;
    public NavMeshAgent NavMeshAgent;

    private void Awake()
    {
        MovementEngine = gameObject.AddComponent<AIMovementEngine>();
        ModuleRunner = gameObject.AddComponent<AIModuleRunner>();
        NavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        NavMeshAgent.agentTypeID = 1;
        NavMeshAgent.radius = MovementEngine.CharCont.radius;
        NavMeshAgent.height = MovementEngine.CharCont.height;
        NavMeshAgent.speed = MovementEngine.CurrentSpeed;
        NavMeshAgent.acceleration = 40f;
        NavMeshAgent.angularSpeed = 120f;
        NavMeshAgent.autoBraking = false;
        NavMeshAgent.autoRepath = true;
        NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        NavMeshAgent.baseOffset = 1;
        NavMeshAgent.stoppingDistance = 2f;
    }

    public void Damage(Player attacker) => OnDamage?.Invoke(attacker);
    public void RoleChange(RoleTypeId role) => OnRoleChange?.Invoke(role);
}