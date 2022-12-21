using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    void Start()
    {
        InvokeRepeating("MonsterSpawn", 2f, 2f);
    }

    private void MonsterSpawn()
    {
        
    }
}
