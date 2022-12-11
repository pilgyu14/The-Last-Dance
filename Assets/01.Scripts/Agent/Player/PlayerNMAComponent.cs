using UnityEngine;
using UnityEngine.AI;
public class PlayerNMAComponent
{
    private NavMeshAgent _agent;
    public void Init(NavMeshAgent agent)
    {
        this._agent = agent; 
    }

    public void SetNavMeshAgent(float baseOffset, float height)
    {
        _agent.baseOffset = baseOffset;
        _agent.height = height; 
    }
}

