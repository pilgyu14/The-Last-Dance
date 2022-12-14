using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopFeedback : Feedback
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _changeTime; 
    [SerializeField]
    private float _delay; 
    [SerializeField]
    private bool _restoreTime;

    private void Start()
    {
        _restoreTime = false; 
    }

    private void Update()
    {
        if(_restoreTime == true)
        {
            if(Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * _speed; 
            }
            else
            {
                Time.timeScale = 1f;
                _restoreTime = false; 
            }
        }
    }

    // 0.05f, 10, 0.1f 
    public void StopTime()
    {
        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            _restoreTime = true; 
        }

        Time.timeScale = _changeTime; 
    }

    IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        _restoreTime = true; 
    }

    public override void PlayFeedback()
    {
        StopTime(); 
    }

    public override void FinishFeedback()
    {
        StopCoroutine(StartTimeAgain(_delay));
    }
}
