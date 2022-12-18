using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEventComponent : MonoBehaviour
{
    public UnityEvent DeathEvent = null;

    [Header("공격")]
    public UnityEvent Attack1Event = null;
    public UnityEvent TackleEvent = null; 
    public UnityEvent EndTackleEvent = null; 

    public void Attack1()
    {
        Attack1Event?.Invoke(); 
    }
    public void PlayDeathEvent()
    {
        DeathEvent?.Invoke(); 
    }
    public void PlayTackleEvent()
    {
        Debug.Log("러쉬");
        TackleEvent?.Invoke();
    }
    public void PlayEndTackleEvent()
    {
        Debug.Log("러쉬끝");
        EndTackleEvent?.Invoke(); 
    }
}
