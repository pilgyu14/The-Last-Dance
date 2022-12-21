using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    protected AudioSource _audioSource;
    [SerializeField]
    protected float _pitchRandomness = 0.2f;
    protected float _basePitch;

    public AudioSource AudioSource => _audioSource; 

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _basePitch = _audioSource.pitch;
    }
    //클립을 랜덤피치로 재생하는 함수
    public void PlayClipWithVariablePitch(AudioClip clip)
    {
        float randomPitch = Random.Range(-_pitchRandomness, _pitchRandomness);
        _audioSource.pitch = _basePitch + randomPitch;
        PlayClip(clip);
    }
    //피치 조정없이 그냥 재생하는 함수
    public void PlayClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
