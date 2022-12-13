using UnityEngine;

// 공격 판정 ( 피드백 ) 
public class AttackJudgementComponent
{
    private GameObject _owner;
    private AttackInfo _attackInfo;

    public void Init(GameObject owner, AttackInfo attackInfo)
    {
        this._owner = owner;
        this._attackInfo = attackInfo; 
    }

    /// <summary>
    /// 공격 피드백 
    /// </summary>
    public void AttackJudge(Transform target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        damagable.GetDamaged(_attackInfo.attackSO.attackDamage, _owner);

        // 이펙트 
        Vector3 hitPos = target.position;
        EffectComponent effectObj = PoolManager.Instance.Pop(_attackInfo.attackSO.hitEffect.name) as EffectComponent;
        effectObj.SetPosAndRot(hitPos, Vector3.zero); 

        // 맞은 위치
        // 히트 오디오 재생 

        // 플레이어 공격이라면 
        // 데미지 텍스트 
        if (_attackInfo.attackSO.isEnemy == false)
        {
            DamageText damageText = PoolManager.Instance.Pop("DamageText") as DamageText; // 풀링으로 
            damageText.SetText(_attackInfo.attackSO.attackDamage, _owner.transform.position + new Vector3(0, 0.5f, 0), Color.white, false);
        }


        _attackInfo.feedbackCallbackHit?.Invoke(); 

        if (_attackInfo.attackSO.isKnockbackAttack == true)
        {
            Vector3 dir = (target.position - _owner.transform.position).normalized;
            IKnockback knockback = target.GetComponent<IKnockback>();
            knockback.Knockback(dir, _attackInfo.attackSO.knockbackPower, 0.2f);
        }
    }
}

