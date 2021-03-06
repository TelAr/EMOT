using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : AudioDefault
{
    [Header("* Jump")]
    public AudioClip JumpAudio;
    public float JumpVolumeOffset;
    [Header("* Reload")]
    public AudioClip ReloadAudio;
    public float ReloadVolumeOffset;
    [Header("* Fire")]
    public AudioClip FireAudio;
    public float FireVolumeOffset;
    [Header("* NoAmmo")]
    public AudioClip NoAmmo;
    public float NoAmmoVolumeOffset;
    [Header("* ParryingSuccess")]
    public AudioClip ParryingSuccess;
    public float ParryingSuccessVolumeOffset;
    [Header("* Sliding")]
    public AudioClip Sliding;
    public float SlidingVolumeOffset;
    [Header("* Dash")]
    public AudioClip Dash;
    public float DashVolumeOffset;
    [Header("* Error")]
    public AudioClip Error;
    public float ErrorVolumeOffset;
    [Header("* Hurt")]
    public AudioClip Hurt;
    public float HurtVolumeOffset;
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

    public void SlidingPlay()
    {


        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = Sliding;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * SlidingVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

    public void DashPlay()
    {


        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = Dash;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * DashVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }

    public void HurtPlay() {


        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = Hurt;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * DashVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();
    }

    public void ErrorPlay()
    {


        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = Error;
        mAudioSource.time = 0f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * ErrorVolumeOffset;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 2f;

        mAudioSource.Play();

    }
}
