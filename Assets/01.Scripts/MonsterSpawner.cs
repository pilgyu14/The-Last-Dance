using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private float x, z;

    void Start()
    {
        InvokeRepeating("MonsterSpawn", 5f, 10f);
    }

    private void MonsterSpawn()
    {
        x = Random.Range(-3f, 3f);
        z = Random.Range(-3f, 3f);

        float monsterSelect = Random.value;
        Debug.Log(monsterSelect); 
        PoolableMono monster;
        if (monsterSelect <= 0.5f)
            monster = PoolManager.Instance.Pop("GhoulMonster");
        else
            monster = PoolManager.Instance.Pop("SlimeMonster");

        Vector3 pos = transform.position;
        pos.x += x;
        pos.z += z;
        monster.transform.position = pos;
    }
}
