using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : AudioDefault
{
    public AudioClip JumpAudio;

    public void JumpPlay() {

        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;


        mAudioSource.clip = JumpAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f * MasterVolume * EffectVolume;

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = 0.8f;

        mAudioSource.Play();

    }



}
