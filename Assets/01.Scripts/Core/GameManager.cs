using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private PoolingListSO _initList = null;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple GameManager is running");
        Instance = this;

        PoolManager.Instance = new PoolManager(transform);

        CreatePool();
    }

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
    }
}
