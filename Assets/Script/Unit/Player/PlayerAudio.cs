using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : AudioDefault
{
    public AudioClip JumpAudio;

    public void JumpPlay() {

        mAudioSource.Stop();
        mAudioSource.clip = JumpAudio;
        mAudioSource.time = 0.05f;
        mAudioSource.volume = 0.5f;
        playTime = 0.8f;
        timer = 0;
        AudioState=State.Jump;
    }



    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }
}
