using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : UnitDefault
{
    public int health = 1;
    public Vector2 accel, m_Move;
    public float Speed = 1;
    public float JumpPower = 6f;
    private const float JUMP_TIME = 0.1f;
    private float jump_timer = 0f;
    private int jumpCounter;
    private const int JUMPMAX = 2;
    private const float GRAVITY = -20f;
    private bool is_side_collision;
    private bool is_under_collision;
    public bool is_jump;
    private Rigidbody2D rb2;
    // Start is called before the first frame update
    void Start()
    {
        is_side_collision = false;
        is_under_collision = false;
        jump_timer = 0;
        rb2 = this.gameObject.GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block_Up"))
        {
            Debug.Log("block");
            jumpCounter = JUMPMAX;
            jump_timer = 0;
            is_under_collision = true;
            transform.position -= new Vector3(0, rb2.velocity.y < 0 ? rb2.velocity.y * Time.deltaTime : 0);

        }

        if (collision.gameObject.CompareTag("Block_Side")) {
            transform.position -= new Vector3(rb2.velocity.x * Time.deltaTime, 0);
            is_side_collision = true;
        }
    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block_Up")) {

            is_under_collision = false;
        }
        if (collision.gameObject.CompareTag("Block_Side"))
        {
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



        rb2.velocity = new Vector3(m_Move.x * Speed, rb2.velocity.y + accel.y * Time.deltaTime);

        //벽 판정시
        if (is_side_collision)
        {

            rb2.velocity = new Vector3(0, rb2.velocity.y);
        }

        //jump
        if (is_jump) {

            if (jumpCounter == JUMPMAX)
            {

                if (jump_timer < JUMP_TIME)
                {
                    jump_timer += Time.deltaTime;
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
