using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainModule : MonoBehaviour
{
    public static MainModule player;

    public bool isPlayer = false;

    void Start()
    {
        if (isPlayer == true)
            player = this;


    }

    void Update()
    {
        
    }
}
