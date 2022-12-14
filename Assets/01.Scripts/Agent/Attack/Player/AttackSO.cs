using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/AttackSO/BaseAttackSO")]
public class AttackSO : ScriptableObject
{
    [Header("체크 변수"),Space(10)]
    public bool isEnemy;
    public bool isRayAttack; 
    public bool isDefaultAttack; 

    public bool isMultipleAttack;
    public bool isBombAttack;
    public bool isKnockbackAttack;

    [Header("힘 관련 변수"), Space(10)]
    public int attackDamage;
    public float knockbackPower;

    [Header("공격 판정 변수"), Space(10)]
    public float attackRadius;
    public float attackAngle; 
    public float attackCoolTime;

    [Header("애니메이션 변수"), Space(10)]
    
    [Tooltip("기본공격이면 설정")]
    public string animationName; 
    public string animationFuncName;

    [Header("이펙트 변수"), Space(10)]
    public GameObject hitEffect;
    public GameObject swingEffect;

    [Header("사운드 변수"), Space(10)]
    public AudioClip swingClip;
    public AudioClip hitClip;

    [Header("아이콘")]
    public Sprite icon;
    public int requiredLevel; 
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
