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
    private List<ParticleSystem> _particleList = new List<ParticleSystem>(); 
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private float _lifeTime = 1f;

    [SerializeField]
    private Vector3 _originPos; 
    [SerializeField]
    private Quaternion _originRot;

    public ParticleSystem Particle => _particle;
    public List<ParticleSystem> ParticleList => _particleList;
    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _particleList ??= _particle.GetComponentsInChildren<ParticleSystem>().ToList();
        parent = transform.parent;

        _originPos = transform.localPosition;
        _originRot = transform.localRotation; 
    }
    public void StartEffect()
    {
        if(transform.parent == null)
        {

        }
        transform.SetParent(null);
       // CheckLifeTime(); 
        _particle.Play();

        StartCoroutine(EndParticle()); 
    }

    [ContextMenu("A")]
    public void Test()
    {
        Debug.Log("@@" + transform.localPosition);
        Debug.Log("@@" + transform.localRotation);

        Debug.Log("@@" + _originPos);
        Debug.Log("@@" + _originRot);

    }
    public void EndEffect()
    {
        Debug.Log("¿Ã∆Â∆Æ ≥°");
        transform.SetParent(parent);
        transform.localPosition = _originPos;
        transform.localRotation = _originRot;
        //_particle.Stop();
    }

    public void SetPosAndRot(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.eulerAngles = rot; 
    }

    private void CheckLifeTime()
    {
        for (int i = 0; i < _particleList.Count; i++)
        {
            float lifeTime = _particleList[i].main.startLifetimeMultiplier;
            if (_lifeTime < lifeTime)
            {
                _lifeTime = lifeTime;
            }
        }
    }

    IEnumerator EndParticle()
    {
        yield return new WaitForSeconds(_lifeTime + 0.1f);
        EndEffect(); 
    }
    public override void Reset()
    {
    }
}
