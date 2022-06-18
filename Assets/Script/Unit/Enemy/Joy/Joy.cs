using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joy : EnemyDefault
{

    private Rigidbody2D rb2d;

    public override void Reset()
    {
        base.Reset();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb2d= gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (isFall) { 
        
        //특수 패턴
        
            isFall = false;
        }
    }

    private void FixedUpdate()
    {
        if (!PatternRunning) {

            rb2d.velocity += new Vector2(0, GameController.GetGameController().GRAVITY*Time.fixedDeltaTime);
        }
    }
}
