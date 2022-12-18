using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격 판정 콜라이더 
/// </summary>
public class AttackCollider : MonoBehaviour
{
    private AttackBase _attackBase;
    private BoxCollider _collider;
    private List<GameObject> colObjs = new List<GameObject>(); // 충돌한 오브젝트 ( 한 번 충돌했으면 더 못하게 ) 
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        gameObject.SetActive(false); 
    }
    private void OnEnable()
    {
    }
    public void Init(AttackBase attackBase)
    {
        this._attackBase = attackBase;
    }

    public float GetAtkRange()
    {
        return _collider.center.z + (_collider.size.z * 0.5f); 
    }
    /// <summary>
    /// 오브젝트 활성화 비활성화 
    /// </summary>
    /// <param name="isActive"></param>
    public void ActiveCollider(bool isActive, bool isContinue = false)
    {
        Debug.Log(isActive + "콜라이더 활성화");
        colObjs.Clear();
        gameObject.SetActive(isActive);

        if (isActive == true && isContinue == false)
            StartCoroutine(DelayColliderFalse()); 
    }

    /// <summary>
    /// 이미 한 번 충돌했던 오브젝트인지 체크 ( 충돌했었으면 true) 
    /// </summary>
    private bool IsColObjs(GameObject obj)
    {
        Debug.Log("################33");
        GameObject findObj = colObjs.Find((x) => x == obj);

        return findObj != null;
    }

    /// <summary>
    /// 판단할 시간 기다려주기 꺼주기 ( not continue attack )
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayColliderFalse()
    {
        yield return new WaitForSeconds(0.1f);
        ColliderFalse(); 
    }

    /// <summary>
    /// 콜라이더 꺼주기 ( continue Attack - tackle, rush ) 
    /// </summary>
    public void ColliderFalse()
    {
        //yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("@@@@@@@@@@2");
        if (IsColObjs(col.gameObject) == true) return; // 충돌했었으면 리턴  
        Debug.Log("충돌");
        colObjs.Add(col.gameObject);
        _attackBase.AtkJudgeComponent.AttackJudge(col.transform); // 공격 피드백 실행 
    }
}
