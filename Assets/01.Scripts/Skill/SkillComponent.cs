using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

/// <summary>
/// ���� Ÿ�Կ� ���� �ٸ��� �����Ұž� 
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
/// ���� Ÿ�԰� ������ ���������� �����ϴ� �Լ� ���� 
/// </summary>
[Serializable]
public class SkillComponent 
{
    [SerializeField,Header("��ų")]
    private List<AttackData> _attackDataList = new List<AttackData>(); 

    /// <summary>
    /// �Լ� ���� 
    /// </summary>
    /// <param name="attackType"></param>
    public void PlayAttackCallback(AttackType attackType)
    {
        GetAttackData(attackType).callback?.Invoke(); 
    }

    /// <summary>
    /// �Լ� �߰� 
    /// </summary>
    /// <param name="attackType"></param>
    /// <param name="callback"></param>
    public void AddAttackData(AttackType attackType, Action callback)
    {
        _attackDataList.Add(new AttackData(attackType, callback));
    }

    /// <summary>
    /// �Լ� ���� 
    /// </summary>
    /// <param name="attackType"></param>
    /// <param name="callback"></param>
    public void RemoveAttackData(AttackType attackType)
    {
        _attackDataList.Remove(GetAttackData(attackType)); 
    }

    /// <summary>
    /// AttackData ã�� 
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
