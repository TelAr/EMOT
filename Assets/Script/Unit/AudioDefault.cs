using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioDefault : MonoBehaviour
{

    public static float MasterVolume = 1, EffectVolume = 1, BGMVolume = 1;

    protected class AudioController { 
    
        public AudioSource Audio;
        public float StartTiming, PlayTime;
    }
    protected List<AudioController> audioControllers = new List<AudioController>();

    protected AudioController GetAudioController() {

        AudioController returnValue = null;
        foreach (AudioController source in audioControllers) {

            if (!source.Audio.isPlaying)
            {

                returnValue = source;
                break;
            }
        }
        if (returnValue == null) {

            returnValue = new AudioController
            {
                Audio = gameObject.AddComponent<AudioSource>()
            };
            audioControllers.Add(returnValue);
        }
        returnValue.Audio.playOnAwake = false;

        return returnValue;
    }

    protected bool IsAlready(AudioClip comp) { 
    
        bool result = false;

        foreach (AudioController source in audioControllers)
        {

            if (!source.Audio == comp)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    protected void Update() {

        foreach (AudioController source in audioControllers) {

            if (Time.timeScale <= 0)
            {
                source.Audio.Pause();
            }
            else { 
            
                source.Audio.UnPause();
            }

            if (source.Audio.time >= source.StartTiming + source.PlayTime)
            {
                source.Audio.Stop();
            }
        }
    }
}
