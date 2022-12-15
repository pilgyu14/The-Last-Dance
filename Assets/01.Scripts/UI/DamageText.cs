using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageText : PoolableMono
{
    private Transform _parentCanvas;
    private RectTransform _rectTrm;
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _parentCanvas ??= GameObject.Find("TextCanvas").transform;
        _rectTrm = GetComponent<RectTransform>(); 
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(int damageAmount, Vector3 pos, bool isCritical, Color color)
    {
        transform.SetParent(_parentCanvas);

        _rectTrm.anchoredPosition = Define.MainCam.WorldToScreenPoint(pos); 
        _textMesh.SetText(damageAmount.ToString());

        if (isCritical)
        {
            _textMesh.color = Color.red;
            _textMesh.fontSize = 50f;
        }
        else
        {
            _textMesh.color = color;

        }
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + 0.7f, 1f));
        seq.Join(_textMesh.DOFade(0, 1f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public override void Reset()
    {
        _textMesh.color = Color.white;
        _textMesh.fontSize = 40f;
    }

}
