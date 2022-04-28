using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeDefault : MissileDefault
{
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.velocity += new Vector2(0, GameController.GRAVITY * Time.fixedDeltaTime);
    }
}
