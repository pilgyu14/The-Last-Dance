using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInfo
{
    public GameObject parent;
    public float delay; 
}

public class EffectComponent : PoolableMono
{
    [SerializeField]
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>(); 
    }
    public void StartEffect()
    {
        _particle.Play(); 
    }

    public void EndEffect()
    {
        _particle.Stop();
    }

    public void SetPosAndRot(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot; 
    }
    public override void Reset()
    {
    }
}
