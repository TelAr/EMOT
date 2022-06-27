using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
//    float timer=0;
//    Color color;


    public float RemainTime;
    public float FadeOutTime;
    public bool test = false;
    public float JudgeStartRadius, JudgeEndRadius;
    public float Judgetime;
    public AudioClip DefaultSound;
    public float ExplosionVolumeOffset = 1;
    private Animator animator = null;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource=null;
    private bool animation_end, isPlay;
    // Start is called before the first frame update
    void Awake()
    {

        animator = gameObject.GetComponent<Animator>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        audioSource=gameObject.GetComponent<AudioSource>();
        Initiation();
        isPlay = true;
    }

    private void OnEnable()
    {
        gameObject.tag = "Enemy";
        gameObject.GetComponent<Damage>().IsEffected = true;
        animation_end = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        if (animator == null)
        {

            animator = gameObject.GetComponent<Animator>();
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
    }

    public void Initiation(float SoundVolume = 1f, AudioClip clip=null) {

        if (audioSource == null)
        {

            audioSource = gameObject.GetComponent<AudioSource>();
        }
        if (clip == null)
        {

            audioSource.clip = DefaultSound;
        }
        else { 
        
            audioSource.clip = clip;
        }
        isPlay = true;
        audioSource.volume = ExplosionVolumeOffset * AudioDefault.MasterVolume * AudioDefault.EffectVolume * SoundVolume;
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
        {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < Judgetime)
            {
                circleCollider.radius = JudgeStartRadius * (Judgetime - animator.GetCurrentAnimatorStateInfo(0).normalizedTime) / Judgetime +
                    JudgeEndRadius * animator.GetCurrentAnimatorStateInfo(0).normalizedTime / Judgetime;

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(isPlay)
        {
            audioSource.Play();
            isPlay = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion")) {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= Judgetime)
            {

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    gameObject.GetComponent<Damage>().IsEffected = false;
                    gameObject.tag = "Untagged";
                }
                else
                {

                    gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
                    animation_end = true;
                }
            }
            
        }

        if (animation_end && !audioSource.isPlaying) { 
        
            gameObject.SetActive(false);
        }



        //LAGACY
        /*
        timer+=Time.deltaTime;
        if (timer > RemainTime) {

            gameObject.tag = "Untagged";
            gameObject.GetComponent<SpriteRenderer>().color=new Color(color.r,color.g,color.b,(FadeOutTime+RemainTime-timer)/FadeOutTime);
            if (FadeOutTime + RemainTime < timer) { 
            
                Destroy(gameObject);
            }
        }
        */
    }
}
