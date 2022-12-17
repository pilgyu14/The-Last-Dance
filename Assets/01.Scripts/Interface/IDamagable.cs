using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    public bool IsEnemy { get; } // ���� ���̸� ���� X 
    public Vector3 HitPoint { get;  } 
    public void GetDamaged(int damage, GameObject damageDealer);
    public void EndHit();
}
