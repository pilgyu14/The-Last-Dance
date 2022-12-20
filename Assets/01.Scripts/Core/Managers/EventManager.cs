using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventsType
{
    LoadMainScene = 100,
    LoadFloor1Scene = 101,
    LoadFloor2Scene = 102, 

    // UI 
    UpdateHpUI = 301,
    UpdateExpUI = 302,

    SetActiveSkillInput = 500,
    CheckPassiveSkill = 501, 
    CheckActiveSkill = 502,
    ClearEvents = 10000
}
public class EventManager : MonoSingleton<EventManager>
{

    private Dictionary<EventsType, Action> eventDictionary = new Dictionary<EventsType, Action>();
    private Dictionary<EventsType, Action<object>> eventParamDictionary = new Dictionary<EventsType, Action<object>>();

    private void Start()
    {
        StartListening(EventsType.ClearEvents, ClearEvents);
    }

    /// <summary>
    /// 이벤트 함수 등록하기 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void StartListening(EventsType eventName, Action listener)
    {
        Action thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //기존 이벤트에 더 많은 이벤트 추가 
            thisEvent += listener;

            //딕셔너리 업데이트
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //처음으로 딕셔너리에 이벤트 추가 
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public void StartListening(EventsType eventName, Action<object> listener)
    {
        Action<object> thisEvent;

        if (Instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.eventParamDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventParamDictionary.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 이벤트 함수 해제하기 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void StopListening(EventsType eventName, Action listener)
    {
        if (Instance == null)
        {
            return;
        }
        Action thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //기존 이벤트에서 이벤트 제거
            thisEvent -= listener;

            //이벤트 업데이트 
            Instance.eventDictionary[eventName] = thisEvent;
        }

    }

    public void StopListening(EventsType eventName, Action<object> listener)
    {
        if (Instance == null)
        {
            return;
        }
        Action<object> thisEvent;
        if (Instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;

            Instance.eventParamDictionary[eventName] = thisEvent;
        }
    }
    /// <summary>
    /// 이벤트 함수 실행 
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent(EventsType eventName)
    {
        Action thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("이벤트 실행!");
            thisEvent?.Invoke();
        }
        else
        {
            Debug.LogError("빈 이벤트입니다");
        }
    }

    public void TriggerEvent(EventsType eventName, object param)
    {
        Action<object> thisEvent = null;
        if (Instance.eventParamDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
        else
        {
            Debug.LogError("빈 이벤트입니다");
        }
    }

    public void ClearEvents()
    {
        Instance.eventDictionary.Clear();
        Instance.eventParamDictionary.Clear();
        StartListening(EventsType.ClearEvents, ClearEvents);

    }
}
