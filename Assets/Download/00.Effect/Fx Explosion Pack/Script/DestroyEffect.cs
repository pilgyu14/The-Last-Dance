using UnityEngine;
using System.Collections;

public class DestroyEffect : PoolableMono
{
    public override void Reset()
    {
        Invoke("Invoke", 2f);
    }

    public void Push()
    {
        PoolManager.Instance.Push(this);
    }
}
