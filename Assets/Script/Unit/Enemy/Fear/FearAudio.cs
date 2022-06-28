using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearAudio : AudioDefault
{

    public AudioClip Beam_groundAudio;
    public float Beam_groundAudioVolumeOffset = 1f;

    public AudioClip EyebeamAudio;
    public float EyebeamAudioVolumeOffset = 1f;

    public AudioClip ExplosionAudio;
    public float ExplosionAudioVolumeOffset = 1f;

    public AudioClip Void_openAudio;
    public float Void_openAudioVolumeOffset = 1f;

    public void Beam_groundPlay()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = Beam_groundAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * Beam_groundAudioVolumeOffset;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }

    public void EyebeamPlay()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = EyebeamAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * EyebeamAudioVolumeOffset;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }

    public void ExplosionPlay()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = ExplosionAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * ExplosionAudioVolumeOffset;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }

    public void Void_openPlay()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = Void_openAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume * Void_openAudioVolumeOffset;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;
    }
}
