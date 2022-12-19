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
    private TextMeshProUGUI _maxExpText;
    [SerializeField]
    private TextMeshProUGUI _curExpText;
    [SerializeField]
    private Image _expBar; 


    /// <summary>
    /// 체력 UI 텍스트 업데이트 
    /// </summary>
    public void UpdateTextUI()
    {
        _maxExpText.text = _playerSO.maxExp.ToString();
        _curExpText.text = _playerSO.exp.ToString();
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
        _expBar.fillAmount = (float)_playerSO.exp / _playerSO.maxExp;
    }
}


public class MainUIComponent : MonoBehaviour
{
    [SerializeField]
    private Slider _hpSlider;
    [SerializeField]
    private TextMeshProUGUI _maxHpText;
    [SerializeField]
    private TextMeshProUGUI _curHpText;

    [SerializeField]
    private HPModule _playerHpModule;

    [SerializeField, Header("경험치")]
    private ExpUIComponent _expUIComponent; 

    private void Start()
    {
        EventManager.Instance.StartListening(EventsType.UpdateHpUI, UpdateHpUI);
        EventManager.Instance.StartListening(EventsType.UpdateHpUI, (x) => UpdateExpUI((float)x));
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
        _maxHpText.text = _playerHpModule.MaxHp.ToString();
        _curHpText.text = _playerHpModule.HP.ToString();

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