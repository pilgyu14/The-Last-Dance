using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[Serializable]
public class SkillInfo
{
    [Header("아이콘")]
    public Sprite icon; // 스킬 아이콘 
    public string skillName; // 스킬 이름
    [TextArea(1, 4)]
    public string skillDescription; // 스킬 설명 
    public int skillLevel; // 스킬 레벨
    public int requiredLevel; // 스킬 획득에 필요 레벨 ( 안 쓸 수도? )

    public void Copy(SkillInfo skill)
    {
        this.icon = skill.icon;
        this.skillName = skill.skillName;
        this.skillDescription = skill.skillDescription;
        this.skillLevel = skill.skillLevel;
        this.requiredLevel = skill.requiredLevel;
    }
}


[CreateAssetMenu(menuName ="SO/AttackSO/BaseAttackSO")]
public class AttackSO : ScriptableObject
{
    [Header("체크 변수"),Space(10)]
    public bool isEnemy;
    public bool isRayAttack;
    public bool isContinueColliderAttack; // 콜라이더 공격인데 지속적으로 켜고 있을지 예) 태클 
    public bool isDefaultAttack; 

    public bool isMultipleAttack;
    public bool isBombAttack;
    public bool isKnockbackAttack;

    [Header("힘 관련 변수"), Space(10)]
    public int attackDamage;
    public float knockbackPower;
    public float rushPower; 

    [Header("공격 판정 변수"), Space(10)]
    public float attackRadius;
    public float attackAngle; 
    public float attackCoolTime;

    [Header("애니메이션 변수"), Space(10)]
    public AnimationClip animationClip; 

    [Tooltip("기본공격이면 설정")]
    public string animationName; 
    public string animationFuncName;

    [Header("이펙트 변수"), Space(10)]
    public GameObject hitEffect;
    public GameObject swingEffect;

    [Header("사운드 변수"), Space(10)]
    public AudioClip swingClip;
    public AudioClip hitClip;

    public SkillInfo skillInfo; 

}

[CreateAssetMenu(menuName = "SO/AttackSO/SpawnAttackSO")]
public class SpawnObjSO: ScriptableObject
{
    public bool isMultiple;     

    public float attackDelay; 

    public AudioClip spawnClip; 
    public GameObject spawnEffect; 
    public GameObject spawnObj; 
}
