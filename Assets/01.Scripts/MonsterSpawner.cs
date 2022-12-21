using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private float x, z;
    private float _time;
    private float _spawnCycle = 15f; 

    void Start()
    {
        InvokeRepeating("MonsterSpawn", 5f, _spawnCycle);
    }

    private void Update()
    {
        _time += Time.deltaTime; 
    }

    private void MonsterSpawn()
    {
        if(_time >= 10f)
        {
            _spawnCycle = 12f; 
        }
        else if(_time >= 20f)
        {
            _spawnCycle = 8f; 
        }
        else if(_time >= 30f)
        {
            _spawnCycle = 5f; 
        }
        x = Random.Range(-3f, 3f);
        z = Random.Range(-3f, 3f);

        float monsterSelect = Random.Range(0f, 1f);
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
