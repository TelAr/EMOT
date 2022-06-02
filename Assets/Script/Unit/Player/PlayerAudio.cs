using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : AudioDefault
{
    public AudioClip JumpAudio;
    public float JumpVolumeOffset;
    public AudioClip ReloadAudio;
    public float ReloadVolumeOffset;
    public AudioClip FireAudio;
    public float FireVolumeOffset;

    public void JumpPlay() {

        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = JumpAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * JumpVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;

        mAudioSource.Play();

    }

    public void ReloadPlay()
    {

        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = ReloadAudio;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * ReloadVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

    public void FirePlay()
    {

        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = FireAudio;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * FireVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

}
