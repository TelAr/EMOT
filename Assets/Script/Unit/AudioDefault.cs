using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefault : MonoBehaviour
{
    public enum State { Jump, Null };
    public State AudioState;

    public static float MasterVolume = 1, EffectVolume = 1, BGMVolume = 1;
    protected AudioSource mAudioSource;
    protected float playTime;

    protected float timer;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<AudioSource>() == null) {

            gameObject.AddComponent<AudioSource>();
        }
        mAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (timer < playTime)
        {
            timer += Time.deltaTime;
            if (!mAudioSource.isPlaying)
            {

                mAudioSource.Play();
            }
        }
        else
        {

            mAudioSource.Stop();
            AudioState = State.Null;
        }
        if (GameController.is_stop)
        {

            mAudioSource.Pause();
        }
        else {

            mAudioSource.UnPause();
        }
    }
}
