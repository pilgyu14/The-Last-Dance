using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class AttackInfo
{
    public AttackType attackType;
    public AttackSO attackSO;
}


[System.Serializable]
public class AttackBase 
{
    // 캐싱 변수 
    private GameObject _owner; 

    private PlayerAnimation _agentAnimation;
    // 적의 경우 생각
    // attackSO.IsEnemy 있다 

    private MoveModule _moveModule;
    private FieldOfView _fov;

    // 인스펙터 참조 변수 
    public AttackInfo attackInfo; 

    public void Init(GameObject owner, PlayerAnimation playerAnimation, MoveModule moveModule,FieldOfView fov)
    {
        _owner = owner; 
        _agentAnimation = playerAnimation;
        _moveModule = moveModule;
        _fov = fov; 
    }
    public virtual void Attack() 
    {
        // 움직임 멈추고
        _moveModule.StopMove();
        
        // 애니메이션 실행 
        if(attackInfo.attackSO.animationFuncName != null)
        {
            Type type = typeof(PlayerAnimation);
            MethodInfo method = type.GetMethod(attackInfo.attackSO.animationFuncName, BindingFlags.Static | BindingFlags.Public);
            method?.Invoke(_agentAnimation, new object[] { });
        }

        // 공격 판정 
        _fov.FindTargets(attackInfo.attackSO.attackAngle, attackInfo.attackSO.attackRadius);  // 범위 안 몬스터 찾기 

        foreach (var target in _fov.TargetList)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable.GetDamaged(attackInfo.attackSO.attackDamage, _owner);
            // 데미지 텍스트 
            // 이펙트 
            // 흔들림 

            if (attackInfo.attackSO.isKnockbackAttack == true)
            {
                IKnockback knockback = target.GetComponent<IKnockback>();
            }
        }

        // 음? 
        _agentAnimation.Update_Zero(); 
    }

    
}

