using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Class : MonoBehaviour
{
    public List<Sound_subClass> sound_list;

    public class Sound_subClass : AudioDefault
    {
        public string Name;
        public AudioClip Clip;
        public float Volume;
        public float Time;

        public void Init(string name, AudioClip clip, float Volume = 0.5f, float time = 0.05f, float VolumeOffset = 1f)
        {
            this.Name = name;
            this.Clip = clip;
            this.Time = time;
            this.Volume = Volume * MasterVolume * EffectVolume * VolumeOffset;
        }

        public AudioSource Play()
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

            return mAudioSource;
        }

        public AudioSource PlayLoop()
        {
            AudioController controller = GetAudioController();

            AudioSource mAudioSource = controller.Audio;
            mAudioSource.Stop();
            mAudioSource.clip = this.Clip;
            mAudioSource.time = this.Time;
            mAudioSource.volume = this.Volume;
            mAudioSource.loop = true;
            mAudioSource.Play();

            controller.StartTiming = mAudioSource.time;
            controller.PlayTime = FULLTIME;

            return mAudioSource;
        }

        public string GetName()
        {
            return Name;
        }
    }

    public void Add(AudioClip[] sounds)
    {
        foreach (AudioClip sound in sounds)
        {
            Sound_subClass sound_SubClass = gameObject.AddComponent<Sound_subClass>();
            sound_SubClass.Init(name: sound.name, clip: sound);
        }
    }
}
