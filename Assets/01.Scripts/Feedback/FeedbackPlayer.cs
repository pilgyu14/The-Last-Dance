using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FeedbackPlayer : MonoBehaviour
{
    [SerializeField]
    private List<Feedback> _feedbackList = new List<Feedback>(); 

    public void PlayAllFeedbacks()
    {
        FinishAllFeedbacks(); 
        foreach (var  f in _feedbackList)
        {
            f.PlayFeedback(); 
        }
    }

    public void FinishAllFeedbacks()
    {
        foreach (var f in _feedbackList)
        {
            f.FinishFeedback();
        }
    }
}
