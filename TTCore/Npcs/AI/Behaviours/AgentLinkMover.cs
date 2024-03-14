using System.Collections;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Behaviours;
public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola
}
public class AgentLinkMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;

    void Start()
    {
        Timing.RunCoroutine(DoStart());
    }
    
    public IEnumerator<float> DoStart()
    {
        Log.Info("Starting AgentLinkMover");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = true;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                Log.Info("Off mesh link detected");
                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return Timing.WaitUntilDone(NormalSpeed(agent));
                else if (method == OffMeshLinkMoveMethod.Parabola)
                    yield return Timing.WaitUntilDone(Parabola(agent, 2.0f, 0.5f));
                agent.CompleteOffMeshLink();
            }
            yield return Timing.WaitForOneFrame;
        }
    }

    public IEnumerator<float> NormalSpeed(NavMeshAgent agent)
    {
        Log.Info("Start moving off mesh link");
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return Timing.WaitForOneFrame;
        }
        Log.Info("Done moving off mesh link");
    }

    public IEnumerator<float> Parabola(NavMeshAgent agent, float height, float duration)
    {
        Log.Info("Start moving off mesh link");
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return Timing.WaitForOneFrame;
        }
        Log.Info("Done moving off mesh link");
    }
}