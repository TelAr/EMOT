using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPhysical : UnitDefault
{
    [Tooltip("Posiotion where Scene is start")]
    public Vector3 OffsetPosition;

    [Tooltip("Minimum guaranteed ratio when button is pressed")]
    public float MinimumJumpPowerRatio;
    public float DownSpeedRatio;

    [Header("* Dash")]
    public float DashTime;
    public float DashDistance;

    [Header("* For GroundJudge")]
    public Transform GroundCheck;
    public float GroundCheckRadius;
    public PlayerStandJudge StandJudge;

    [Header("* SetEnvironment")]
    public LayerMask EnvironmentLayer;

    [Header("* Set Slope")]
    public float MaxSlopeAngle;
    public PhysicsMaterial2D NoFriction;
    public PhysicsMaterial2D FullFriction;
    public float SlopeCheckDistance;
    public float SlopeSlideSpeed = 5f;

    [SerializeField]
    private int state = 0;

    //const
    private const float JUMP_TIME = 0.1f;
    private const int JUMPMAX = 2;

    //statement OR effected
    [SerializeReference]
    private bool jumping;
    private float moving;
    private float direction;
    private Vector2 accel;
    private Vector2 collision_point_avg;
    private float jumpTimer = 0f;

    [SerializeReference]
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
    private bool isUniquAction;
    private Vector2 slopeNormalPerp;
    private bool isAir;
    private float jumpVelocity;
    private Vector2 slidingVelocity;

    [SerializeReference]
    private bool isOnSlope;

    [SerializeField]
    private float slopeDownAngle;

    [SerializeField]
    private float slopeSideAngle;
    private float lastSlopeAngle;

    [SerializeField]
    private bool canWalkOnSlope;

    [SerializeReference]
    private bool isGrounded;

    [SerializeReference]
    private bool isJump;

    public override void Reset()
    {
        base.Reset();
        DefaultSettingSpeed = 5;
        DefaultSettingJumpPower = 8;
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

            if (collision_point_avg.y < TargettingPos.y && isGrounded)
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

            if (collision_point_avg.y < TargettingPos.y && isGrounded)
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
            CheckGround();
            SlopeCheck();
            MovementControll();
            JumpControll();

            if (!isAir)
            {
                if (verticalInput < 0 || StandJudge.GetStuckState)
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
        }

        gameObject.GetComponent<SpriteRenderer>().flipX = direction > 0;

        if (isFall)
        {
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

    public bool IsJump
    {
        get { return isJump; }
        set { isJump = canWalkOnSlope || rb2.velocity.magnitude < 0.1f ? value : false; }
    }
    public bool IsUniquAction
    {
        get { return isUniquAction; }
        set { isUniquAction = value; }
    }

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

        if (isGrounded && !isOnSlope && !isJump)
        {
            state = 0;
            nowVelocity.Set(actualSpeed * moving, rb2.velocity.y);
        }
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJump)
        {
            state = 1;
            nowVelocity.Set(
                -actualSpeed * moving * slopeNormalPerp.x,
                -actualSpeed * moving * slopeNormalPerp.y
            );
        }
        else if (!isGrounded)
        {
            state = 2;
            nowVelocity.Set(actualSpeed * moving, rb2.velocity.y);
        }
        else
        {
            state = 3;
            nowVelocity.Set(actualSpeed * moving * 0.3f, rb2.velocity.y);
            if (!isJump)
            {
                nowVelocity += accel * Time.deltaTime * SlopeSlideSpeed;
            }
        }

        rb2.velocity = nowVelocity;
        rb2.velocity += accel * Time.fixedDeltaTime;
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
                {
                    gameObject.GetComponent<PlayerAudio>().JumpPlay();
                }

                rb2.velocity = new Vector2(rb2.velocity.x, actualJumpPower);
                jumpCounter--;
                jumpVelocity = 0;
                isJump = false;
            }
            return;
        }

        if (jumpTimer >= JUMP_TIME)
        {
            jumping = false;
            isJump = false;
            jumpCounter--;
            jumpVelocity = 0;
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

            float velocityY = actualJumpPower * timeDeltaRatio;
            rb2.velocity = new Vector2(rb2.velocity.x, velocityY);
        }
        else
        {
            jumpVelocity +=
                actualJumpPower * (1f - MinimumJumpPowerRatio) * Time.fixedDeltaTime / JUMP_TIME;
            rb2.velocity = new Vector2(rb2.velocity.x, jumpVelocity);
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
            SlopeCheckDistance,
            EnvironmentLayer
        );
        RaycastHit2D slopeHitBack = Physics2D.Raycast(
            checkPos,
            -transform.right,
            SlopeCheckDistance,
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
            SlopeCheckDistance,
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

        if (
            isOnSlope
            && (slopeDownAngle < MaxSlopeAngle && slopeSideAngle < MaxSlopeAngle)
            && Mathf.Abs(moving) < 0.01f
        )
        {
            rb2.sharedMaterial = FullFriction;
        }
        else
        {
            rb2.sharedMaterial = NoFriction;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            GroundCheck.position,
            GroundCheckRadius,
            EnvironmentLayer
        );

        if (isGrounded && !IsJump)
        {
            jumpCounter = JUMPMAX;
        }

        /*
        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }
        */
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
