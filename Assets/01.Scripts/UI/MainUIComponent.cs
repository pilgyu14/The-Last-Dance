using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ExpUIComponent
{
    [SerializeField]
    private PlayerSO _playerSO; 
    [SerializeField]
    private TextMeshProUGUI _expText;
    [SerializeField]
    private Image _expBar; 


    /// <summary>
    /// 체력 UI 텍스트 업데이트 
    /// </summary>
    public void UpdateTextUI()
    {
        int expPercent = (_playerSO.exp / _playerSO.maxExp) * 100; 
        _expText.text = string.Format("{0} / {1} ( {2:N1}% )", _playerSO.exp.ToString(), _playerSO.maxExp.ToString() , expPercent);
    }

    /// <summary>
    /// 체력바 UI 
    /// </summary>
    /// <param name="prevHp"></param>
    /// <returns></returns>
    public IEnumerator UpdateExpUI(float prevHp)
    {
        while (Mathf.Abs(_playerSO.exp - prevHp) > 0.5f)
        {
            prevHp = Mathf.Lerp(prevHp, _playerSO.exp, Time.unscaledDeltaTime * 5);
            _expBar.fillAmount = (float)prevHp / _playerSO.maxExp;
            yield return null;
        }
        SetExpBar(); 
    }

    public void SetExpBar()
    {
        _expBar.fillAmount = (float)_playerSO.exp / _playerSO.maxExp;
    }
}


public class MainUIComponent : MonoBehaviour
{
    [Header("체력")]
    [SerializeField]
    private Slider _hpSlider;
    [SerializeField]
    private TextMeshProUGUI _hpText;

    [SerializeField]
    private HPModule _playerHpModule;

    [SerializeField, Header("경험치")]
    private ExpUIComponent _expUIComponent; 

    private void Start()
    {
        EventManager.Instance.StartListening(EventsType.UpdateHpUI, UpdateHpUI);
        EventManager.Instance.StartListening(EventsType.UpdateHpUI, (x) => UpdateExpUI( (float)x ) );

        StartCoroutine(Init()); 
    }

    IEnumerator Init()
    {
        while (_playerHpModule.MaxHp == 0)
        {
            yield return null; 
        }
        UpdateHpUI();
        _expUIComponent.UpdateTextUI();
        _expUIComponent.SetExpBar(); 
    }

    public void UpdateExpUI(float prevExp)
    {
        StartCoroutine(_expUIComponent.UpdateExpUI(prevExp)); 
        _expUIComponent.UpdateTextUI(); 
    }

    /// <summary>
    /// 체력 UI 텍스트 업데이트 
    /// </summary>
    public void UpdateHpUI()
    {
        _hpText.text = string.Format("{0} / {1}", _playerHpModule.HP.ToString(), _playerHpModule.MaxHp.ToString());
        StartCoroutine(UpdateHpUI(_playerHpModule.PrevHp)); 
    }

    /// <summary>
    /// 체력바 UI 
    /// </summary>
    /// <param name="prevHp"></param>
    /// <returns></returns>
    IEnumerator UpdateHpUI(float prevHp)
    {
        while (Mathf.Abs(_playerHpModule.HP - prevHp) > 0.5f)
        {
            prevHp = Mathf.Lerp(prevHp, _playerHpModule.HP, Time.unscaledDeltaTime * 5);
            _hpSlider.value = (float)prevHp / _playerHpModule.MaxHp;

            yield return null;
        }
        _hpSlider.value = (float)_playerHpModule.HP / _playerHpModule.MaxHp;
    }
}