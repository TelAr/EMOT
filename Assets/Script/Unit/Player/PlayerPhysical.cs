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

    [Header("* For GroundJudge")]
    public PlayerGroundJudge Judge;

    [Header("* SetEnvironment")]
    public LayerMask EnvironmentLayer;

    [Header("* Set Slope")]
    public float MaxSlopeAngle;
    public PhysicsMaterial2D NoFriction;
    public PhysicsMaterial2D FullFriction;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;

    //statement OR effected
    private bool jumping;
    private float moving;
    private float direction;
    private Vector2 accel;
    private Vector2 collision_point_avg;
    private float jumpTimer = 0f;
    private int jumpCounter;
    private Rigidbody2D rb2;
    private CapsuleCollider2D colli2D;
    private GameController gc;
    private PlayerHealth ph;
    private PlayerAction pa;
    private PlayerVisual pv;
    private PlayerAudio playerAudio;
    private float DashTimer;
    private Vector3 DashSP;
    private Vector3 DashEP;
    private float verticalInput;
    private bool downState;
    private float bindTimer = 0f;

    private float slopeCheckDistance;
    private Vector2 slopeNormalPerp;

    [SerializeReference]
    private bool isOnSlope;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private bool canWalkOnSlope;

    public override void Reset()
    {
        base.Reset();
        Speed = 5;
        JumpPower = 8;
    }

    public void Start()
    {
        isAir = true;
        jumpTimer = 0;
        jumpCounter = 0;
        accel = Vector2.zero;
        moving = 0;
        rb2.velocity = Vector2.zero;
        transform.position = OffsetPosition;
        jumping = false;
        isUniquAction = false;
    }

    private void Awake()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        ph = gameObject.GetComponent<PlayerHealth>();
        pa = gameObject.GetComponent<PlayerAction>();
        pv = gameObject.GetComponent<PlayerVisual>();
        playerAudio = gameObject.GetComponent<PlayerAudio>();
        colli2D = gameObject.GetComponent<CapsuleCollider2D>();
        direction = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            CollsionBlock(collision);

            if (collision_point_avg.y < TargettingPos.y && Judge.IsGround)
            {
                jumpCounter = JUMPMAX;
                jumpTimer = 0;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            CollsionBlock(collision);

            if (collision_point_avg.y < TargettingPos.y && Judge.IsGround)
            {
                jumpCounter = JUMPMAX;
                jumpTimer = 0;
                isAir = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            if (!isJump && jumpCounter == JUMPMAX)
            {
                jumpCounter--;
            }
            isAir = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) { }

    void FixedUpdate()
    {
        if (bindTimer > 0)
        {
            rb2.velocity = Vector2.zero;
            bindTimer -= Time.fixedDeltaTime;
            return;
        }

        if (!isUniquAction)
        {
            SlopeCheck();
            MovementControll();
            JumpControll();

            if (!isAir && verticalInput < 0)
            {
                downState = true;
                rb2.velocity = new Vector2(rb2.velocity.x * DownSpeedRatio, rb2.velocity.y);
                colli2D.size = new Vector2(1, 1);
                colli2D.offset = new Vector2(0, 0.5f);
                pv.DownSprite();
            }
            else
            {
                downState = false;
                colli2D.size = new Vector2(1, 2);
                colli2D.offset = new Vector2(0, 1f);
                pv.NormalSprite();
            }
        }

        //차후 스트라이트, 스파인 작업에 따라 별도의 명령으로 바뀔 수 있음
        gameObject.GetComponent<SpriteRenderer>().flipX = direction > 0;

        if (isFall)
        {
            //추락판정
            ph.Hurt(20);

            isFall = false;
        }

        if (DashTimer < DashTime)
        {
            DashTimer += Time.fixedDeltaTime;
            rb2.velocity = DashEP;
        }
        else
        {
            if (isUniquAction)
            {
                isUniquAction = false;
                pv.NormalSprite();
                rb2.velocity = Vector2.zero;
            }
        }
    }

    //action state
    private bool isJump;
    public bool IsJump
    {
        get { return isJump; }
        set { isJump = value; }
    }
    private bool isUniquAction;
    public bool IsUniquAction
    {
        get { return isUniquAction; }
        set
        {

            isUniquAction = value;
        }
    }
    private bool isAir;
    public bool IsAir
    {
        get { return isAir; }
    }

    public bool IsBind
    {
        get
        {

            return bindTimer > 0f;
        }
    }

    public Vector3 TargettingPos
    {
        get
        {

            return gameObject.transform.position
                + new Vector3(0, transform.localScale.y * colli2D.size.y * 0.5f);
        }
    }

    public void Bind(float bindTime)
    {
        moving = 0;
        bindTimer = bindTime;
    }

    public void BindFree()
    {
        bindTimer = 0;
    }

    public void Moving(float x)
    {
        if (x > 0.1f)
        {
            direction = 1;
        }
        if (x < -0.1f)
        {
            direction = -1;
        }
        moving = x;
    }

    public void VerticalInput(float y)
    {
        verticalInput = y;
    }

    public void Dash()
    {
        DashTimer = 0;
        isUniquAction = true;

        Vector3 normalVector = new Vector3(
            direction,
            verticalInput < 0 ? (isAir ? verticalInput : 0) : verticalInput,
            0
        ).normalized;

        DashEP = normalVector * DashDistance / DashTime;

        rb2.velocity = Vector2.zero;

        if (downState)
        {
            playerAudio.SlidingPlay();
            pv.SlidingSprite();
        }
        else
        {
            playerAudio.DashPlay();
            pv.DashSprite();
        }
    }

    public float GetDirection
    {
        get { return direction; }
    }

    public bool GetIsDown
    {
        get { return downState; }
    }

    private void CollsionBlock(Collision2D collision)
    {
        collision_point_avg = Vector2.zero;
        foreach (ContactPoint2D point in collision.contacts)
        {
            collision_point_avg += point.point;
        }
        collision_point_avg /= collision.contacts.Length;
    }

    private void MovementControll()
    {
        accel = new Vector2(0, GameController.GetGameController.GRAVITY);
        Vector2 nowVelocity = new();

        if (Judge.IsGround && !isOnSlope && !isJump)
        {
            nowVelocity.Set(Speed * moving, 0.0f);
        }
        else if (Judge.IsGround && isOnSlope && canWalkOnSlope && !isJump)
        {
            nowVelocity.Set(Speed * moving * slopeNormalPerp.x, Speed * moving * slopeNormalPerp.y);
        }
        else
        {
            nowVelocity.Set(Speed * moving, rb2.velocity.y);
        }

        //        rb2.velocity = new Vector3(moving * Speed, rb2.velocity.y + accel.y * Time.fixedDeltaTime);
        rb2.velocity = nowVelocity + accel * Time.fixedDeltaTime;
    }

    private void JumpControll()
    {
        if (!isJump)
        {
            if (jumping)
            {
                jumping = false;
                isJump = false;
                jumpCounter--;
            }
            return;
        }
        if (jumpCounter != JUMPMAX)
        {
            if (jumpCounter > 0)
            {
                if (gameObject.GetComponent<PlayerAudio>() != null)
                    gameObject.GetComponent<PlayerAudio>().JumpPlay();
                rb2.velocity = new Vector2(rb2.velocity.x, JumpPower);
                jumpCounter--;
                isJump = false;
            }
            return;
        }

        if (jumpTimer >= JUMP_TIME)
        {
            jumping = false;
            isJump = false;
            jumpCounter--;
            return;
        }

        if (!jumping)
        {
            if (gameObject.GetComponent<PlayerAudio>() != null)
            {
                gameObject.GetComponent<PlayerAudio>().JumpPlay();
            }

            float timeDeltaRatio =
                MinimumJumpPowerRatio
                - (1f - MinimumJumpPowerRatio) * Time.fixedDeltaTime / JUMP_TIME;

            float velocityY = JumpPower * timeDeltaRatio;
            rb2.velocity = new Vector2(rb2.velocity.x, velocityY);
        }
        else
        {
            rb2.velocity +=
                new Vector2(0, JumpPower * (1f - MinimumJumpPowerRatio))
                * Time.fixedDeltaTime
                / JUMP_TIME;
        }
        jumping = true;
        jumpTimer += Time.fixedDeltaTime;
    }

    //reference: https://github.com/Bardent/Rigidbody2D-Slopes-Unity
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position;

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(
            checkPos,
            transform.right,
            slopeCheckDistance,
            EnvironmentLayer
        );
        RaycastHit2D slopeHitBack = Physics2D.Raycast(
            checkPos,
            -transform.right,
            slopeCheckDistance,
            EnvironmentLayer
        );

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            checkPos,
            Vector2.down,
            slopeCheckDistance,
            EnvironmentLayer
        );

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (slopeDownAngle > MaxSlopeAngle || slopeSideAngle > MaxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && moving == 0.0f)
        {
            rb2.sharedMaterial = FullFriction;
        }
        else
        {
            rb2.sharedMaterial = NoFriction;
        }
    }

    struct PhysicalData
    {
        public Vector3 pos;
    }

    public string GetJsonData()
    {
        PhysicalData data = new() { pos = gameObject.transform.position };
        string json = JsonUtility.ToJson(data);
        return json;
    }

    public void SetJsonData(string json)
    {
        PhysicalData data = JsonUtility.FromJson<PhysicalData>(json);
        gameObject.transform.position = data.pos;
    }
}
