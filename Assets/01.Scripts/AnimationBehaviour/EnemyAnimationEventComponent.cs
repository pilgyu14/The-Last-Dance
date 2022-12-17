using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEventComponent : MonoBehaviour
{
    public UnityEvent DeathEvent = null; 

    public void PlayDeathEvent()
    {
        DeathEvent?.Invoke(); 
    }
}
