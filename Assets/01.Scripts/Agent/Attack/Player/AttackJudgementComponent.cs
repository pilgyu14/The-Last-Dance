using UnityEngine;

// 공격 판정 ( 피드백 ) 
public class AttackJudgementComponent
{
    private IAgent _owner;
    private AttackInfo _attackInfo;

    public void Init(IAgent owner, AttackInfo attackInfo)
    {
        this._owner = owner;
        this._attackInfo = attackInfo; 
    }

    /// <summary>
    /// 공격 피드백 
    /// </summary>
    public void AttackJudge(Transform target)
    {
        Debug.LogError("공격 피드백"); 
        IDamagable damagable = target.GetComponent<IDamagable>();
        damagable.GetDamaged(_attackInfo.attackSO.attackDamage, _owner.obj);

        // 이펙트 
        Vector3 hitPos = target.position + Vector3.up * 1;
        EffectComponent effectObj = PoolManager.Instance.Pop(_attackInfo.attackSO.hitEffect.name) as EffectComponent;
        effectObj.SetPosAndRot(hitPos, Vector3.zero);

        _attackInfo.feedbackCallbackHit?.Invoke(); // 스탑 히트, 흔들림 

        // 맞은 위치
        // 히트 오디오 재생 
        _owner.AudioPlayer.PlayClip(_attackInfo.attackSO.hitClip);

        // 플레이어 공격이라면 
        // 데미지 텍스트 
        if (_attackInfo.attackSO.isEnemy == false)
        {
            DamageText damageText = PoolManager.Instance.Pop("DamageText") as DamageText; // 풀링으로 
            damageText.SetText(_attackInfo.attackSO.attackDamage, hitPos + new Vector3(0, 3f, 0), false, Color.white);
        }



        if (_attackInfo.attackSO.isKnockbackAttack == true)
        {
            Vector3 dir = (target.position - _owner.obj.transform.position).normalized;
            IKnockback knockback = target.GetComponent<IKnockback>();
            knockback.Knockback(dir, _attackInfo.attackSO.knockbackPower, 0.2f);
        }
    }
}

