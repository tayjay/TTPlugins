using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Pathing;

public class AIPathFinder : MonoBehaviour
{
   //private List<ActionPF> actionList = new List<ActionPF>();
   private int currentActionIndex = 0;
   private Player player;
   private NavMeshAgent navMeshAgent;


   void Start()
   {
       Log.Info("Loaded");
       player = Player.Get(GetComponent<ReferenceHub>());
       navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
       var characterController = GetComponent<CharacterController>();
       navMeshAgent.radius = characterController.radius;
       navMeshAgent.acceleration = 40f;
       navMeshAgent.speed = 8.5f;
       navMeshAgent.angularSpeed = 120f;
       navMeshAgent.stoppingDistance = 0.3f;
       navMeshAgent.baseOffset = 1;
       navMeshAgent.autoRepath = true;
       navMeshAgent.autoTraverseOffMeshLink = true;
       navMeshAgent.height = characterController.height;
       navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
       navMeshAgent.agentTypeID = 0;
       Timing.RunCoroutine(AIHandler(), Segment.FixedUpdate);
   }

   //public void AddPathGoal(ActionPF action) { actionList.Add(action); }

   IEnumerator<float> AIHandler()
   {
       Log.Info("AI Handler On");
       while (true)
       {
           if (player.CurrentRoom.Type != Exiled.API.Enums.RoomType.HczServers)
               navMeshAgent.baseOffset = 1.0f;
           else
               navMeshAgent.baseOffset = 1.4f;

           //if (currentActionIndex < actionList.Count && !player.IsDead)
           {
               /*ActionPF currentAction = actionList[currentActionIndex];

               if(!currentAction.IsInProgress())
               {
                   Log.Info($"Starting");
                   //StartCoroutine(currentAction.ExecuteCoroutine(navMeshAgent, player));
               }

               if (currentAction.IsDone())
               {
                   currentActionIndex++;
               }*/
           }

           yield return Timing.WaitForOneFrame;
       }
   }
}