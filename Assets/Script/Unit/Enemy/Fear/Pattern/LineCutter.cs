using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LineCutter : PatternDefault
{
    [Header("* LineCutter Pattern Value")]
    public int LineNumber;
    public float LineWidth = 0.5f, LineDisapearWidth = 2f;
    [Tooltip("Time which Line is apeared")]
    public float PatternTime;
    [Tooltip("Time which field is getting dark")]
    public float FadeInTime;
    [Tooltip("Time which field is absolutely dark\n In half time, player light is smaller and disapear")]
    public float DarkTime;
    [Tooltip("Time which field is getting light")]
    public float FadeOutTime;
    [Tooltip("Player light's ridius when FadeInTime")]
    public float SightRadius;
    public float JudgeTime = 0.1f;
    [Tooltip("Linespawn's offset position\n" +
        "LineCutter's startPoint and endPoint is on the circle which circle's middlePoint is MiddlePoint, and circle's radius is Radius")]
    public Vector3 MiddlePoint;
    [Tooltip("Linespawn's offset radius\n" +
    "LineCutter's startPoint and endPoint is on the circle which circle's middlePoint is MiddlePoint, and circle's radius is Radius")]
    public float Radius;

    public int Damage = 20;
    public Color ReadyColor, ExplosionColor;

    private float timer = 0, subtimer = 0;
    private int step = 0;
    private List<GameObject> lines = new List<GameObject>();
    public override void Setting()
    {
        timer = 0;
        subtimer = 0;
        step = 0;
    }


    public override void Stop()
    {
        base.Stop();
        foreach (var line in lines) { 
        
            returnLine(line);
        }
        lines.Clear();
    }

    private GameObject callLine(Vector3 sp, Vector3 ep) { 
    
        GameObject line;
        KeyValuePair<Vector3, Vector3> pair = new KeyValuePair<Vector3, Vector3>(sp, ep);
        line = EffectPoolingController.Instance.GetLineRenderer(pair);
        if (line.GetComponent<EdgeCollider2D>() == null)
        {

            line.AddComponent<EdgeCollider2D>();
        }
        else if (!line.GetComponent<EdgeCollider2D>().isActiveAndEnabled) {

            line.GetComponent<EdgeCollider2D>().enabled = true;
        }
        line.GetComponent<EdgeCollider2D>().isTrigger = true;
        line.GetComponent<EdgeCollider2D>().points= new Vector2[]{sp, ep};


        if (line.GetComponent<Damage>() == null)
        {
            line.AddComponent<Damage>();
        }
        else if (!line.GetComponent<Damage>().isActiveAndEnabled)
        {
            line.GetComponent<Damage>().enabled = true;
        }
        line.GetComponent<Damage>().DamageValue = Damage;
        line.GetComponent<Damage>().IsEffected = false;


        line.GetComponent<LineRenderer>().startColor = ReadyColor - new Color(0, 0, 0, 1);
        line.GetComponent<LineRenderer>().endColor = ReadyColor - new Color(0, 0, 0, 1);

        line.GetComponent<LineRenderer>().startWidth = LineWidth;
        line.GetComponent<LineRenderer>().endWidth = LineWidth;

        line.tag = "Enemy";
        line.layer = 8;

        return line;
    }


    private void returnLine(GameObject line)
    {
        line.GetComponent<EdgeCollider2D>().isTrigger = false;
        line.GetComponent<EdgeCollider2D>().points = new Vector2[] { };
        line.GetComponent<EdgeCollider2D>().enabled = false;
        line.GetComponent<Damage>().IsEffected = false;

        line.tag = "Untagged";
        line.layer = 0;
        line.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            if (step == 0) {

                subtimer -= Time.deltaTime;
                if (subtimer <= 0) {

                    float rotate = Random.Range(0f, 360f);
                    float another = rotate + Random.Range(90f, 270f);

                    Vector3 sp = MiddlePoint + new Vector3(Mathf.Cos(rotate * Mathf.Deg2Rad), Mathf.Sin(rotate * Mathf.Deg2Rad)) * Radius;
                    Vector3 ep = MiddlePoint + new Vector3(Mathf.Cos(another * Mathf.Deg2Rad), Mathf.Sin(another * Mathf.Deg2Rad)) * Radius;

                    lines.Add(callLine(sp, ep));
                    subtimer = PatternTime/LineNumber;
                }
                foreach (var line in lines) { 
                
                    line.GetComponent<LineRenderer>().startColor += new Color(0,0,0,1)*Time.deltaTime/(PatternTime / LineNumber);
                    line.GetComponent<LineRenderer>().endColor += new Color(0, 0, 0, 1) * Time.deltaTime / (PatternTime / LineNumber);
                }

                if (timer <= FadeInTime)
                {

                    LightController.Instance.Global.GetComponent<Light2D>().intensity = 1 - (timer / FadeInTime);
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().intensity = (timer / FadeInTime);
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().pointLightOuterRadius = SightRadius * (timer / FadeInTime);
                }
                else {

                    LightController.Instance.Global.GetComponent<Light2D>().intensity = 0;
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().intensity = 1;
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().pointLightOuterRadius = SightRadius;
                }

                if (timer > PatternTime) {

                    step = 1;
                    timer = 0;
                }
            }
            if (step == 1) {

                if (timer < DarkTime * 0.5f)
                {
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().pointLightOuterRadius = SightRadius * (1-(timer / (DarkTime * 0.5f)));
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().intensity = (1 - (timer / (DarkTime * 0.5f)));
                }
                else {

                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().pointLightOuterRadius = 0;
                    LightController.Instance.PlayerTarget.GetComponent<Light2D>().intensity = 0;
                }

                if (timer > DarkTime)
                {

                    step = 2;
                    timer = 0;
                    foreach (var line in lines)
                    {
                        line.GetComponent<LineRenderer>().startColor = ExplosionColor;
                        line.GetComponent<LineRenderer>().endColor = ExplosionColor;
                        line.GetComponent<Damage>().IsEffected = true;
                    }
                    subtimer = JudgeTime;
                }
            }
            if (step == 2) {

                if (subtimer > 0)
                {
                    subtimer -= Time.deltaTime;
                    if (subtimer <= 0)
                    {
                        foreach (var line in lines)
                        {
                            line.GetComponent<Damage>().IsEffected = false;
                        }
                    }
                }

                foreach (var line in lines)
                {
                    float level = (timer / FadeOutTime);
                    line.GetComponent<LineRenderer>().startWidth = LineWidth * (1 - level) + LineDisapearWidth * level;
                    line.GetComponent<LineRenderer>().endWidth = LineWidth * (1 - level) + LineDisapearWidth * level;
                    line.GetComponent<LineRenderer>().startColor = ExplosionColor - new Color(0, 0, 0, 1) * level;
                    line.GetComponent<LineRenderer>().endColor = ExplosionColor - new Color(0, 0, 0, 1) * level;
                }

                if (timer < FadeOutTime * 0.5f)
                {

                    LightController.Instance.Global.GetComponent<Light2D>().intensity = timer / (FadeOutTime * 0.5f);
                }
                else {

                    LightController.Instance.Global.GetComponent<Light2D>().intensity = 1;
                }


                if (timer > FadeOutTime) { 
                
                    Stop();
                }
            }

        }
    }
}
