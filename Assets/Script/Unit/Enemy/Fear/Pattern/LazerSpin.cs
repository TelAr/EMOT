using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerSpin : PatternDefault
{
    public float PositioningDelay;
    public Vector3 AnchorPosition;

    public Material DefaultMaterial;
    public float LazerLength = 30f;
    public float PreTransitionDelay;

    public float PreDelay;
    public Color PreColor;
    public float PreWidth;
    public Material PreMaterial;

    public float TransitionDelay;

    public float ActiveTime;
    public Color ActiveColor;
    public float ActiveWidth;
    public Material ActiveMaterial;
    public float ActiveRotatePerSec;

    public float DisapearTime;

    public int LazerCount = 0;
    public int DamagePerImmuneTime = 10;
    public float ImmuneTime = 0.5f;
    public LayerMask EnvironmentSet;

    public GameObject ObstacleModel;
    public int ObstacleCount;
    public float ObstacleRadius;
    public float ObstacleRotatePerSec;

    private float timer = 0;
    private int step = 0;
    private List<GameObject> lazers = new();
    private List<RaycastHit2D> hit2Ds = new();
    private float rotation;
    private Vector3 dampVel;

    public override void Setting()
    {
        timer = 0;
        step = 0;
        rotation = Random.Range(0, 360);
        SetRayCastHit2DOnList();
    }

    public override void Run()
    {
        base.Run();
        SetLazerList();
    }

    public override void Stop()
    {
        base.Stop();
        ReturnLazer();
    }

    private void Update()
    {
        if (IsRun)
        {
            timer += Time.deltaTime;
            float ratio;
            switch (step)
            {
                case 0:
                    ratio = timer / PositioningDelay;
                    transform.position = Vector3.SmoothDamp(
                        transform.position,
                        AnchorPosition,
                        ref dampVel,
                        PositioningDelay * 0.7f
                    );
                    if (ratio > 1)
                    {
                        timer = 0;
                        step = 1;
                    }
                    break;
                case 1:
                    ratio = timer / PreTransitionDelay;
                    UpdateLazerTransform(ratio);
                    if (ratio > 1)
                    {
                        timer = 0;
                        step = 2;
                    }
                    break;
                case 2:
                    ratio = timer / PreDelay;
                    UpdateLazerTransform();
                    if (ratio > 1)
                    {
                        foreach (var lazer in lazers)
                        {
                            LineRenderer lr = lazer.GetComponent<LineRenderer>();
                            if (ActiveMaterial != null)
                            {
                                lr.material = ActiveMaterial;
                            }
                        }
                        timer = 0;
                        step = 3;
                    }
                    break;
                case 3:
                    ratio = timer / TransitionDelay;

                    foreach (var lazer in lazers)
                    {
                        LineRenderer lr = lazer.GetComponent<LineRenderer>();
                        lr.startWidth = lr.endWidth = PreWidth * (1 - ratio) + ActiveWidth * ratio;
                        lr.startColor = lr.endColor = PreColor * (1 - ratio) + ActiveColor * ratio;
                    }
                    if (ratio > 1)
                    {
                        foreach (var lazer in lazers)
                        {
                            lazer.GetComponent<Damage>().IsEffected = true;
                        }
                        timer = 0;
                        step = 4;
                    }
                    break;
                case 4:
                    ratio = timer / ActiveTime;

                    rotation += ActiveRotatePerSec * Time.deltaTime;
                    UpdateLazerTransform();
                    if (ratio > 1)
                    {
                        step = 5;
                        timer = 0;
                    }
                    break;
                case 5:
                    ratio = timer / DisapearTime;
                    foreach (var lazer in lazers)
                    {
                        LineRenderer lr = lazer.GetComponent<LineRenderer>();
                        lr.startWidth = lr.endWidth = ActiveWidth * (1 - ratio);
                    }
                    if (ratio > 1)
                    {
                        Stop();
                    }
                    break;
            }
        }
    }

    private void SetLazerList()
    {
        lazers = new();

        for (int t = 0; t < LazerCount; t++)
        {
            GameObject line = EffectPoolingController.Instance.GetLineRenderer();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.startColor = lr.endColor = PreColor;
            if (PreMaterial != null)
            {
                lr.material = PreMaterial;
            }
            lr.startWidth = lr.endWidth = PreWidth;
            lr.SetPosition(0, ((Fear)Caster).BeamEyePosOffset);
            lr.SetPosition(1, ((Fear)Caster).BeamEyePosOffset);
            if (line.GetComponent<Damage>() == null)
            {
                line.AddComponent<Damage>();
            }
            if (!line.GetComponent<Damage>().isActiveAndEnabled)
            {
                line.GetComponent<Damage>().enabled = true;
            }
            line.GetComponent<Damage>().DamageValue = DamagePerImmuneTime;
            line.GetComponent<Damage>().ImmuneTime = ImmuneTime;
            line.GetComponent<Damage>().IsEffected = false;

            if (line.GetComponent<EdgeCollider2D>() == null)
            {
                line.AddComponent<EdgeCollider2D>();
            }
            if (!line.GetComponent<EdgeCollider2D>().isActiveAndEnabled)
            {
                line.GetComponent<EdgeCollider2D>().enabled = true;
            }
            line.GetComponent<EdgeCollider2D>().isTrigger = true;

            line.tag = "Enemy";

            lazers.Add(line);
        }
    }

    private void UpdateLazerTransform(float ratio = 1f)
    {
        for (int t = 0; t < LazerCount; t++)
        {
            hit2Ds[t] = Physics2D.Raycast(
                ((Fear)Caster).BeamEyePosOffset,
                DegreeToVector2(rotation + t * (360f / LazerCount)),
                LazerLength * ratio,
                EnvironmentSet
            );
        }

        for (int t = 0; t < LazerCount; t++)
        {
            lazers[t].GetComponent<LineRenderer>().SetPosition(0, ((Fear)Caster).BeamEyePosOffset);

            Vector2 EndPoint;
            if (hit2Ds[t].collider != null)
            {
                EndPoint = hit2Ds[t].point;
            }
            else
            {
                EndPoint =
                    (Vector2)((Fear)Caster).BeamEyePosOffset
                    + DegreeToVector2(rotation + t * (360f / LazerCount)) * LazerLength * ratio;
            }
            lazers[t].GetComponent<LineRenderer>().SetPosition(1, EndPoint);
            lazers[t].GetComponent<EdgeCollider2D>().points = new Vector2[]
            {
                ((Fear)Caster).BeamEyePosOffset,
                EndPoint
            };
        }
    }

    private void SetRayCastHit2DOnList()
    {
        hit2Ds = new();

        for (int t = 0; t < LazerCount; t++)
        {
            RaycastHit2D raycastHit2D = new RaycastHit2D();
            hit2Ds.Add(raycastHit2D);
        }
    }

    private void ReturnLazer()
    {
        foreach (var l in lazers)
        {
            LineRenderer lr = l.GetComponent<LineRenderer>();

            lr.material = DefaultMaterial;
            l.GetComponent<Damage>().enabled = false;
            l.GetComponent<EdgeCollider2D>().enabled = false;
            l.tag = "Untagged";
            l.SetActive(false);
        }
    }

    private Vector2 DegreeToVector2(float degree)
    {
        return new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
    }
}
