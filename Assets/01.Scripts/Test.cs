using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public Action A; 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            A?.Invoke(); 
        }
    }

    [ContextMenu("Add")]
    public void Add()
    {
        A += PrintA;
        A += PrintB;
    }

    [ContextMenu("Remove")]
    public void Remove()
    {
        A -= PrintA;
        A -= PrintB; 
    }

    
    public void PrintA()
    {
        Debug.Log("A");
    }
    public void PrintB()
    {
        Debug.Log("B");
    }
}
