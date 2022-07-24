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

    public int LazerCount = 0;
    public int DamagePerImmuneTime = 10;
    public float ImmuneTime = 0.5f;
    public LayerMask EnvironmentSet;

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
            lazers.Add(line);
        }
    }

    private void SetRayCastHit2DOnList()
    {
        hit2Ds = new();

        for (int t = 0; t < LazerCount; t++)
        {
            RaycastHit2D raycastHit2D = new RaycastHit2D();
            raycastHit2D.distance = 0;
        }
    }

    private void ReturnLazer()
    {
        foreach (var l in lazers)
        {
            LineRenderer lr = l.GetComponent<LineRenderer>();

            lr.material = DefaultMaterial;
            l.GetComponent<Damage>().enabled = false;
            l.SetActive(false);
        }
    }
}
