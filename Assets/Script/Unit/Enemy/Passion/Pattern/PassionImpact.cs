using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PassionImpact : PatternDefault
{
    public List<Vector3> PatternPositionList;
    public float JumpSpeed = 20f;
    public Vector3 OriginPosition;

    public GameObject TargettingSignModel;
    public Color TargettingSignColor;
    public float TargettingTime = 1f;
    public int RushCount = 3;
    public float RushMaxLength = 20f;
    public float RushSpeed = 5f;
    public float GaugeAmont = 10f;
    public float GaugeRecoverPerSec = 2f;
    public float GaugeDamagePerButton = 0.3f;

    public Color BeamColor;
    public float BeamMaxWidth;

    public GameObject GaugeObjectModel;
    public Vector3 GaugePosOffset;

    public float PlayerStunTime = 2f;

    private float timer = 0;
    private float fixedTimer = 0;
    private int step = 0;
    private int rushCounter = 0;
    private GameObject beam = null;
    private GameObject targettingSignObject = null;
    private GameObject gaugeObject = null;
    private Vector3 nomalVec = Vector3.zero;
    private float rushLeng = 0;
    private float nowGauge;
    private Vector3 impactPoint;
    private bool isSpecialParrying = false;

    public override void Setting()
    {
        timer = 0;
        step = 0;
        rushCounter = 0;
        isSpecialParrying = false;
        if (targettingSignObject == null)
        {
            targettingSignObject = Instantiate(TargettingSignModel);
            targettingSignObject.GetComponent<SpriteRenderer>().color = TargettingSignColor;
        }
        targettingSignObject.SetActive(false);

        if (gaugeObject == null)
        {
            gaugeObject = Instantiate(GaugeObjectModel);
        }
        gaugeObject.SetActive(false);
    }

    public override void Run()
    {
        base.Run();

        float maxDistance = 0;
        Vector3 targetPosition = OriginPosition;
        foreach (var pair in PatternPositionList)
        {
            if (
                maxDistance
                < (
                    pair - GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos
                ).magnitude
            )
            {
                maxDistance = (
                    pair - GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos
                ).magnitude;
                targetPosition = pair;
            }
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(
            targetPosition - gameObject.transform.position,
            JumpSpeed,
            GameController.GetGameController.GRAVITY
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Block"))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (step == 0)
            {
                TryRush();
                targettingSignObject.GetComponent<SpriteRenderer>().color =
                    TargettingSignColor - new Color(0, 0, 0, 1);
                targettingSignObject.SetActive(true);
            }
            else if (step == 2)
            {
                TryRush();
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        gaugeObject.SetActive(false);
        targettingSignObject.SetActive(false);
        ReturnBeam();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun)
        {
            float ratio;
            if (beam != null && beam.activeSelf)
            {
                beam.GetComponent<LineRenderer>()
                    .SetPosition(1, transform.position + new Vector3(0, 0, 1));
            }

            timer += Time.deltaTime;
            switch (step)
            {
                case 0:
                    //before landing step
                    break;
                case 1:
                    ratio = timer / TargettingTime;
                    if (ratio < 1)
                    {
                        targettingSignObject.GetComponent<SpriteRenderer>().color =
                            TargettingSignColor - new Color(0, 0, 0, 0.5f - ratio);
                        targettingSignObject.transform.localScale =
                            TargettingSignModel.transform.localScale * (1 - ratio);
                        targettingSignObject.transform.position = GameController.GetPlayer
                            .GetComponent<PlayerPhysical>()
                            .TargettingPos;
                        BeamEffect(ratio);
                    }
                    else
                    {
                        ReturnBeam();
                        SetBeam();
                        if (GameController.GetPlayer.GetComponent<PlayerAction>().IsParrying())
                        {
                            GameController.GetPlayer
                                .GetComponent<PlayerAudio>()
                                .ParryingSuccessPlay();
                            nowGauge = GaugeAmont * 0.5f;
                            impactPoint = GameController.GetPlayer
                                .GetComponent<PlayerPhysical>()
                                .TargettingPos;

                            GameController.GetPlayer.transform.position =
                                impactPoint + new Vector3(1, 0, 0);
                            transform.position = impactPoint + new Vector3(-1, 0, 0);

                            gaugeObject.transform.position = impactPoint + GaugePosOffset;
                            gaugeObject.SetActive(true);

                            GameController.GetPlayer.GetComponent<PlayerPhysical>().Bind(100);
                            step = 3;
                        }
                        else
                        {
                            GameController.GetPlayer.GetComponent<PlayerHealth>().Hurt(20, 0);
                            nomalVec = (
                                GameController.GetPlayer
                                    .GetComponent<PlayerPhysical>()
                                    .TargettingPos - gameObject.transform.position
                            ).normalized;
                            rushLeng = Mathf.Max(
                                RushMaxLength,
                                (
                                    GameController.GetPlayer
                                        .GetComponent<PlayerPhysical>()
                                        .TargettingPos - gameObject.transform.position
                                ).magnitude
                            );

                            fixedTimer = 0;
                            step = 2;
                        }
                        timer = 0;
                    }
                    break;
                case 2:
                    //parrying fail
                    //Phsical part is on FixedUpdate
                    break;
                case 3:
                    //parrying suceess

                    //player action logic
                    nowGauge += GaugeRecoverPerSec * Time.deltaTime;
                    if (isSpecialParrying)
                    {
                        nowGauge -= GaugeDamagePerButton;
                        GameController.GetPlayer.GetComponent<PlayerAudio>().ParryingSuccessPlay();
                        isSpecialParrying = false;
                    }

                    gaugeObject.GetComponent<Gauge>().GaugeValue = nowGauge / GaugeAmont;

                    if (nowGauge < 0)
                    {
                        //success event
                        GameController.GetPlayer.GetComponent<PlayerPhysical>().BindFree();
                        step = 4;
                    }
                    if (nowGauge > GaugeAmont)
                    {
                        //fail event
                        GameController.GetPlayer.GetComponent<PlayerPhysical>().BindFree();
                        BuffDebuff stun = new("Stun");
                        stun.Timer = PlayerStunTime;
                        stun.SpeedRatio = -1;
                        GameController.GetPlayer
                            .GetComponent<BuffDebuffApplyer>()
                            .AddBuffDebuff(stun);
                        step = 4;
                    }

                    break;
                case 4:
                    ratio = timer / TargettingTime;
                    if (ratio < 1)
                    {
                        BeamEffect(ratio);
                    }
                    else
                    {
                        Stop();
                    }

                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsRun)
        {
            if (step == 0)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity +=
                    new Vector2(0, GameController.GetGameController.GRAVITY) * Time.fixedDeltaTime;
            }
            else if (step == 2)
            {
                fixedTimer += Time.fixedDeltaTime;
                gameObject.GetComponent<Rigidbody2D>().velocity = nomalVec * RushSpeed;

                if (fixedTimer > rushLeng / RushSpeed)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    TryRush();
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    private void SetBeam()
    {
        GameObject retObject = null;

        retObject = EffectPoolingController.Instance.GetLineRenderer();
        LineRenderer lr = retObject.GetComponent<LineRenderer>();
        lr.startColor = lr.endColor = BeamColor;
        lr.startWidth = 0;
        lr.endWidth = BeamMaxWidth;
        lr.SetPosition(0, transform.position + new Vector3(0, 0, 1));
        lr.SetPosition(1, transform.position + new Vector3(0, 0, 1));
        lr.numCapVertices = 10;
        beam = retObject;
    }

    private void ReturnBeam()
    {
        if (beam != null)
        {
            beam.SetActive(false);
        }
    }

    public void OnParrying()
    {
        if (step == 3)
        {
            isSpecialParrying = true;
        }
    }

    private void TryRush()
    {
        timer = 0;
        if (rushCounter < RushCount)
        {
            rushCounter++;
            step = 1;
        }
        else
        {
            step = 4;
        }
    }

    private void BeamEffect(float ratio)
    {
        if (beam != null && beam.activeSelf)
        {
            beam.GetComponent<LineRenderer>().endWidth = BeamMaxWidth * (1 - ratio);
        }
    }
}
