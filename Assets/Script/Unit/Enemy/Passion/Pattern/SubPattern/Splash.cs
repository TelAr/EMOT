using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{

    public List<BoxCollider2D> colliders = new List<BoxCollider2D>();

    [SerializeField]
    private float ratio;
    private Animator m_Animator;

    private void Awake()
    {
        foreach (var collider in gameObject.GetComponents<BoxCollider2D>()) {

            colliders.Add(collider);
        }

        m_Animator=gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        foreach (var collider in colliders) {

            collider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ratio = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (ratio > (2.0f / 7.0f))
        {

            colliders[0].enabled = false;
        }
        else { 
        
            colliders[0].enabled = true;
        }

        if (ratio > (4.0f / 7.0f)) {

            colliders[1].enabled = false;
        }
        else
        {
            colliders[1].enabled = true;

        }

        if (ratio > 1) { 
        
            gameObject.SetActive(false);
        }
    }
}
