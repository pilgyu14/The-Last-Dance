using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/AttackSO/BaseAttackSO")]
public class AttackSO : ScriptableObject
{
    public bool isEnemy; 
    public bool isDefaultAttack; 

    public bool isMultipleAttack;
    public bool isBombAttack;
    public bool isKnockbackAttack; 

    public int attackDamage;
    public float knockbackPower; 

    public float attackRadius;
    public float attackAngle; 
    public float attackDelay;

    public GameObject hitEffect;
    public GameObject swingEffect;

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
