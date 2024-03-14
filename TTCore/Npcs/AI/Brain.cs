using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using TTCore.Npcs.AI.Behaviours;
using TTCore.Npcs.AI.Pathing;
using UnityEngine;

namespace TTCore.Npcs.AI;

public class Brain : MonoBehaviour
{
    protected GameObject GameObject;
    protected Npc Npc;
    protected List<AIBehaviour> Behaviours;
        
    private void Start()
    {
        
        Behaviours = new List<AIBehaviour>();
        //AddBehaviourOfType<DummyBehaviour>();
        //AddBehaviourOfType<MoveBehaviour>();
        
        if(Npc == null && GameObject != null)
            this.Npc = Player.Get(GameObject) as Npc;
        else if(GameObject == null && Npc != null)
            this.GameObject = Npc.GameObject;
        else
            throw new System.Exception("GameObject and Npc are both null!");
        
        Log.Debug("Starting Brain for " + (Npc != null ? Npc.Nickname : "Unknown")+ " with "+Behaviours.Count+" behaviours");
    }
        
    public void Init(GameObject gameObject)
    {
        this.GameObject = gameObject;
    }

    public void Init(Npc npc)
    {
        this.Npc = npc;
        this.GameObject = npc.GameObject;
        
        Custom.OnSetupNpcBrain(new SetupNpcBrainEventArgs(npc));
    }

    public void FixedUpdate()
    {
        //Log.Debug("FixedUpdate for "+Behaviours.Count+" behaviours");
        foreach (var behaviour in Behaviours)
            behaviour.FixedUpdateBehaviour();
    }
    
    public void Update()
    {
        //Log.Debug("Update for "+Behaviours.Count+" behaviours");
        foreach (var behaviour in Behaviours)
            behaviour.UpdateBehaviour();
    }
    
    public void AddBehaviourOfType<T>() where T : AIBehaviour
    {
        Log.Debug("Adding behaviour of type " + typeof(T).Name);
        Behaviours.Add(GameObject.AddComponent<T>());
    }
    
    public void RemoveBehaviourOfType<T>() where T : AIBehaviour
    {
        if (GameObject.TryGetComponent(out T behaviour))
        {
            Behaviours.Remove(behaviour);
            Destroy(behaviour);
        }
    }
    
    public AIBehaviour GetBehaviourOfType<T>() where T : AIBehaviour
    {
        return Behaviours.Find(behaviour => behaviour.GetType() == typeof(T));
    }

    private void OnEnable()
    {
        //
    }
        
    private void OnDisable()
    {
        //
    }

    private void OnDestroy()
    {
        //
    }
}