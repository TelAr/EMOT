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
    public AudioClip NoAmmo;
    public float NoAmmoVolumeOffset;
    public AudioClip ParryingSuccess;
    public float ParryingSuccessVolumeOffset;

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

    public void FirePlay(float InputOffset=1f)
    {

        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = FireAudio;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * FireVolumeOffset * InputOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

    public void NoAmmoPlay() {


        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = NoAmmo;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * NoAmmoVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

    public void ParryingSuccessPlay()
    {


        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = ParryingSuccess;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * ParryingSuccessVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

}
