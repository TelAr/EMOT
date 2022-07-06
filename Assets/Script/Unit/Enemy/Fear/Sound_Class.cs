using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Class : AudioDefault
{
    private string Name;
    private AudioClip Clip;
    private float Volume;
    private float Time;
    
    
    public void Init(string name, AudioClip clip, float Volume = 0.5f, float time = 0.05f, float VolumeOffset = 1f)
    {
        this.Name = name;
        this.Clip = clip;
        this.Time = time;
        this.Volume = Volume * MasterVolume * EffectVolume * VolumeOffset;
    }
    
    public void play()
    {
        AudioController controller = GetAudioController();

        AudioSource mAudioSource = controller.Audio;
        mAudioSource.Stop();
        mAudioSource.clip = this.Clip;
        mAudioSource.time = this.Time;
        mAudioSource.volume = this.Volume;
        mAudioSource.Play();

        controller.StartTiming = mAudioSource.time;
        controller.PlayTime = FULLTIME;
    }

    public string GetName()
    {
        return Name;
    }
}
