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
    private Animator animator;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource;
    private bool animation_end;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Enemy";
//        color = gameObject.GetComponent<SpriteRenderer>().color;
        animator = gameObject.GetComponent<Animator>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
        audioSource=gameObject.GetComponent<AudioSource>();
        audioSource.volume *= AudioDefault.MasterVolume * AudioDefault.EffectVolume;
        animation_end = false;
    }

    // Update is called once per frame
    void Update()
    {
       

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion")) {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < Judgetime)
            {
                circleCollider.radius = JudgeStartRadius * (Judgetime - animator.GetCurrentAnimatorStateInfo(0).normalizedTime) / Judgetime + 
                    JudgeEndRadius * animator.GetCurrentAnimatorStateInfo(0).normalizedTime / Judgetime;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                gameObject.tag = "Untagged";
            }
            else {

                gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
                animation_end = true;
            }
            
        }

        if (animation_end && !audioSource.isPlaying) { 
        
            Destroy(gameObject);
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
