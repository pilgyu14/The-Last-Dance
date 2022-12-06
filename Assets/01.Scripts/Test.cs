using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Material mat; 

    public Action A; 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeMat(); 
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeMat2();
        }
    }

    public void ChangeMat()
    {
        mat.SetFloat("_FullScreenIntensity", 0);
    }
    public void ChangeMat2()
    {
        mat.SetFloat("_FullScreenIntensity", 0.028f);
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
