using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysical : UnitDefault
{
    //public value
    public float Speed = 1;
    public float JumpPower = 6f;
    public Vector3 OffsetPosition;
    public float MinimumJumpPowerRatio;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;

    //statement OR effected
    private bool is_side_collision;
    private bool jumping;
    private Vector2 accel, m_Move;
    private Vector2 collision_point_avg;
    private Vector2 collision_point_variance;
    private float jumpTimer = 0f;
    private int jumpCounter;
    private Rigidbody2D rb2;
    private GameController gc;
    private PlayerHealth ph;

    //action
    public bool isJump;


    public override void Reset()
    {
        base.Reset();
        Speed = 5;
        JumpPower = 8;
    }


    // Start is called before the first frame update
    public override void Start()
    {
        is_side_collision = false;
        jumpTimer = 0;
        jumpCounter = 0;
        accel = Vector2.zero;
        m_Move = Vector2.zero;
        rb2.velocity = Vector2.zero;
        transform.position = OffsetPosition;
        jumping = false;
    }

    public void Awake()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        ph = gameObject.GetComponent<PlayerHealth>();
    }


    public void OnMove(InputValue value)
    {
        m_Move = value.Get<Vector2>();
        if (m_Move.x < 0) { 
        
            m_Move.x = -1;
        }
        if (m_Move.x > 0) { 
        
            m_Move.x = 1;
        }
    }
    public void OnJump(InputValue value) {

        isJump = value.Get<float>() > 0 ? true : false;
    }


    private void CollsionBlock(Collision2D collision) {

        collision_point_avg = Vector2.zero;
        collision_point_variance = Vector2.zero;
        foreach (ContactPoint2D point in collision.contacts)
        {

            collision_point_avg += point.point;
        }
        collision_point_avg /= collision.contacts.Length;
        foreach (ContactPoint2D point in collision.contacts)
        {
            collision_point_variance += new Vector2(Mathf.Pow(point.point.x - collision_point_avg.x, 2), Mathf.Pow(point.point.y - collision_point_avg.y, 2));
        }
        collision_point_variance /= collision.contacts.Length;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Block"))
        {
            CollsionBlock(collision);


            if (collision_point_variance.x < collision_point_variance.y&&
                collision_point_variance.magnitude>0.00000001f)//수평 충돌
            {
                transform.position = new Vector3(collision_point_avg.x < transform.position.x ? collision_point_avg.x + transform.localScale.x * 0.5f :
                    collision_point_avg.x - transform.localScale.x * 0.5f, transform.position.y);
                is_side_collision = true;

            }
            else //수직 충돌
            {
                if (collision_point_avg.y < transform.position.y)
                {
                    jumpCounter = JUMPMAX;
                    jumpTimer = 0;
                }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            CollsionBlock(collision);

            if (collision_point_variance.x < collision_point_variance.y)//수평 충돌
            {
                is_side_collision = true;

            }
            else //수직 충돌
            {
                if (collision_point_avg.y < transform.position.y) {

                    jumpCounter = JUMPMAX;
                    jumpTimer = 0;
                    is_side_collision = false;
                }
                   
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            is_side_collision = false;
            if (!isJump && jumpCounter == JUMPMAX) {

                jumpCounter--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        accel = new Vector2(0, GameController.GRAVITY);
        rb2.velocity = new Vector3(m_Move.x * Speed, rb2.velocity.y + accel.y * Time.fixedDeltaTime);
        //벽 판정시
        if (is_side_collision)
        {
            rb2.velocity = new Vector2(collision_point_avg.x < transform.position.x ?
                (m_Move.x>0?rb2.velocity.x:0):
                (m_Move.x<0?rb2.velocity.x:0)
                , rb2.velocity.y);
        }

        //jump
        if (isJump)
        {

            if (jumpCounter == JUMPMAX)
            {

                if (jumpTimer < JUMP_TIME)
                {
                    if (!jumping) {

                        if (gameObject.GetComponent<PlayerAudio>() != null) gameObject.GetComponent<PlayerAudio>().JumpPlay();
                        rb2.velocity = new Vector2(rb2.velocity.x, JumpPower * (MinimumJumpPowerRatio - (1f - MinimumJumpPowerRatio) * Time.fixedDeltaTime / JUMP_TIME));
                    }
                    else
                    {
                        rb2.velocity += new Vector2(0, JumpPower * (1f - MinimumJumpPowerRatio)) * Time.fixedDeltaTime / JUMP_TIME;
                    }
                    jumping = true;
                    jumpTimer += Time.fixedDeltaTime;
                }
                else
                {
                    jumping = false;
                    isJump = false;
                    jumpCounter--;
                }
            }
            else
            {
                if (jumpCounter > 0)
                {
                    if (gameObject.GetComponent<PlayerAudio>() != null) gameObject.GetComponent<PlayerAudio>().JumpPlay();
                    rb2.velocity = new Vector2(rb2.velocity.x, JumpPower);
                    jumpCounter--;
                    isJump = false;
                }
            }
        }
        else if (jumping) {
            jumping = false;
            isJump = false;
            jumpCounter--;
        }

        if (isFall) {

            //추락판정
            ph.Hurt(20);

            isFall = false;
        }

    }
}
