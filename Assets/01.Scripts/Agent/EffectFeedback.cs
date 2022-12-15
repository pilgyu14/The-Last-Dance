using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectFeedback : Feedback
{
    [SerializeField]
    private ParticleSystem _particle;

    public override void FinishFeedback()
    {
        _particle.Stop(); 
    }

    public override void PlayFeedback()
    {
        _particle.Play();
    }
}
