using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassionMove : PatternDefault
{
    public float PreDelay;
    public float Speed;
    public float RushLengthAfterPlayer;

    public GameObject EffectModel;

    public GameObject Point3LineRendererModel;
    public Color AfterImageColor;
    public Material AfterImageMaterial;
    public float AfterImageWidth;

    private float timer;
    private int step;
    private GameObject point3LineRenderer = null;
    private GameObject effectObject = null;
    private Vector3 rushTargetting;
    private Vector3 beginPos;
    private float ratio;

    public override void Stop()
    {
        base.Stop();
        ((Passion)Caster).BodyBlowDamage.IsEffected = false;
        point3LineRenderer.SetActive(false);
    }

    public override void Setting()
    {
        timer = 0;
        step = 0;

        if (point3LineRenderer == null)
        {
            point3LineRenderer = Instantiate(Point3LineRendererModel);
            LineRenderer lr = point3LineRenderer.GetComponent<LineRenderer>();

            lr.widthMultiplier = AfterImageWidth;

            lr.startColor = lr.endColor = AfterImageColor;
            if (AfterImageMaterial != null)
            {
                lr.material = AfterImageMaterial;
            }
        }
        point3LineRenderer.SetActive(false);

        if (effectObject == null)
        {
            effectObject = Instantiate(EffectModel);
        }
        effectObject.transform.position = transform.position;
        effectObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (IsRun)
        {
            timer += Time.fixedDeltaTime;
            switch (step)
            {
                case 0:
                    ratio = timer / PreDelay;

                    //something pre_pose animation or effect need
                    effectObject.transform.localScale =
                        EffectModel.transform.localScale * Mathf.Pow(ratio, 2);
                    effectObject.transform.position = transform.position;

                    if (ratio > 1)
                    {
                        step = 1;
                        timer = 0;
                        point3LineRenderer.SetActive(true);
                        effectObject.SetActive(false);
                        SetRushRoute();
                        ((Passion)Caster).BodyBlowDamage.IsEffected = true;
                    }
                    break;
                case 1:

                    ratio = timer / ((rushTargetting - beginPos).magnitude / Speed);

                    float squRatio = 1 - Mathf.Pow(1 - ratio, 2);

                    transform.position = beginPos * (1 - squRatio) + rushTargetting * squRatio;

                    Vector3 linearPos = beginPos * (1 - ratio) + rushTargetting * ratio;
                    LineRenderer lr = point3LineRenderer.GetComponent<LineRenderer>();
                    lr.SetPosition(0, linearPos);
                    lr.SetPosition(1, (linearPos + transform.position) * 0.5f);
                    lr.SetPosition(2, transform.position);

                    if (ratio > 1)
                    {
                        Stop();
                    }
                    break;
            }
        }
    }

    private void SetRushRoute()
    {
        float length =
            GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos.x
            - transform.position.x;
        if (length < 0)
        {
            length -= RushLengthAfterPlayer;
        }
        else
        {
            length += RushLengthAfterPlayer;
        }
        rushTargetting = transform.position + new Vector3(length, 0, 0);
        beginPos = transform.position;
    }
}
