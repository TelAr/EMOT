using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyAudio : AudioDefault
{

    public AudioClip SwingAudio;

    public void SwingPlay()
    {

        mAudioSource.Stop();
        mAudioSource.clip = SwingAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume;
        playTime = 0.8f;
        timer = 0;
        AudioState = State.Swing;
    }



}
