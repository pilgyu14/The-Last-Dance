using System;
using System.Collections.Generic;
using UnityEngine; 

/// <summary>
/// 공격 판정 콜라이더 
/// </summary>
public class AttackCollider : MonoBehaviour
{
    private AttackBase _attackBase; 
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
    public void Init(AttackBase attackBase)
    {
        this._attackBase = attackBase;
    }

    /// <summary>
    /// 오브젝트 활성화 비활성화 
    /// </summary>
    /// <param name="isActive"></param>
    public void ActiveCollider(bool isActive, bool isContinue = false)
    {
        gameObject.SetActive(isActive);
     
        if(isActive == true && isContinue == false)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// 이미 한 번 충돌했던 오브젝트인지 체크 ( 충돌했었으면 true) 
    /// </summary>
    private bool IsColObjs(GameObject obj)
    {
       GameObject findObj = colObjs.Find((x) => x == obj);

        return findObj == null; 
    }

    private void OnTriggerEnter(Collider other)
    {

        if (IsColObjs(other.gameObject) == true) return; // 충돌했었으면 리턴  

        colObjs.Add(other.gameObject);
        _attackBase.AtkJudgeComponent.AttackJudge(other.transform); // 공격 피드백 실행 
    }
}
