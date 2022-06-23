using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysical : UnitDefault
{
    [Header("* Conrolled Value")]
    public float Speed = 1;
    public float JumpPower = 6f;
    [Tooltip("Posiotion where Scene is start")]
    public Vector3 OffsetPosition;
    [Tooltip("Minimum guaranteed ratio when button is pressed")]
    public float MinimumJumpPowerRatio;
    public float DownSpeedRatio;
    [Header("* Dash")]
    public float DashTime;
    public float DashDistance;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;

    //statement OR effected
    private bool is_side_collision;
    private bool jumping;
    private float moving, direction;
    private Vector2 accel;
    private Vector2 collision_point_avg;
    private Vector2 collision_point_variance;
    private float jumpTimer = 0f;
    private int jumpCounter;
    private Rigidbody2D rb2;
    private BoxCollider2D colli2D;
    private GameController gc;
    private PlayerHealth ph;
    private PlayerAction pa;
    private PlayerVisual pv;
    private PlayerAudio playerAudio;
    private float DashTimer;
    private Vector3 DashSP, DashEP;
    private float verticalInput;
    private bool downState;
    private float bindTimer = 0f;

    //action state
    private bool isJump;
    public bool IsJump {

        get { return isJump; }
        set { isJump = value; }
    }
    private bool isUniquAction;
    public bool IsUniquAction {

        get { return isUniquAction; }
    }
    private bool isAir;
    public bool IsAir {

        get { return isAir; }
    }

    public bool IsBind {

        get { 
        
            return bindTimer > 0f;
        }
    }

    public Vector3 TargettingPos
    {
        get {

            return gameObject.transform.position + new Vector3(0, transform.localScale.y * colli2D.size.y * 0.5f);
        }
    }


    public override void Reset()
    {
        base.Reset();
        Speed = 5;
        JumpPower = 8;
    }


    // Start is called before the first frame update
    public void Start()
    {
        is_side_collision = false;
        jumpTimer = 0;
        jumpCounter = 0;
        accel = Vector2.zero;
        moving = 0;
        rb2.velocity = Vector2.zero;
        transform.position = OffsetPosition;
        jumping = false;
        isUniquAction = false;
    }

    protected override void Awake()
    {
        base.Awake();
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        ph = gameObject.GetComponent<PlayerHealth>();
        pa = gameObject.GetComponent<PlayerAction>();
        pv = gameObject.GetComponent<PlayerVisual>();
        playerAudio = gameObject.GetComponent<PlayerAudio>();
        colli2D = gameObject.GetComponent<BoxCollider2D>();
        direction = 1;
    }

    public void Bind(float bindTime) {
        moving = 0;
        bindTimer = bindTime;
    }

    public void BindFree() {

        bindTimer = 0;
    }

    public void Moving(float x) {

        if (x > 0.1f) {

            direction = 1;
        }
        if (x < -0.1f) {

            direction = -1;
        }
        moving = x;
    }

    public void VerticalInput(float y) {

        verticalInput = y;
    }

    public void Dash()
    {
        DashTimer = 0;
        isUniquAction = true;
        DashEP = new Vector3(direction, verticalInput < 0 ? (isAir ? verticalInput : 0) : verticalInput, 0).normalized * DashDistance / DashTime;
        rb2.velocity = Vector2.zero;
        if (downState)
        {
            playerAudio.SlidingPlay();
            pv.SlidingSprite();
        }
        else {
            playerAudio.DashPlay();
            pv.DashSprite();
        }
    }

    public float GetDirection {
        get {
            return direction;
        } 
    }

    public bool GetIsDown {
        get {
            return downState;
        }
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
                    isAir = false;
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
            isAir = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bindTimer > 0) {

            rb2.velocity = Vector2.zero;
            bindTimer -= Time.fixedDeltaTime;
            return;
        }

        if (!isUniquAction) {

            accel = new Vector2(0, GameController.GetGameController().GRAVITY);
            rb2.velocity = new Vector3(moving * Speed, rb2.velocity.y + accel.y * Time.fixedDeltaTime);
            //벽 판정시
            if (is_side_collision)
            {
                rb2.velocity = new Vector2(collision_point_avg.x < transform.position.x ?
                    (moving > 0 ? rb2.velocity.x : 0) :
                    (moving < 0 ? rb2.velocity.x : 0)
                    , rb2.velocity.y);
            }

            //jump
            if (isJump)
            {

                if (jumpCounter == JUMPMAX)
                {

                    if (jumpTimer < JUMP_TIME)
                    {
                        if (!jumping)
                        {

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
            else if (jumping)
            {
                jumping = false;
                isJump = false;
                jumpCounter--;
            }

            if (!isAir && verticalInput < 0)
            {
                downState = true;
                rb2.velocity = new Vector2(rb2.velocity.x * DownSpeedRatio, rb2.velocity.y);
                colli2D.size = new Vector2(1, 1);
                colli2D.offset = new Vector2(0, 0.5f);
                pv.DownSprite();
            }
            else {

                downState = false;
                colli2D.size = new Vector2(1, 2);
                colli2D.offset = new Vector2(0, 1f);
                pv.NormalSprite();
            }

        }
        

        //차후 스트라이트, 스파인 작업에 따라 별도의 명령으로 바뀔 수 있음
        gameObject.GetComponent<SpriteRenderer>().flipX = direction > 0;

        if (isFall) {

            //추락판정
            ph.Hurt(20);

            isFall = false;
        }


        if (DashTimer < DashTime)
        {

            DashTimer += Time.fixedDeltaTime;
            rb2.velocity = DashEP;
        }
        else {

            if (isUniquAction) {

                isUniquAction = false;
                pv.NormalSprite();
                rb2.velocity = Vector2.zero;
            }

        }

    }


    struct PhysicalData
    {
        public Vector3 pos;


    }

    public string GetJsonData()
    {
        PhysicalData data = new()
        {
            pos = gameObject.transform.position
        };
        string json = JsonUtility.ToJson(data);
        return json;
    }

    public void SetJsonData(string json)
    {

        PhysicalData data = JsonUtility.FromJson<PhysicalData>(json);
        gameObject.transform.position = data.pos;

    }
}
