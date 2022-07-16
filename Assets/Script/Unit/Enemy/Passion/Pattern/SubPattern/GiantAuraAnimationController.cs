using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAuraAnimationController : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SplashLoop"))
        {

            if (GetComponent<Damage>() != null)
            {
                GetComponent<Damage>().IsEffected = true;
            }
        }
        else {

            if (GetComponent<Damage>() != null)
            {
                GetComponent<Damage>().IsEffected = false;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End")){ 
        
            gameObject.SetActive(false);

        }

    }
}
