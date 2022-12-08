using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HPModule :  MonoBehaviour ,IComponent
{
    [SerializeField]
    private int _curHp;
    private int _maxHp;

    // UI길이 체력에 따라 길거나 작게 설정 
    [SerializeField]
    private Slider 

    public int HP
    {
        get => _curHp; 
        set
        {
            _curHp = Mathf.Clamp(_curHp - value, 0, _maxHp); 
        }
    }

    public void Init(int curHp,int maxHp)
    {
        this._curHp = curHp;
        this._maxHp = maxHp; 
    }

    public void Damaged(int dmg)
    {
        StartCoroutine(DamagedCoroutine(dmg)); 
        // UI가 있다면 UI업데이트 
    }

    IEnumerator DamagedCoroutine(int dmg)
    {
        // 
        int target = 
        while(true)
        {

            yield return null; 
        }
    }

    /// <summary>
    /// 체력바 길이 설정 
    /// </summary>
    private void SetUILength()
    {

    }
    
}

