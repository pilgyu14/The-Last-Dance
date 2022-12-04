using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void PlayFeedback(); // 피드백 재생 
    public abstract void FinishFeedback(); // 피드백 종료 
}
