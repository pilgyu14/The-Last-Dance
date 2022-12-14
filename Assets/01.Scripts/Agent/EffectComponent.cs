using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private List<ParticleSystem> particleList = new List<ParticleSystem>(); 
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private float _lifeTime = 1f; 
    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        particleList ??= _particle.GetComponentsInChildren<ParticleSystem>().ToList();
        parent = transform.parent; 
    }
    public void StartEffect()
    {
        transform.SetParent(null);
       // CheckLifeTime(); 
        _particle.Play();

        StartCoroutine(EndParticle()); 
    }

    public void EndEffect()
    {
        transform.SetParent(parent); 
        //_particle.Stop();
    }

    public void SetPosAndRot(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot; 
    }

    private void CheckLifeTime()
    {
        for (int i = 0; i < particleList.Count; i++)
        {
            float lifeTime = particleList[i].main.startLifetimeMultiplier;
            if (_lifeTime < lifeTime)
            {
                _lifeTime = lifeTime;
            }
        }
    }

    IEnumerator EndParticle()
    {
        yield return new WaitForSeconds(_lifeTime);
        EndEffect(); 
    }
    public override void Reset()
    {
    }
}
