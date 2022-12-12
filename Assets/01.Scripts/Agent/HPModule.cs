using System.Collections;
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
    private Slider _hpSlider;
    [SerializeField]
    private Slider _prevHpSlider; 

    public int HP
    {
        get => _curHp; 
        set
        {
            _curHp = Mathf.Clamp(_curHp + value, 0, _maxHp); 
        }
    }

    public void Init(int curHp,int maxHp)
    {
        this._curHp = curHp;
        this._maxHp = maxHp; 
    }

    /// <summary>
    /// HP변경 함수 (데미지는 - 로 회복은 +로) 
    /// </summary>
    /// <param name="dmg"></param>
    public void ChangeHP(int dmg)
    {
        int tempHp = HP; 
        HP = dmg; 
        StartCoroutine(UpdateHpUI(tempHp)); 
        // UI가 있다면 UI업데이트 
    }

    /// <summary>
    /// 체력바 UI 
    /// </summary>
    /// <param name="prevHp"></param>
    /// <returns></returns>
    IEnumerator UpdateHpUI(int prevHp)
    {
        while(Mathf.Abs(HP - prevHp) > 0.01f)
        {
            prevHp = (int)Mathf.Lerp(prevHp, HP, Time.deltaTime * 5);
            _hpSlider.value = (float) prevHp / _maxHp; 

            yield return null; 
        }
        StartCoroutine(UpdatePrevHpUI(_hpSlider.value)); 
    }

    /// <summary>
    /// 배경 체력바 UI 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator UpdatePrevHpUI(float target)
    {
        while (_prevHpSlider.value - target > 0.01f)
        {
            _prevHpSlider.value = Mathf.Lerp(_prevHpSlider.value, target, Time.deltaTime * 10); 

            yield return null;
        }
        _prevHpSlider.value = _hpSlider.value; 
    }

    /// <summary>
    /// 체력바 길이 설정 
    /// </summary>
    private void SetUILength()
    {

    }
    
}

