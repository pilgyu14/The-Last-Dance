using System;
using UnityEngine; 

public class TimerModule
{
    private bool _isCheckTime = true;

    private float _curTime;
    private float _maxTime;
    private Action _timerEvent; // 최대 시간에 도달했을때 발생하는 이벤트 

    public TimerModule(float maxTime,Action callback)
    {
        this._maxTime = maxTime;
        this._timerEvent = callback; 
    }
    public void UpdateSomething()
    {
        if (_isCheckTime == false) return; 

        _curTime += Time.deltaTime; 
        if(_curTime >= _maxTime)
        {
            _timerEvent?.Invoke(); 
            _curTime = 0;
        }
    }

    public void ClearTime()
    {
        _curTime = 0; 
    }
}
