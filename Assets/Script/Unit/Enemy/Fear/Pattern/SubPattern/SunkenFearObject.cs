using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkenFearObject : SpawnedObject
{
    public GameObject Hole, Tentacle, Loop;
    public float Predelay, JudgeTime, postDelay;

    [SerializeField]
    private float animationTime;
    private Animator animator;
    private int step = 0;
    private float timer;

    private void Awake()
    {
        animator = Hole.GetComponent<Animator>();
    }

    void OnEnable()
    {
        timer = 0;
        step = 0;
        Tentacle.SetActive(false);
        Loop.SetActive(false);
        animator.SetBool("IsLoop", true);
        Loop.GetComponent<Animator>().SetBool("IsLoop", true);
    }

    void Update()
    {
        animationTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        switch (step) {

            case 0:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Loop"))
                {
                    step = 1;
                }
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > Predelay) {

                    step = 2;
                    timer = 0;
                    Tentacle.SetActive(true);
                    Loop.SetActive(true);
                    if (Tentacle.GetComponent<Damage>())
                    {

                        Tentacle.GetComponent<Damage>().IsEffected = true;
                    }
                }
                break;
            case 2:
                timer += Time.deltaTime;
                if (timer > JudgeTime)
                {

                    step = 3;
                    timer = 0;
                    if (Tentacle.GetComponent<Damage>()) {

                        Tentacle.GetComponent<Damage>().IsEffected = false;
                    }
                }
                break;
            case 3:
                timer += Time.deltaTime;
                if (timer > postDelay) {

                    Tentacle.SetActive(false);
                    Loop.GetComponent<Animator>().SetBool("IsLoop", false);
                    timer = 0;
                    step = 4;
                    animator.SetBool("IsLoop", false);
                }
                break;
            case 4:

                if (Loop.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("End")) {

                    Loop.SetActive(false);
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("SunkenCloseAnimation") 
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f) {

                    Caster.Stop();
                    gameObject.SetActive(false);
                }
                break;
        }

    }
}
