using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/AttackSO"]
public class AttackSO : ScriptableObject
{
    public bool isMultipleAttack;
    public bool isBombAttack; 

    public float attackDamage;
    public float attackRange;
    public float attackDelay;

    public GameObjec hitEffect;

    public AudioClip shakeClip;
    public AudioClip hitClip;

    public Sprite icon; 
}

