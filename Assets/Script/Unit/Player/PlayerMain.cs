using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : UnitDefault
{
    //public value
    public int health = 1;
    public float Speed = 1;
    public float JumpPower = 6f;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;
    private const float GRAVITY = -20f;

    //statement OR effected
    private bool jump_state;
    private bool is_side_collision;
    private bool is_under_collision;
    private Vector2 accel, m_Move;
    private Vector2 side_collision_point;
    private float jump_timer = 0f;
    private int jumpCounter;
    private Rigidbody2D rb2;

    //action
    private bool is_jump;

    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        is_side_collision = false;
        is_under_collision = false;
        jump_timer = 0;
        rb2 = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void Awake()
    {

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block_Up"))
        {
            jumpCounter = JUMPMAX;
            jump_timer = 0;
            is_under_collision = true;

        }
        if (collision.gameObject.CompareTag("Block_Side"))
        {
            Debug.Log("attach");
            side_collision_point = collision.GetContact(0).point;
            transform.position = new Vector3(side_collision_point.x < transform.position.x ? side_collision_point.x + transform.localScale.x * 0.5f :
                side_collision_point.x - transform.localScale.x * 0.5f, transform.position.y);
            is_side_collision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block_Up")) {
            is_under_collision = false;
        }
        if (collision.gameObject.CompareTag("Block_Side"))
        {
            Debug.Log("detach");
            is_side_collision = false;
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        accel = new Vector2(0, GRAVITY);

        //지상 판정시
        if (is_under_collision) {

            accel = Vector2.zero;
            rb2.velocity = new Vector3(rb2.velocity.x, rb2.velocity.y>0? rb2.velocity.y:0);
        }

        rb2.velocity = new Vector3(m_Move.x * Speed, rb2.velocity.y + accel.y * Time.fixedDeltaTime);

        //벽 판정시
        if (is_side_collision)
        {
            rb2.velocity = new Vector2(side_collision_point.x < transform.position.x ?
                (m_Move.x>0?rb2.velocity.x:0):
                (m_Move.x<0?rb2.velocity.x:0)
                , rb2.velocity.y);
//            rb2.velocity = new Vector3(0, rb2.velocity.y);
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

        


        /*
         
        물리 법칙 적용 

         */


    }
}
