using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRush : PatternDefault
{

    public int JumpCount;
    public float JumpDelay;
    public float JumpSpeed;

    private bool jumpReady;
    private float timer;
    private int jumpcounter;
    private Rigidbody2D rb;
    public override void Setting()
    {
        jumpcounter = JumpCount;
        jumpReady = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
    }

    public void Reset()
    {
        JumpCount = 3;
    }

    public override void Run() { 
    
        base.Run();
        caster.GetComponent<EnemyDefault>().statement = "JumpShoot";
    }

    // Start is called before the first frame update
    void Start()
    {
        rb=gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpReady = true;
        timer = 0;
        rb.velocity = Vector3.zero;
        if (jumpcounter <= 0) { 
        
            Stop();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (is_run) {

            rb.velocity += new Vector2(0, GameController.GRAVITY * Time.fixedDeltaTime);

            if (jumpReady && jumpcounter > 0) {

                if (timer < JumpDelay)
                {

                    timer += Time.deltaTime;
                }
                else {

                    rb.velocity = Ballistics.Ballistic(GameController.GetPlayer().transform.position, JumpSpeed, GameController.GRAVITY);
                    jumpcounter--;
                    jumpReady = false;
                }

            }
            
        }
    }
}
