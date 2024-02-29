using System;
using Exiled.API.Features;
using UnityEngine;

namespace TTCore.Npcs.AI.Behaviours;

public abstract class AIBehaviour : MonoBehaviour
{
    public abstract void UpdateBehaviour();
    
    public abstract void FixedUpdateBehaviour();
    
    
    public Priority BehaviourPriority { get; protected set; }

    public AIBehaviour(Priority priority)
    {
        this.BehaviourPriority = priority;
    }
    
    public Npc Owner { get; private set; }
    
    public void Init(Npc npc)
    {
        this.Owner = npc;
    }

    private void OnEnable()
    {
        this.Owner = Player.Get(this.gameObject) as Npc;
        Log.Debug("Initialized behaviour for " + Owner?.Nickname);
    }
    
    public enum Priority
    {
        Low,
        Medium,
        High
    }
}