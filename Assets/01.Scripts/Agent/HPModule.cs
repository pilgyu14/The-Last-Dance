using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HPModule :  MonoBehaviour ,IComponent
{
    [SerializeField]
    private int _curHp;
    private int _prevHp; 
    private int _maxHp;

    [SerializeField]
    private bool _isPlayer; 

    // UI길이 체력에 따라 길거나 작게 설정 
    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private List<Slider> _hpSliderList;
    [SerializeField]
    private List<Slider> _prevHpSliderList; 

    public int HP
    {
        get => _curHp; 
        set
        {
            _curHp = Mathf.Clamp(_curHp + value, 0, _maxHp); 
        }
    }
    public int PrevHp => _prevHp; 
    public int MaxHp => _maxHp; 

    public void Init(int curHp,int maxHp)
    {
        this._curHp = curHp;
        this._maxHp = maxHp; 
    }

    /// <summary>
    /// HP변경 함수 (데미지는 - 로 회복은 +로)  
    /// 죽었으면 false 반환 
    /// </summary>
    /// <param name="dmg"></param>
    public bool ChangeHP(int dmg)
    {
         _prevHp= HP; 
        HP = dmg;
        //StopAllCoroutines(); 
        StartCoroutine(UpdateHpUI(_prevHp));
        // UI가 있다면 UI업데이트 
        if(_isPlayer == true)
        {
            EventManager.Instance.TriggerEvent(EventsType.UpdateHpUI); // 메인 UI 업데이트 
        }

        if (HP == 0)
        {
            _canvas.SetActive(false);
            return false; // 죽었으면 false 반환 
        }
        return true; 
    }

    /// <summary>
    /// 체력바 UI 
    /// </summary>
    /// <param name="prevHp"></param>
    /// <returns></returns>
    IEnumerator UpdateHpUI(float prevHp)
    {
        while(Mathf.Abs(HP - prevHp) > 0.5f)
        {
            for(int i =0;i < _hpSliderList.Count; i++)
            {
                prevHp = Mathf.Lerp(prevHp, HP, Time.unscaledDeltaTime * 5);
                _hpSliderList[i].value = (float)prevHp / _maxHp;
            }
            yield return null; 
        }

        for (int i = 0; i < _hpSliderList.Count; i++)
        {
            _hpSliderList[i].value = (float)HP / _maxHp;
            if (_prevHpSliderList[i] != null)
            {
                StartCoroutine(UpdatePrevHpUI(_hpSliderList[i].value,i));
            }
        }
    }

    /// <summary>
    /// 배경 체력바 UI 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator UpdatePrevHpUI(float target,int idx)
    {
        while (Mathf.Abs(_prevHpSliderList[idx].value - target) > 0.01f)
        {
            _prevHpSliderList[idx].value = Mathf.Lerp(_prevHpSliderList[idx].value, target, Time.unscaledDeltaTime * 10); 

            yield return null;
        }
        _prevHpSliderList[idx].value = _hpSliderList[idx].value; 
    }

    /// <summary>
    /// 체력바 길이 설정 
    /// </summary>
    private void SetUILength()
    {

    }
    
    public void ActiveHpUI()
    {
        _canvas.SetActive(true); 
    }

 
}

