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
    private GameObject _canvas; 
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
    /// 죽었으면 false 반환 
    /// </summary>
    /// <param name="dmg"></param>
    public bool ChangeHP(int dmg)
    {
        float tempHp = HP; 
        HP = dmg;
        //StopAllCoroutines(); 
        StartCoroutine(UpdateHpUI(tempHp));
        // UI가 있다면 UI업데이트 

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
            prevHp = Mathf.Lerp(prevHp, HP, Time.unscaledDeltaTime * 5);
            _hpSlider.value = (float)prevHp / _maxHp; 

            yield return null; 
        }
        _hpSlider.value =(float) HP / _maxHp;

        StartCoroutine(UpdatePrevHpUI(_hpSlider.value)); 
    }

    /// <summary>
    /// 배경 체력바 UI 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator UpdatePrevHpUI(float target)
    {
        while (Mathf.Abs(_prevHpSlider.value - target) > 0.01f)
        {
            _prevHpSlider.value = Mathf.Lerp(_prevHpSlider.value, target, Time.unscaledDeltaTime * 10); 

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
    
    public void ActiveHpUI()
    {
        _canvas.SetActive(true); 
    }
}

