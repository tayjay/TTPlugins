using System.Collections.Generic;
using TTCore.Npcs.AI.Core.Management;
using TTCore.Npcs.AI.Core.World.AIModules;
using UnityEngine;

namespace TTAdmin.Data;

public class AIData
{
    
    public bool HasEnemyTarget { get; set; }
    public Vector3 EnemyTarget { get; set; }
    public bool HasFollowTarget { get; set; }
    public Vector3 FollowTarget { get; set; }
    public List<string> Modules { get; set; }
    public Vector3 WishDir { get; set; }
    public bool HasPathfinder { get; set; }
    public Vector3 PathfinderTargetLocation { get; set; }
    public PlayerData Player { get; set; }


    public AIData(AIPlayerProfile profile)
    {
        Player = new PlayerData(profile.Player);
        Modules = new();
        foreach (var module in profile.WorldPlayer.ModuleRunner.Modules)
            Modules.Add(module.GetType().Name);
        HasEnemyTarget = profile.WorldPlayer.ModuleRunner.HasEnemyTarget;
        EnemyTarget = profile.WorldPlayer.ModuleRunner.EnemyTarget?.GetPosition(profile.WorldPlayer.ModuleRunner) ?? Vector3.zero;
        HasFollowTarget = profile.WorldPlayer.ModuleRunner.HasFollowTarget;
        FollowTarget = profile.WorldPlayer.ModuleRunner.FollowTarget?.GetPosition(profile.WorldPlayer.ModuleRunner) ?? Vector3.zero;
        WishDir = profile.WorldPlayer.MovementEngine.WishDir;
        AIPathfinder pathfinder = profile.WorldPlayer.ModuleRunner.GetModule<AIPathfinder>();
        HasPathfinder = pathfinder != null;
        PathfinderTargetLocation = pathfinder?.TargetLocation ?? Vector3.zero;
        
        
    }
}