using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : GrenadeDefault
{
    public StickyBombs BombController = null;
    private bool is_sticky = false;
    private Rigidbody2D attached;
    private void Awake()
    {
        gameObject.GetComponent<FixedJoint2D>().enabled = false;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Boom();
        }

        gameObject.GetComponent<FixedJoint2D>().enabled = true;
        if (!is_sticky) {
            attached = collision.gameObject.GetComponent<Rigidbody2D>();
            gameObject.GetComponent<FixedJoint2D>().connectedBody = attached;
            is_sticky = true;
        }
        
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (gameObject.GetComponent<FixedJoint2D>().connectedBody == null) {

            gameObject.GetComponent<FixedJoint2D>().enabled = false;
        }
        if (BombController != null)
        {
            if (BombController.KaBoom <= 0) {

                Boom();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (is_sticky) {
            if (attached != null&&attached!=gameObject.GetComponent<Rigidbody2D>())
            {
                gameObject.GetComponent<FixedJoint2D>().connectedBody = attached;
            }
            else {

                is_sticky = false;
            }
        }
        
    }

    public void Boom() {

        impact.Impact();
        is_sticky = false;
        gameObject.GetComponent<FixedJoint2D>().connectedBody = null;
        gameObject.SetActive(false);
    }

}
