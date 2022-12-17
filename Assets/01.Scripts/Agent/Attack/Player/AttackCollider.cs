using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격 판정 콜라이더 
/// </summary>
public class AttackCollider<T> : MonoBehaviour where T : IDamagable
{
    private AttackBase<T> _attackBase;
    private Collider _collider;
    private List<GameObject> colObjs = new List<GameObject>(); // 충돌한 오브젝트 ( 한 번 충돌했으면 더 못하게 ) 

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        colObjs.Clear();
    }
    public void Init(AttackBase<T> attackBase)
    {
        this._attackBase = attackBase;
    }

    /// <summary>
    /// 오브젝트 활성화 비활성화 
    /// </summary>
    /// <param name="isActive"></param>
    public void ActiveCollider(bool isActive, bool isContinue = false)
    {
        Debug.Log(isActive + "콜라이더 활성화");
        gameObject.SetActive(isActive);

        if (isActive == true && isContinue == false)
            StartCoroutine(ColliderFalse());
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

    IEnumerator ColliderFalse()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("@@@@@@@@@@2");
        //if (IsColObjs(col.gameObject) == true) return; // 충돌했었으면 리턴  
        Debug.Log("충돌");
        colObjs.Add(col.gameObject);
        _attackBase.AtkJudgeComponent.AttackJudge(col.transform); // 공격 피드백 실행 
    }
}
