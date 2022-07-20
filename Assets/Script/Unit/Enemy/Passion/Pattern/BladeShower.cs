using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeShower : PatternDefault
{
    public Vector3 OriginPosition, JumpPosition;
    public float RepositionTime;
    public GameObject ShockWaveModel;
    public float LightBeamTime, SlashSummonTime, SlashJudgeTime, SlashEndTime;
    public int SlashCount;
    public Color BeforeColor, AfterColor;
    public float SlashDefaultWidth, SlashMaxWidth;
    public Material BeamMaterial;
    public Material DefaultMaterial;
    public float BeamWidth;
    public int SlashDamage;
    public float SlashHeigt, SlashWeightRange;
    public float JudgeTime = 0.1f;

    private float timer = 0, subtimer = 0;
    private GameObject[] shockWaves = new GameObject[2] { null, null };
    private GameObject lightBeam = null;
    [SerializeField]
    private List<SlashLine> slashList = new();
    [SerializeField]
    private int step = 0;
    private Vector3 vel;

    [System.Serializable]
    private class SlashLine {

        private readonly BladeShower Caster;
        private GameObject lineRender = null;
        [SerializeField]
        private float timer = 0;
        private int step = 0;

        public SlashLine(GameObject slr, BladeShower caster) {

            Caster = caster;
            lineRender = slr;

            lineRender.GetComponent<LineRenderer>().startColor = lineRender.GetComponent<LineRenderer>().endColor = Caster.BeforeColor;
            lineRender.GetComponent<LineRenderer>().startWidth = lineRender.GetComponent<LineRenderer>().endWidth = Caster.SlashDefaultWidth;
            lineRender.SetActive(true);

            if (lineRender.GetComponent<EdgeCollider2D>() == null)
            {
                lineRender.AddComponent<EdgeCollider2D>();
            }
            else if (!lineRender.GetComponent<EdgeCollider2D>().isActiveAndEnabled) {

                lineRender.GetComponent<EdgeCollider2D>().enabled = true;
            }
            Vector3[] points = new Vector3[lineRender.GetComponent<LineRenderer>().positionCount];
            lineRender.GetComponent<LineRenderer>().GetPositions(points);
            Vector2[] points2d = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++) { 
            
                points2d[i] = new Vector2(points[i].x, points[i].y);
            }
            lineRender.GetComponent<EdgeCollider2D>().points = points2d;
            lineRender.GetComponent<EdgeCollider2D>().isTrigger = true;

            if (lineRender.GetComponent<Damage>() == null)
            {
                lineRender.AddComponent<Damage>();
            }
            else if (!lineRender.GetComponent<Damage>().isActiveAndEnabled)
            {
                lineRender.GetComponent<Damage>().enabled = true;
            }
            lineRender.GetComponent<Damage>().DamageValue = Caster.SlashDamage;
            lineRender.GetComponent<Damage>().IsEffected = false;
        }

        public bool Tick() {

            if (lineRender == null) {

                return false;
            }

            timer+=Time.deltaTime;
            LineRenderer lr = lineRender.GetComponent<LineRenderer>();
            switch (step) { 
            
                case 0:
                    if (timer> Caster.SlashJudgeTime) {
                        lineRender.GetComponent<Damage>().IsEffected = true;
                        lr.startColor = lr.endColor = Caster.AfterColor;
                        lr.startWidth = lr.endWidth = Caster.SlashMaxWidth;
                        timer = 0;
                        step = 1;

                    }
                    break;
                case 1:
                    float ratio = timer / Caster.SlashEndTime;
                    lr.startWidth = lr.endWidth = Caster.SlashMaxWidth * (1-ratio);
                    if (timer > Caster.JudgeTime) {

                        lineRender.GetComponent<Damage>().IsEffected = false;
                    }

                    if (ratio > 1) { 
                    
                        timer = 0;
                        step = 2;
                        Caster.ReturnDamagedLine(lineRender);
                        lineRender = null;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        public GameObject GetLineRenderer {

            get { 
            
                return lineRender;
            }
        }

        public GameObject SetLineRenderer
        {

            set { 
            
                lineRender = value;
            }
        }
    }


    public override void Setting()
    {
        timer = 0;
        subtimer = 0;
        for (int t = 0; t < 2; t++) {
            if (shockWaves[t] == null) { 
            
                shockWaves[t]=Instantiate(ShockWaveModel);
                shockWaves[t].transform.localScale 
                    = new Vector3(Mathf.Abs(shockWaves[t].transform.localScale.x) * (t * 2 - 1)*(-1), shockWaves[t].transform.localScale.y);
                shockWaves[t].SetActive(false);
            }
        }

        slashList = new();
        step = 0;
    }

    public override void Stop()
    {
        base.Stop();
        foreach (var slash in slashList)
        {
            if (slash.GetLineRenderer != null) {

                ReturnDamagedLine(slash.GetLineRenderer);
                slash.SetLineRenderer = null;
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Block") && step == 1) {

            float height =  collision.contacts[0].point.y;

            for (int t = 0; t < 2; t++)
            {
                //logic need
                shockWaves[t].transform.position = new Vector3(gameObject.transform.position.x, height);
                shockWaves[t].SetActive(true);
            }

            //logic need
            lightBeam = CallLine();
            LineRenderer lr = lightBeam.GetComponent<LineRenderer>();
            lr.startColor = lr.endColor = BeforeColor - new Color(0, 0, 0, 0.5f);
            lr.startWidth = lr.endWidth = BeamWidth;
            lr.SetPosition(0, new Vector3(gameObject.transform.position.x, height, 2));
            lr.SetPosition(1, new Vector3(gameObject.transform.position.x, height + 100, 2));
            lr.numCapVertices = 0;
            lr.material = BeamMaterial;
            lightBeam.SetActive(true);

            step = 2;
        }
    }

    private void FixedUpdate()
    {
        if (IsRun) {

            if (step == 1)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, GameController.GetGameController.GRAVITY) * Time.fixedDeltaTime;
            }
            else
            {

                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

        }

    }

    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            bool isRemain = false;
            for (int t=0;t<slashList.Count;t++) {

                isRemain = (slashList[t].Tick() || isRemain);
            }

            switch (step) { 
                case 0://go to OriginPosition
                    transform.position = Vector3.SmoothDamp(transform.position, JumpPosition+OriginPosition, ref vel, RepositionTime * 0.5f);
                    if (timer > RepositionTime) {
                        timer = 0;
                        step = 1;
//                        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -50);
                    }
                    break;
                case 1://go down
                    //just phisics part, so go to FixedUpdate OR OnCollisionEnter2D
                    break;
                case 2://lightBeam
                    float ratio = timer / LightBeamTime;

                    LineRenderer lr = lightBeam.GetComponent<LineRenderer>();
                    lr.startWidth = lr.endWidth = (1 - Mathf.Pow(ratio, 2)) * BeamWidth;

                    if (ratio > 1) {
                        timer = 0;
                        subtimer = 0;
                        step = 3;
                        lr.material = DefaultMaterial;
                        lr.gameObject.SetActive(false);
                    }
                    break;
                case 3://Slash Open
                    subtimer -= Time.deltaTime;
                    if (subtimer < 0) {

                        if (slashList.Count >= SlashCount) {

                            step = 4;
                            timer = 0;
                            break;
                        }

                        Vector3 up, dp;
                        up = new Vector3(Random.Range(-SlashWeightRange * 0.5f, SlashWeightRange * 0.5f), SlashHeigt * 0.5f);
                        dp = new Vector3(Random.Range(-SlashWeightRange * 0.5f, SlashWeightRange * 0.5f), -SlashHeigt * 0.5f);

                        slashList.Add(new SlashLine(CallLine(new KeyValuePair<Vector3, Vector3>(up, dp)), this));
                        subtimer= SlashSummonTime/SlashCount;
                    }
                    break;
                case 4://stop
                    if(!isRemain) Stop();
                    break;
            }
        }
    }


    private GameObject CallLine(KeyValuePair<Vector3, Vector3>? points=null ) {

        GameObject line = null;

        line = EffectPoolingController.Instance.GetLineRenderer(points);

        line.SetActive(false);
        line.GetComponent<LineRenderer>().startColor = line.GetComponent<LineRenderer>().endColor = BeforeColor;
        line.GetComponent<LineRenderer>().startWidth = line.GetComponent<LineRenderer>().endWidth = 0;

        return line;
    }

    private void ReturnDamagedLine(GameObject line)
    {
        line.GetComponent<EdgeCollider2D>().isTrigger = false;
        line.GetComponent<EdgeCollider2D>().points = new Vector2[] { };
        line.GetComponent<EdgeCollider2D>().enabled = false;
        line.GetComponent<Damage>().IsEffected = false;
        line.GetComponent<LineRenderer>().material = DefaultMaterial;

        line.tag = "Untagged";
        line.layer = 0;
        line.SetActive(false);
    }
}
