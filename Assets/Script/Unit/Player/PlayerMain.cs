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
    public Vector2 offset_position;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;
    private const float GRAVITY = -20f;

    //statement OR effected
    private bool is_side_collision;
    private Vector2 accel, m_Move;
    private Vector2 collision_point_avg;
    private Vector2 collision_point_variance;
    private float jump_timer = 0f;
    private int jumpCounter;
    private Rigidbody2D rb2;

    //action
    public bool is_jump;


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
    }

    public void Awake()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();

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

            Debug.Log(collision_point_variance.sqrMagnitude);
            
            if (collision_point_variance.x < collision_point_variance.y&&
                collision_point_variance.sqrMagnitude>0.00000001f)//수평 충돌
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

                Debug.Log("dive");
                jumpCounter--;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        accel = new Vector2(0, GRAVITY);
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
        if (is_jump) {

            if (jumpCounter == JUMPMAX)
            {

                if (jump_timer < JUMP_TIME)
                {
                    jump_timer += Time.fixedDeltaTime;
                    rb2.velocity = new Vector2(rb2.velocity.x, JumpPower * jump_timer / (JUMP_TIME));
                }
                else
                {
                    is_jump = false;
                    jumpCounter--;
                }
            }
            else {

                if (jumpCounter > 0)
                {
                    rb2.velocity = new Vector2(rb2.velocity.x, JumpPower);
                    jumpCounter--;
                    is_jump = false;
                }
            }
        }

        if (gameObject.transform.position.y < -100f) {

            //추락판정
            LifeSettingStart(Life - 1);
        }

    }
}
