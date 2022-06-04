using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoomerang : PatternDefault
{
    public GameObject BigBoomerangModel;
    public int SwingAmount;
    public float PreDelay;
    public float SwingSpeed;
    public float SwingDelay;
    public float LineWidth;
    public List<Vector2> LimitArea;
    public bool IsOtherPattern;

    private float timer;
    private GameObject boomerang = null;
    private Queue<KeyValuePair<Vector3, Vector3>> lineQueue = new Queue<KeyValuePair<Vector3, Vector3>>();
    private GameObject nowLineRenderer = null, nextLineRendrer = null;
    private int beforeDirection, afterDirection;
    private int step;
    private Vector3 startPos;
    private bool isFlying;
    private float nowFlyTime;

    public override void Setting()
    {
        timer = 0;
        step = 0;

    }

    private void Reset()
    {
        cooldown = 40f;
        stack = 1;
        max_distance = 100;
        min_distance = 0;

    }


    // Start is called before the first frame update
    void Awake()
    {


    }

    public override void Run()
    {
        if (boomerang != null && boomerang.activeSelf) 
        {
            Debug.Log("Deny");
            caster.GetComponent<EnemyDefault>().statement = "PatternDeny";
            return;
        }
        base.Run();

        isFlying = false;
        if (boomerang == null) {

            boomerang = Instantiate(BigBoomerangModel);
        }
        boomerang.SetActive(true);
        boomerang.transform.position = transform.position;
        caster.GetComponent<EnemyDefault>().statement = "BigBoomerang";
        beforeDirection = 0;
        afterDirection = 0;
        startPos = transform.position;
        //위치 미리 생성
        for (int t = 0; t < SwingAmount; t++) {


            Vector3 sp = Vector3.zero, dp = Vector3.zero;

            switch (afterDirection) { 
            
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

            while (beforeDirection == afterDirection) {

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

            lineQueue.Enqueue(new KeyValuePair<Vector3, Vector3>(sp, dp));
        }

        KeyValuePair<Vector3, Vector3> sample=new KeyValuePair<Vector3, Vector3>();

        if (lineQueue.TryPeek(out sample)) {

            nextLineRendrer = EffectPoolingController.EffectObjectController.GetLineRenderer(sample);
            nextLineRendrer.GetComponent<LineRenderer>().startWidth = 0;
            nextLineRendrer.GetComponent<LineRenderer>().endWidth = 0;
            lineQueue.Dequeue();
        }

    }

    public override void Stop()
    {
        base.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_run || (IsOtherPattern&& (boomerang != null && boomerang.activeSelf)))
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

                    boomerang.transform.position = startPos + new Vector3(0, LimitArea[1].y-startPos.y) * (1 - Mathf.Pow((timer / PreDelay) - 1, 2));
                }
                else
                {
                    isFlying = true;
                    step = 1;
                    timer = 0;
                    nowLineRenderer = nextLineRendrer;
                    KeyValuePair<Vector3, Vector3> sample = new KeyValuePair<Vector3, Vector3>();
                    if (lineQueue.TryPeek(out sample))
                    {

                        nextLineRendrer = EffectPoolingController.EffectObjectController.GetLineRenderer(sample);
                        nextLineRendrer.GetComponent<LineRenderer>().startWidth = 0;
                        nextLineRendrer.GetComponent<LineRenderer>().endWidth = 0;
                        lineQueue.Dequeue();
                    }
                    else
                    {

                        nextLineRendrer = null;
                    }
                    float nowDistance = (nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) - nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                    nowFlyTime = nowDistance / SwingSpeed;

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
                                    + nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) * timer) / nowFlyTime;

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
                        KeyValuePair<Vector3, Vector3> sample = new KeyValuePair<Vector3, Vector3>();
                        if (lineQueue.TryPeek(out sample))
                        {

                            nextLineRendrer = EffectPoolingController.EffectObjectController.GetLineRenderer(sample);
                            nextLineRendrer.GetComponent<LineRenderer>().startWidth = 0;
                            nextLineRendrer.GetComponent<LineRenderer>().endWidth = 0;
                            lineQueue.Dequeue();
                        }
                        else
                        {
                            step = 2;
                            nextLineRendrer = null;
                        }
                        float nowDistance = (nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) - nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                        nowFlyTime = nowDistance / SwingSpeed;
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
                                    + nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) * timer) / nowFlyTime;

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
                        nextLineRendrer = EffectPoolingController.EffectObjectController.GetLineRenderer
                        (new KeyValuePair<Vector3, Vector3>(nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1), Vector3.zero));
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
                        float nowDistance = (nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) - nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                        nowFlyTime = nowDistance / SwingSpeed;
                        step = 3;
                    }
                }
            }
            else if (step == 3) {

                nowLineRenderer.GetComponent<LineRenderer>().SetPosition(1, transform.position + new Vector3(0, 0, 1));
                if (timer < nowFlyTime)
                {

                    /*
                     *             transform.position = initiatingPos * Mathf.Pow((timer - (oneWayTime)) / oneWayTime, 2)
                     *               + targetPos * (1 - Mathf.Pow((timer - (oneWayTime)) / oneWayTime, 2));
                     * 
                    */
                    boomerang.transform.position = nowLineRenderer.GetComponent<LineRenderer>().GetPosition(0) * Mathf.Pow((nowFlyTime - timer) / nowFlyTime, 2)
                                + nowLineRenderer.GetComponent<LineRenderer>().GetPosition(1) * (1 - Mathf.Pow((nowFlyTime - timer) / nowFlyTime, 2));

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
}
