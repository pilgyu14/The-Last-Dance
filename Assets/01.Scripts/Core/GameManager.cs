using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // ������Ʈ 
    private CoroutineComponent _coroutineComponent; 

    [SerializeField]
    private PoolingListSO _initList = null;

    private Transform _playerTrm;

    [SerializeField]
    private int _requirementsMonster;
    private int _monsterCnt;

    // ������Ƽ 
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
        PoolManager.Instance = new PoolManager(transform); //Ǯ�Ŵ��� ����
        _coroutineComponent = new CoroutineComponent(); 

        CreatePool();
    }

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
    }

    public void BegineCoroutine()
    {
        StartCoroutine(_coroutineComponent.coroutine); 
    }

    public void MonsterDie()
    {
        _monsterCnt++;
    }

    public bool CanNextFloor()
    {
        return _requirementsMonster <= _monsterCnt;
    }
}
