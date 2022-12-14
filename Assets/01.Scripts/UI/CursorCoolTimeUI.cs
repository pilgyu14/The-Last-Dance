using UnityEngine;
using TMPro;
using DG.Tweening; 
public class CursorCoolTimeUI : PoolableMono
{
    [SerializeField]
    private TextMeshProUGUI _coolTimeText;
    private RectTransform _rectTrm; 

    private void Awake()
    {
        _coolTimeText = GetComponent<TextMeshProUGUI>();
        _rectTrm = GetComponent<RectTransform>();
    }

    public void UpdateCoolTimeText(float coolTime)
    {
        _coolTimeText.text = string.Format("{0}S", coolTime);

        _rectTrm.anchoredPosition = Input.mousePosition;


        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + 0.7f, 1f));
        seq.Join(_coolTimeText.DOFade(0, 1f));
        seq.Join(transform.DOScale(0.5f, 1f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public override void Reset()
    {
        _coolTimeText.color = Color.white;
        transform.localScale = Vector3.one; 
    }
}
