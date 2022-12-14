using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IAgent
{
    public GameObject obj { get; }
    public AgentAudioPlayer AudioPlayer { get;}
    public NavMeshAgent NavMeshAgent { get;}
    public bool IsDie();
    public void OnDie(); 
}
