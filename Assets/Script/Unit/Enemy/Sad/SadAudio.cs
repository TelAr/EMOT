using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadAudio : AudioDefault
{

    public AudioClip SwingAudio;
    public float SwingAudioVolumeOffset=1f;

    public AudioClip GrenadeFireAudio;
    public float GrenadeFireAudioVolumeOffset=1f;

    public void SwingPlay()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = SwingAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * SwingAudioVolumeOffset;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }


    public void GrenadeFirePlay(float input=1) {

        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = GrenadeFireAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * GrenadeFireAudioVolumeOffset * input;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }

}
