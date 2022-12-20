using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[Serializable]
public class SkillInfo
{
    [Header("������")]
    public Sprite icon; // ��ų ������ 
    public string skillName; // ��ų �̸�
    [TextArea(1, 4)]
    public string skillDescription; // ��ų ���� 
    public int skillLevel; // ��ų ����
    public int requiredLevel; // ��ų ȹ�濡 �ʿ� ���� ( �� �� ����? )

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
    [Header("üũ ����"),Space(10)]
    public bool isEnemy;
    public bool isRayAttack;
    public bool isContinueColliderAttack; // �ݶ��̴� �����ε� ���������� �Ѱ� ������ ��) ��Ŭ 
    public bool isDefaultAttack; 

    public bool isMultipleAttack;
    public bool isBombAttack;
    public bool isKnockbackAttack;

    [Header("�� ���� ����"), Space(10)]
    public int attackDamage;
    public float knockbackPower;
    public float rushPower; 

    [Header("���� ���� ����"), Space(10)]
    public float attackRadius;
    public float attackAngle; 
    public float attackCoolTime;

    [Header("�ִϸ��̼� ����"), Space(10)]
    public AnimationClip animationClip; 

    [Tooltip("�⺻�����̸� ����")]
    public string animationName; 
    public string animationFuncName;

    [Header("����Ʈ ����"), Space(10)]
    public GameObject hitEffect;
    public GameObject swingEffect;

    [Header("���� ����"), Space(10)]
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
