using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Execute : PatternDefault
{
    [Header("* Execute Pattern Value")]
    public float Predelay;
    public float JumpTime, AirHoldTime;
    [InfoBox("DownOffset is only Get X value")]
    public Vector3 JumpOffset;
    public Vector3 DownOffset;
    public Vector3 TargettingOriginPos;
    public Vector3 WarningSignScale;
    public float SlashDelay;
    public float SlashRemainTime;
    public Vector3 SlashPosOffset;
    public GameObject SlashObjectModel;

    private float timer = 0;
    private float step = 0;
    private GameObject slashObject = null, warningSignObject = null;
    private Vector3 vel;
    private Vector3 jumpPosition;
    private int targettingDirection;
    private bool isGround = false;
    private Rigidbody2D rb2D = null;
    public override void Setting()
    {
        timer = 0;
        step = 0;

        if (slashObject == null) { 
        
            slashObject = Instantiate(SlashObjectModel);
            slashObject.AddComponent<RemainAuraForExecute>();
            slashObject.GetComponent<RemainAuraForExecute>().RemainTime = SlashRemainTime;
            slashObject.SetActive(false);
        }

        if (warningSignObject == null) {

            warningSignObject = EffectPoolingController.Instance.GetWarningSign(Vector3.zero, WarningSignScale, 0, AirHoldTime, 1.5f);
            warningSignObject.SetActive(false);
        }

        if (rb2D == null) {

            rb2D = GetComponent<Rigidbody2D>();
        }

    }


    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            switch (step) { 
            
                case 0:
                    //
                    //기 모으는 연출 관련 로직
                    //
                    if (timer > Predelay) { 
                    
                        step = 1;
                        timer = 0;
                        jumpPosition = transform.position + JumpOffset;
                    }
                    break;
                case 1:
                    if (timer < JumpTime)
                    {

                        transform.position = Vector3.SmoothDamp(transform.position, jumpPosition, ref vel, JumpTime);
                    }
                    else {
                        timer = 0;
                        step = 2;
                        warningSignObject.SetActive(true);
                    }
                    break;
                case 2:
                    if(timer < AirHoldTime)
                    {
                        Vector3 targetPos = GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos;
                        targettingDirection = targetPos.x > TargettingOriginPos.x ? -1 : 1;
                        warningSignObject.transform.position = targetPos;
                        transform.position = new Vector3(targetPos.x+(DownOffset.x)* targettingDirection, jumpPosition.y);
                    }
                    else {
                        timer = 0;
                        step = 3;
                        isGround = false;
                        rb2D.velocity = new Vector2(0,-5);
                    }
                    break;
                case 3:
                    if (isGround)
                    {
                        timer = 0;
                        step = 4;
                    }
                    break;
                case 4:
                    if (SlashDelay < timer) {
                        Vector3 slashScale = slashObject.transform.localScale;
                        slashObject.transform.position = 
                            gameObject.transform.position + new Vector3(SlashPosOffset.x * targettingDirection * (-1), SlashPosOffset.y);
                        slashObject.transform.localScale = new Vector3(Mathf.Abs(slashScale.x) * targettingDirection * (-1), slashScale.y, slashScale.z);
                        slashObject.SetActive(true);

                        Stop();
                    }
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsRun) {

            if (step == 3) {

                rb2D.velocity += new Vector2(0, GameController.GetGameController.GRAVITY) * Time.fixedDeltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Block"))
        {

            isGround = true;
        }
    }
}
