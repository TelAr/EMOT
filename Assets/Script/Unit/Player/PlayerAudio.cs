using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip JumpAudio;
    public enum State { 
    Jump, Null
    };
    public State AudioState;

    private AudioSource mAudioSource;
    private float playTime;

    private float timer;

    public void JumpPlay() {

        mAudioSource.Stop();
        mAudioSource.clip = JumpAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f;
        playTime = 0.8f;
        timer = 0;
        AudioState=State.Jump;
    }

    // Start is called before the first frame update
    void Start()
    {
        mAudioSource=gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < playTime)
        {
            timer+=Time.deltaTime;
            if (!mAudioSource.isPlaying) {

                mAudioSource.Play();
            }
        }
        else {

            mAudioSource.Stop();
            AudioState = State.Null;
        }
    }
}
