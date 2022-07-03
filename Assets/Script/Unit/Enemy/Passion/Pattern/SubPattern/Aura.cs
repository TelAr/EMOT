using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    private Vector3 moveDirection;
    private float beginVel = 1f, afterVel = 2f;
    private float accelDelay = 1f;
    private float timer = 0;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float BeginV, float AfterV, float AccelDelay, Vector3 Direction) {

        moveDirection = Direction;
        moveDirection.Normalize();
        beginVel = BeginV;
        afterVel = AfterV;
        accelDelay = AccelDelay;
        timer = 0;
    }

    public void FixedUpdate() {

        timer += Time.fixedDeltaTime;
        if (timer < accelDelay)
        {
            rb.velocity = moveDirection * beginVel;
        }
        else {

            rb.velocity = moveDirection * afterVel;
        }
    }


}
