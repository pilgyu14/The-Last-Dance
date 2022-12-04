using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/AttackSO/BaseAttackSO")]
public class AttackSO : ScriptableObject
{
    [Header("üũ ����"),Space(10)]
    public bool isEnemy;
    public bool isDefaultAttack; 

    public bool isMultipleAttack;
    public bool isBombAttack;
    public bool isKnockbackAttack;

    [Header("�� ���� ����"), Space(10)]
    public int attackDamage;
    public float knockbackPower;

    [Header("���� ���� ����"), Space(10)]
    public float attackRadius;
    public float attackAngle; 
    public float attackDelay;

    [Header("�ִϸ��̼� ����"), Space(10)]
    
    [Tooltip("�⺻�����̸� ����")]
    public string animationName; 
    public string animationFuncName;

    [Header("����Ʈ ����"), Space(10)]
    public GameObject hitEffect;
    public GameObject swingEffect;

    [Header("���� ����"), Space(10)]
    public AudioClip swingClip;
    public AudioClip hitClip;

    public Sprite icon; 
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
