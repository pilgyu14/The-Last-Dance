using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudioPlayer : AudioPlayer
{
    [SerializeField]
    private AudioClip buttonClickSound = null;

    public void OnButtonClick()
    {
        PlayClip(buttonClickSound);
    }
}
