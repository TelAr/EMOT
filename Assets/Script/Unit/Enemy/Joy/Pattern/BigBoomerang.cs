using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoomerang : PatternDefault
{
    [Header("* BigBoomerang Pattern Value")]
    public GameObject BigBoomerangModel;
    [Tooltip("Must be twice is Guaranteed, so realtime SwingAmound=SwingAmount+2")]
    public int SwingAmount;
    public float PreDelay;
    public float SwingSpeed;
    public float SwingDelay;
    public float LineWidth;
    public List<Vector2> LimitArea;
    [Tooltip("If this flag is True, Other pattern can be apeared")]
    public bool IsOtherPattern;
    [Tooltip("If level>=This Value, Boomerang is targetting player")]
    public int MinimumLevelToTargetting;

    public Color SPColor, EPColor;

    private GameController gc;
    private float timer;
    private GameObject boomerang = null;
    private Vector3 sp = Vector3.zero, dp = Vector3.zero;
    private GameObject nowLineRenderer = null, nextLineRendrer = null;
    private int beforeDirection, afterDirection;
    private int step;
    private int counter = 0;
    private Vector3 startPos;
    private bool isFlying;
    private float nowFlyTime;

    public override void Setting()
    {
        timer = 0;
        step = 0;
        counter = 0;
    }

    private void Reset()
    {
        Cooldown = 40f;
        Stack = 1;
        MaxDistance = 100;
        MinDistance = 0;

    }


    void Awake()
    {
        gc=GameController.GetGameController;

    }

    public override void Run()
    {
        if (boomerang != null && boomerang.activeSelf) 
        {
            Debug.Log("Deny");
            Caster.GetComponent<EnemyDefault>().Statement = "PatternDeny";
            return;
        }
        base.Run();

        isFlying = false;
        if (boomerang == null) {

            boomerang = Instantiate(BigBoomerangModel);
        }
        boomerang.SetActive(true);
        boomerang.transform.position = transform.position;
        Caster.GetComponent<EnemyDefault>().Statement = "BigBoomerang";
        beforeDirection = 0;
        afterDirection = 0;
        startPos = transform.position;

        initiateLine();

    }

    public override void Stop()
    {
        base.Stop();
    }




    // Update is called once per frame
    void Update()
    {
        if (Caster.GetFall()) {

            boomerang.SetActive(false);
            Stop();
        }

        if (IsRun || (IsOtherPattern&& (boomerang != null && boomerang.activeSelf)))
        {
            timer += Time.deltaTime;
            boomerang.transform.Rotate(0, 0, Time.deltaTime * 720);

            if (step == 0)
            {

                if (timer + SwingDelay > PreDelay && nextLineRendrer != null)
                {

                    if (timer < PreDelay - SwingDelay * 0.5f)
                    {

                        nextLineRendrer.GetComponent<LineRenderer>().startWidth = LineWidth * ((timer - (PreDelay - SwingDelay)) / (SwingDelay * 0.5f));
                    }
                    else
                    {

                        nextLineRendrer.GetComponent<LineRenderer>().endWidth = LineWidth * ((timer - (PreDelay - SwingDelay * 0.5f)) / (SwingDelay * 0.5f));
                    }
                }

                if (timer < PreDelay)
                {

                    boomerang.transform.position = startPos + new Vector3(0, LimitArea[1].y - startPos.y) * (1 - Mathf.Pow((timer / PreDelay) - 1, 2)) + new Vector3(0, 0, -0.1f);
                }
                else
                {
                    isFlying = true;
                    step = 1;
                    timer = 0;
                    nowLineRenderer = nextLineRendrer;

                    initiateLine();

                    SetNowFlyTime();
                    counter++;
                    if (IsOtherPattern) {

                        Stop();
                    }
                }
            }
            else if (step == 1)
            {

                if (isFlying)
                {
                    if (timer < nowFlyTime)
                    {
                        boomerang.transform.position = (nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0) * (nowFlyTime - timer)
                                    + nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) * timer) / nowFlyTime + new Vector3(0, 0, -0.1f);

                        if (timer < nowFlyTime * 0.5f)
                        {

                            nowLineRenderer.GetComponent<LineRenderer>().startWidth = LineWidth * ((nowFlyTime * 0.5f - timer) / (nowFlyTime * 0.5f));
                        }
                        else
                        {

                            nowLineRenderer.GetComponent<LineRenderer>().endWidth = LineWidth * ((nowFlyTime - timer) / (nowFlyTime * 0.5f));
                        }

                    }
                    else
                    {
                        isFlying = false;
                        timer = 0;
                        if (nowLineRenderer != null) nowLineRenderer.SetActive(false);
                    }

                }

                if (!isFlying)
                {

                    if (timer < SwingDelay)
                    {

                        if (timer < SwingDelay * 0.5f)
                        {

                            nextLineRendrer.GetComponent<LineRenderer>().startWidth = LineWidth * (timer / (SwingDelay * 0.5f));
                        }
                        else
                        {

                            nextLineRendrer.GetComponent<LineRenderer>().endWidth = LineWidth * ((timer - SwingDelay * 0.5f) / (SwingDelay * 0.5f));
                        }

                    }
                    else
                    {

                        isFlying = true;
                        timer = 0;
                        nowLineRenderer = nextLineRendrer;

                        if (counter<SwingAmount)
                        {
                            initiateLine();
                            SetNowFlyTime();
                            counter++;
                        }
                        else
                        {
                            step = 2;
                            nextLineRendrer = null;
                        }
                    }


                }


            }
            else if (step == 2)
            {

                if (isFlying)
                {
                    if (timer < nowFlyTime)
                    {
                        boomerang.transform.position = (nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0) * (nowFlyTime - timer)
                                    + nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) * timer) / nowFlyTime + new Vector3(0, 0, -0.1f);

                        if (timer < nowFlyTime * 0.5f)
                        {

                            nowLineRenderer.GetComponent<LineRenderer>().startWidth = LineWidth * ((nowFlyTime * 0.5f - timer) / (nowFlyTime * 0.5f));
                        }
                        else
                        {

                            nowLineRenderer.GetComponent<LineRenderer>().endWidth = LineWidth * ((nowFlyTime - timer) / (nowFlyTime * 0.5f));
                        }

                    }
                    else
                    {

                        isFlying = false;
                        timer = 0;
                        nowLineRenderer.SetActive(false);
                        nextLineRendrer = EffectPoolingController.Instance.GetLineRenderer
                        (new KeyValuePair<Vector3, Vector3>(nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1), transform.position + new Vector3(0, 0, 1)));
                    }

                }
                else
                {
                    
                    nextLineRendrer.GetComponent<LineRenderer>().SetPosition(1, transform.position + new Vector3(0, 0, 1));

                    if (timer < SwingDelay)
                    {

                        if (timer < SwingDelay * 0.5f)
                        {

                            nextLineRendrer.GetComponent<LineRenderer>().startWidth = LineWidth * (timer / (SwingDelay * 0.5f));
                        }
                        else
                        {

                            nextLineRendrer.GetComponent<LineRenderer>().endWidth = LineWidth * ((timer - SwingDelay * 0.5f) / (SwingDelay * 0.5f));
                        }

                    }
                    else
                    {

                        isFlying = true;
                        timer = 0;
                        nowLineRenderer = nextLineRendrer;
                        SetNowFlyTime();
                        step = 3;
                    }
                }
            }
            else if (step == 3) {

                nowLineRenderer.GetComponent<LineRenderer>().SetPosition(1, transform.position + new Vector3(0, 0, 1));
                if (timer < nowFlyTime)
                {
                    boomerang.transform.position = nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0) * Mathf.Pow((nowFlyTime - timer) / nowFlyTime, 2)
                                + nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) * (1 - Mathf.Pow((nowFlyTime - timer) / nowFlyTime, 2)) + new Vector3(0, 0, -0.1f);

                    if (timer < nowFlyTime * 0.5f)
                    {

                        nowLineRenderer.GetComponent<LineRenderer>().startWidth = LineWidth * ((nowFlyTime * 0.5f - timer) / (nowFlyTime * 0.5f));
                    }
                    else
                    {

                        nowLineRenderer.GetComponent<LineRenderer>().endWidth = LineWidth * ((nowFlyTime - timer) / (nowFlyTime * 0.5f));
                    }

                }
                else
                {
                    boomerang.SetActive(false);
                    isFlying = false;
                    timer = 0;
                    nowLineRenderer.SetActive(false);
                    if (!IsOtherPattern) Stop();
                }
            }
        }



    }

    private void SetNowFlyTime() {

        float nowDistance = (nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0) - nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1)).magnitude;
        nowFlyTime = nowDistance / SwingSpeed;
    }

    private void SetLine()
    {

        switch (afterDirection)
        {

            case 0:
                sp = new Vector3(Random.Range(LimitArea[0].x, LimitArea[1].x), LimitArea[1].y, 0);
                break;
            case 1:
                sp = new Vector3(LimitArea[1].x, Random.Range(LimitArea[0].y, LimitArea[1].y), 0);
                break;
            case 2:
                sp = new Vector3(Random.Range(LimitArea[0].x, LimitArea[1].x), LimitArea[0].y, 0);
                break;
            case 3:
                sp = new Vector3(LimitArea[0].x, Random.Range(LimitArea[0].y, LimitArea[1].y), 0);
                break;
            default:
                break;
        }
        beforeDirection = afterDirection;

        if (GameController.Level < MinimumLevelToTargetting)//올랜덤
        {
            while (beforeDirection == afterDirection)
            {

                afterDirection = Random.Range(0, 4);
            }
            switch (afterDirection)
            {

                case 0:
                    dp = new Vector3(Random.Range(LimitArea[0].x, LimitArea[1].x), LimitArea[1].y, 0);
                    break;
                case 1:
                    dp = new Vector3(LimitArea[1].x, Random.Range(LimitArea[0].y, LimitArea[1].y), 0);
                    break;
                case 2:
                    dp = new Vector3(Random.Range(LimitArea[0].x, LimitArea[1].x), LimitArea[0].y, 0);
                    break;
                case 3:
                    dp = new Vector3(LimitArea[0].x, Random.Range(LimitArea[0].y, LimitArea[1].y), 0);
                    break;
                default:
                    break;
            }
        }
        else
        { //플레이어 타게팅

            Vector3 unit = GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos - sp;
            float parameter;
            if (unit.y == 0 || unit.x == 0)
            {

                dp = new Vector3(Random.Range(LimitArea[0].x, LimitArea[1].x), LimitArea[0].y, 0);
                afterDirection = 2;
            }
            else
            {

                switch (beforeDirection)
                {
                    case 0:
                        parameter = (LimitArea[0].y - sp.y) / unit.y;
                        float xwide = parameter * unit.x + sp.x;
                        if (xwide > LimitArea[1].x)
                        {
                            afterDirection = 1;
                            dp = new Vector3(LimitArea[1].x, ((LimitArea[1].x - sp.x) / unit.x) * unit.y + sp.y, 0);
                        }
                        else if (xwide < LimitArea[0].x)
                        {
                            afterDirection = 3;
                            dp = new Vector3(LimitArea[0].x, ((LimitArea[0].x - sp.x) / unit.x) * unit.y + sp.y, 0);
                        }
                        else
                        {
                            afterDirection = 2;
                            dp = new Vector3(unit.x * parameter + sp.x, LimitArea[0].y);
                        }
                        break;
                    case 1:
                        parameter = (LimitArea[0].x - sp.x) / unit.x;
                        float ywide = parameter * unit.y + sp.y;
                        if (ywide > LimitArea[1].y)
                        {
                            afterDirection = 0;
                            dp = new Vector3(((LimitArea[1].y - sp.y) / unit.y) * unit.x + sp.x, LimitArea[1].y, 0);
                        }
                        else if (ywide < LimitArea[0].x)
                        {
                            afterDirection = 2;
                            dp = new Vector3(((LimitArea[0].y - sp.y) / unit.y) * unit.x + sp.x, LimitArea[0].y, 0);
                        }
                        else
                        {
                            afterDirection = 3;
                            dp = new Vector3(LimitArea[0].x, unit.y * parameter + sp.y);
                        }
                        break;
                    case 2:
                        parameter = (LimitArea[1].y - sp.y) / unit.y;
                        xwide = parameter * unit.x + sp.x;
                        if (xwide > LimitArea[1].x)
                        {
                            afterDirection = 1;
                            dp = new Vector3(LimitArea[1].x, ((LimitArea[1].x - sp.x) / unit.x) * unit.y + sp.y, 0);
                        }
                        else if (xwide < LimitArea[0].x)
                        {
                            afterDirection = 3;
                            dp = new Vector3(LimitArea[0].x, ((LimitArea[0].x - sp.x) / unit.x) * unit.y + sp.y, 0);
                        }
                        else
                        {
                            afterDirection = 0;
                            dp = new Vector3(unit.x * parameter + sp.x, LimitArea[1].y);
                        }
                        break;
                    case 3:
                        parameter = (LimitArea[1].x - sp.x) / unit.x;
                        ywide = parameter * unit.y + sp.y;
                        if (ywide > LimitArea[1].y)
                        {
                            afterDirection = 0;
                            dp = new Vector3(((LimitArea[1].y - sp.y) / unit.y) * unit.x + sp.x, LimitArea[1].y, 0);
                        }
                        else if (ywide < LimitArea[0].x)
                        {
                            afterDirection = 2;
                            dp = new Vector3(((LimitArea[0].y - sp.y) / unit.y) * unit.x + sp.x, LimitArea[0].y, 0);
                        }
                        else
                        {
                            afterDirection = 3;
                            dp = new Vector3(LimitArea[1].x, unit.y * parameter + sp.y);
                        }
                        break;
                    default:
                        break;
                }

            }

        }


    }

    private void initiateLine() {
        SetLine();
        KeyValuePair<Vector3, Vector3> sample = new KeyValuePair<Vector3, Vector3>(sp, dp);
        nextLineRendrer = EffectPoolingController.Instance.GetLineRenderer(sample);
        nextLineRendrer.GetComponent<LineRenderer>().startColor = SPColor;
        nextLineRendrer.GetComponent<LineRenderer>().endColor = EPColor;
        nextLineRendrer.GetComponent<LineRenderer>().startWidth = 0;
        nextLineRendrer.GetComponent<LineRenderer>().endWidth = 0;
    }

}
