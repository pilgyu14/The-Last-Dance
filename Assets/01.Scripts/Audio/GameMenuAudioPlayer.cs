using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuAudioPlayer : AudioPlayer
{
    [SerializeField]
    private AudioClip _menuBGM = null;

    void Start()
    {
        PlayClip(_menuBGM);
    }

}
