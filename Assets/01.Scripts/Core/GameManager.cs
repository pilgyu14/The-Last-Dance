using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // 컴포넌트 
    private CoroutineComponent _coroutineComponent; 

    [SerializeField]
    private PoolingListSO _initList = null;

    private Transform _playerTrm;

    // 프로퍼티 
    public CoroutineComponent CoroutineComponent => _coroutineComponent; 
    public Transform PlayerTrm
    {
        get
        {
            if(_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return _playerTrm;
        }
    }

    private void Awake()
    {
        //if (Instance != null)
        //    Debug.LogError("Multiple GameManager is running");
        //Instance = this;

        PoolManager.Instance = new PoolManager(transform); //풀매니저 생성
        coroutineComponent = new CoroutineComponent(); 

        CreatePool();
    }

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
    }

    public void BegineCoroutine()
    {
        StartCoroutine(coroutineComponent.coroutine); 
    }
}
