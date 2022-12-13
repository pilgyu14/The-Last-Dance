using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Test, 

    HitEffect_Red =100, 
}


public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<string, Pool<PoolableMono>> _pools = new Dictionary<string, Pool<PoolableMono>>();

    private Transform _trmParent;

    public PoolManager(Transform trmParent)
    {
        _trmParent = trmParent;
    }

    public void CreatePool(PoolableMono prefab, int count = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, _trmParent, count);
        if (_pools == null)
            _pools = new Dictionary<string, Pool<PoolableMono>>(); 
        _pools.Add(prefab.gameObject.name, pool);

    }

    public PoolableMono Pop(string prefabName)
    {
        if (!_pools.ContainsKey(prefabName))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }

        PoolableMono item = _pools[prefabName].Pop();
        item.Reset();
        return item;
    }

    public void Push(PoolableMono obj)
    {
        _pools[obj.name.Trim()].Push(obj);
    }
}
