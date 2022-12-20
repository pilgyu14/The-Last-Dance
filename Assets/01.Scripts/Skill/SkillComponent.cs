using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

/// <summary>
/// 공격 타입에 따라 다르게 실행할거야 
/// AttackType, callback
/// </summary>
[Serializable]
public class AttackData
{
    public AttackType attackType;
    public Action callback;

    public AttackData(AttackType attackType, Action callback)
    {
        this.attackType = attackType;
        this.callback = callback;
    }
}


/// <summary>
/// 공격 타입과 공격을 직접적으로 수행하는 함수 관리 
/// </summary>
[Serializable]
public class SkillComponent 
{
    [SerializeField,Header("스킬")]
    private List<AttackData> _attackDataList = new List<AttackData>(); 

    /// <summary>
    /// 함수 실행 
    /// </summary>
    /// <param name="attackType"></param>
    public void PlayAttackCallback(AttackType attackType)
    {
        GetAttackData(attackType).callback?.Invoke(); 
    }

    /// <summary>
    /// 함수 추가 
    /// </summary>
    /// <param name="attackType"></param>
    /// <param name="callback"></param>
    public void AddAttackData(AttackType attackType, Action callback)
    {
        _attackDataList.Add(new AttackData(attackType, callback));
    }

    /// <summary>
    /// 함수 삭제 
    /// </summary>
    /// <param name="attackType"></param>
    /// <param name="callback"></param>
    public void RemoveAttackData(AttackType attackType)
    {
        _attackDataList.Remove(GetAttackData(attackType)); 
    }

    /// <summary>
    /// AttackData 찾기 
    /// </summary>
    /// <param name="attackType"></param>
    /// <returns></returns>
    public AttackData GetAttackData(AttackType attackType)
    {
        foreach (var attackData in _attackDataList)
        {
            if (attackData.attackType == attackType)
            {
                return attackData;
            }
        }
        return null; 
    }
}
