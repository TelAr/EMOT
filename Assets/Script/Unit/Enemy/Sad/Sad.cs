using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sad : EnemyDefault
{
    private Rigidbody2D rb2d;

    public override void Reset()
    {
        base.Reset();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }

    private void FixedUpdate()
    {
        if (!PatternRunning || DefaultPhysicalForcedEnable)
        {

            rb2d.velocity += new Vector2(0, GameController.GetGameController.GRAVITY * Time.fixedDeltaTime);
        }
    }
}
