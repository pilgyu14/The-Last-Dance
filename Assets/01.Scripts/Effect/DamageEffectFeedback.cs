using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
public class DamageEffectFeedback : Feedback
{
    [SerializeField]
    private Image _effectImage;

    private Sequence seq; 
    private void Awake()
    {
        _effectImage = GetComponent<Image>(); 
    }

    private void Start()
    {
    }

    public override void PlayFeedback()
    {
        seq = DOTween.Sequence();
        seq.Append(_effectImage.DOFade(0.1f,0.2f));
        seq.Append(_effectImage.DOFade(0f, 0.2f));
    }
    public override void FinishFeedback()
    {
        //DOTween.KillAll(); 
    }


}
