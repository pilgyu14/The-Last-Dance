using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class CamShakeFeedback : Feedback
{
    [SerializeField]
    private CinemachineFreeLook _cmVCam;
    [SerializeField, Range(0,5f)]
    private float _amplitude = 1, _intensity = 1;
    [SerializeField,Range(0,1)]
    private float _shakeTime = 1.0f;

    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        if (_cmVCam == null)
            _cmVCam = GameObject.FindObjectOfType<CinemachineFreeLook>(); 

        _noise ??= _cmVCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    IEnumerator ShakeCoroutine()
    {

        float time = _shakeTime;
        while (time > 0)
        {
            _noise.m_AmplitudeGain = Mathf.Lerp(0, _amplitude, time / _shakeTime);
            yield return null;
            time -= Time.deltaTime;
        }
        _noise.m_AmplitudeGain = 0;
    }

    public override void PlayFeedback()
    {
        _noise.m_AmplitudeGain = _amplitude;
        _noise.m_FrequencyGain = _intensity;
        StartCoroutine(ShakeCoroutine());
        
    }

    public override void FinishFeedback()
    {
        StopAllCoroutines();
        _noise.m_AmplitudeGain = 0;
    }
}
