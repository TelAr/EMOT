using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : GrenadeDefault
{
    public StickyBombs BombController = null;
    public Sprite Normal;
    public Vector2 NormalSpriteSize;    
    public Sprite StickyForm;
    public Vector2 StickyFormSpriteSize;
    private float bombTimer;
    private bool is_sticky = false;
    private Rigidbody2D attached;
    private SpriteRenderer sr;

    private void Awake()
    {
        gameObject.GetComponent<FixedJoint2D>().enabled = false;
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        sr.sprite = Normal;
        sr.size= NormalSpriteSize;
    }

    public void SetTimer(float time) {

        bombTimer = time;
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
            sr.sprite = StickyForm;
            sr.size = StickyFormSpriteSize;
        }
        
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        bombTimer -= Time.deltaTime;

        if (bombTimer < 0) { 
        
            Boom();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (gameObject.GetComponent<FixedJoint2D>().connectedBody == null)
        {

            gameObject.GetComponent<FixedJoint2D>().enabled = false;
        }
        if (is_sticky) {
            if (attached != null&&attached!=gameObject.GetComponent<Rigidbody2D>())
            {
                gameObject.GetComponent<FixedJoint2D>().connectedBody = attached;
            }
            else {

                is_sticky = false;
            }
        }

        if (gameObject.GetComponent<FixedJoint2D>().connectedBody != null 
            && !gameObject.GetComponent<FixedJoint2D>().connectedBody.gameObject.CompareTag("Block")) {

            gameObject.GetComponent<FixedJoint2D>().connectedBody = null;
            is_sticky = false;
        }
        
    }

    public void Boom() {

        impact.Impact();
        is_sticky = false;
        gameObject.GetComponent<FixedJoint2D>().connectedBody = null;
        gameObject.SetActive(false);
    }

}
