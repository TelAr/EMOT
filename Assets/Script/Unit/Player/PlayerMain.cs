using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : UnitDefault
{
    //public value
    public int health = 1;
    public float Speed = 1;
    public float JumpPower = 6f;
    public int Life = 3;
    public Vector3 offset_position;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;

    //statement OR effected
    private bool is_side_collision;
    private bool jumping;
    private Vector2 accel, m_Move;
    private Vector2 collision_point_avg;
    private Vector2 collision_point_variance;
    private float jump_timer = 0f;
    private int jumpCounter;
    private Rigidbody2D rb2;
    private GameController gc;
    private PlayerAudio playerAudio;
    private float immunTimer = 0;
    private SpriteRenderer spriteRenderer;

    //action
    public bool is_jump;


    public override void Reset()
    {
        base.Reset();
        health = 1;
        Speed = 5;
        JumpPower = 8;
        Life = 3;
    }

    public void LifeSettingStart(int life) {

        Start();
        Life = life;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        is_side_collision = false;
        jump_timer = 0;
        jumpCounter = 0;
        accel = Vector2.zero;
        m_Move = Vector2.zero;
        rb2.velocity = Vector2.zero;
        transform.position = offset_position;
        Life = 3;
        jumping = false;
    }

    public void Awake()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        playerAudio = gameObject.GetComponent<PlayerAudio>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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

        is_jump = value.Get<float>() > 0 ? true : false;
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

//            Debug.Log(collision_point_variance.sqrMagnitude);
            
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
                    jump_timer = 0;
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
                jumpCounter = JUMPMAX;
                jump_timer = 0;
                is_side_collision = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            is_side_collision = false;
            if (!is_jump && jumpCounter == JUMPMAX) {

                jumpCounter--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Enemy")&& immunTimer<=0) {
            hurt();

        }

        if (collision.gameObject.GetComponent<MissileDefault>() != null) {

            if (!collision.gameObject.GetComponent<MissileDefault>().IsPlayerPanetrate) { 
            
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void hurt(float immuneTime=2f) {

        immunTimer = immuneTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (immunTimer > 0) {
            immunTimer -= Time.fixedDeltaTime;
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
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
        if (is_jump)
        {

            if (jumpCounter == JUMPMAX)
            {

                if (jump_timer < JUMP_TIME)
                {
                    if (!jumping) {

                        playerAudio.JumpPlay();
                    }
                    jumping = true;
                    jump_timer += Time.fixedDeltaTime;
                    rb2.velocity = new Vector2(rb2.velocity.x, JumpPower * jump_timer / (JUMP_TIME));
                }
                else
                {
                    jumping = false;
                    is_jump = false;
                    jumpCounter--;
                }
            }
            else
            {
                if (jumpCounter > 0)
                {
                    playerAudio.JumpPlay();
                    rb2.velocity = new Vector2(rb2.velocity.x, JumpPower);
                    jumpCounter--;
                    is_jump = false;
                }
            }
        }
        else if (jumping) {
            jumping = false;
            is_jump = false;
            jumpCounter--;
        }

        if (is_fall) {

            //추락판정
            LifeSettingStart(Life - 1);
            is_fall = false;
        }

    }
}
