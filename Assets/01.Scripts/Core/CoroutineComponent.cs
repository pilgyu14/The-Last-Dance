using System;
using System.Collections;
using UnityEngine;

public class CoroutineComponent
{
    public IEnumerator coroutine;

    public void SetCoroutine(IEnumerator co)
    {
        this.coroutine = co; 
    }
}

