using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenEffect : Feedback
{
    [SerializeField]
    private Material _mat;
    [SerializeField]
    private float _maxIntensity = 0.028f;

    public bool One = true;


    public override void PlayFeedback()
    {
        StopAllCoroutines();
       StartCoroutine(StartEffect(_maxIntensity)); 
    }
    public override void FinishFeedback()
    {
        StopAllCoroutines(); 
        StartCoroutine(StartEffect(0f));
    }


    IEnumerator StartEffect(float target)
    {
        float intensity = _mat.GetFloat("_FullScreenIntensity");
        
        while (Mathf.Abs(intensity - target) > 0.001f)
        {
            intensity = Mathf.Lerp(intensity, target, Time.deltaTime);
            _mat.SetFloat("_FullScreenIntensity", intensity);
            yield return null; 
        }
    }
}
