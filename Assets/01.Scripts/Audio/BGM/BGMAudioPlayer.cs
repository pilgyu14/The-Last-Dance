using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudioPlayer : AudioPlayer
{
    [SerializeField]
    private AudioClip BGM = null;

    void Start()
    {
        PlayClip(BGM);
    }

}
