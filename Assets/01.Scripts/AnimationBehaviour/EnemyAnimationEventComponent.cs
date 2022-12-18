using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEventComponent : MonoBehaviour
{
    public UnityEvent DeathEvent = null;
    public UnityEvent Attack1Event = null; 

    public void Attack1()
    {
        Attack1Event?.Invoke(); 
    }
    public void PlayDeathEvent()
    {
        DeathEvent?.Invoke(); 
    }
}
