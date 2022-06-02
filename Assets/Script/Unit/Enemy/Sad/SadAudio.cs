using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadAudio : AudioDefault
{

    public AudioClip SwingAudio;

    public void SwingPlay()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = SwingAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }



}
