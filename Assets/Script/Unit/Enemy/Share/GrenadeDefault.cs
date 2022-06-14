using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeDefault : MissileDefault
{
    public float RotatePerSecond = 0;
    protected Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        rb2D.velocity += new Vector2(0, GameController.GetGameController().GRAVITY * Time.fixedDeltaTime);
        transform.Rotate(new Vector3(0, 0, RotatePerSecond * Time.fixedDeltaTime));
    }
}
