using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkenFear : PatternDefault
{
    [Header("* MultipleShadowCatch Pattern Value")]
    public GameObject SpawnerModel;
    [Tooltip("Based on the player's position where spawner is spawned")]
    public Vector3 SpawnOffset;
    [Tooltip("If time is over, pattern is stop forced")]
    public float LimitedWaittingTime;
    [Header("* Eyebeam Value")]
    public Color EyeBeamColor;
    public Material EyeBeamMaterial;
    public float BeamPreDelay, BeamTime, BeamOutTime;
    public float BeamWidth;
    public float BeamHalfLength;
    public GameObject StrokeModel;


    private GameObject spawner = null;
    private Material defaultMaterial = null;
    private GameObject eyeBeam = null;
    private float timer;
    private int step = -1;
    private Vector3 eyeBeamTarget;
    private float direction;
    private GameObject stroke = null;


    public int GetStep{
    
    
        get{ return step; }
    }

    public override void Setting()
    {
        timer = 0;
        step = -1;
        if (spawner == null) { 
        
            spawner = Instantiate(SpawnerModel);
            spawner.GetComponent<SunkenSpawner>().Caster = this;
            spawner.SetActive(false);
        }

        if (stroke == null) { 
        
            stroke = Instantiate(StrokeModel);
            stroke.SetActive(false);
        }
    }

    public override void Run()
    {
        base.Run();
        spawner.transform.position = GameController.GetPlayer.transform.position + SpawnOffset;
        spawner.SetActive(true);
    }

    public void EyebeamCall(Vector3 target) {

        if (eyeBeam == null)
        {

            eyeBeam = EffectPoolingController.Instance.GetLineRenderer();
        }
        else { 
        
            eyeBeam.gameObject.SetActive(true);
        }
        if (defaultMaterial == null) {

            defaultMaterial = eyeBeam.GetComponent<LineRenderer>().material;
        }
        eyeBeam.GetComponent<LineRenderer>().material = EyeBeamMaterial;
        eyeBeam.GetComponent<LineRenderer>().startWidth = eyeBeam.GetComponent<LineRenderer>().endWidth = BeamWidth;
        eyeBeam.GetComponent<LineRenderer>().startColor = eyeBeam.GetComponent<LineRenderer>().endColor = EyeBeamColor;
        step = 0;
        eyeBeamTarget = target;
        if (transform.position.x > target.x)
        {

            direction = 1;
        }
        else {

            direction = -1;
        }
    }

    public void EyebeamReturn()
    {
        if (eyeBeam != null) {
            eyeBeam.GetComponent<LineRenderer>().material = defaultMaterial;
            eyeBeam.SetActive(false);
            eyeBeam = null;
        }
    }

    private void Update()
    {

        if (IsRun) {
            timer += Time.deltaTime;

            if (step != -1) {
                eyeBeam.GetComponent<LineRenderer>().SetPosition(0, transform.position + ((Fear)Caster).BeamEyePosOffset);
            }

            switch (step)
            {
                case -1:
                    if (LimitedWaittingTime < timer)
                    {
                        Stop();
                    }
                    break;
                case 0:
                    if (timer < BeamPreDelay)
                    {
                        eyeBeam.GetComponent<LineRenderer>().SetPosition(1,
                            (transform.position + ((Fear)Caster).BeamEyePosOffset) * (1 - timer / BeamPreDelay) 
                            + (timer / BeamPreDelay) * (eyeBeamTarget + direction * new Vector3(BeamHalfLength, 0, 0)));
                    }
                    else{

                        step = 1;
                        timer = 0;
                        stroke.transform.position = eyeBeamTarget + direction * new Vector3(BeamHalfLength, 0, 0);
                        if (direction > 0)
                        {

                            stroke.GetComponent<SpriteRenderer>().flipX = false;
                        }
                        else {

                            stroke.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        stroke.SetActive(true);
                    }
                    break;
                case 1:
                    if (timer < BeamTime)
                    {
                        eyeBeam.GetComponent<LineRenderer>().SetPosition(1,
                            eyeBeamTarget + 2 * (0.5f - timer / BeamTime) * direction * new Vector3(BeamHalfLength, 0, 0));
                        stroke.transform.position = eyeBeamTarget + 2 * (0.5f - timer / BeamTime) * direction * new Vector3(BeamHalfLength, 0, 0);
                    }
                    else {

                        step = 2;
                        timer = 0;

                    }
                    break;
                case 2:
                    if (timer < BeamOutTime)
                    {
                        eyeBeam.GetComponent<LineRenderer>().startWidth = eyeBeam.GetComponent<LineRenderer>().endWidth 
                            = BeamWidth * (1 - timer / BeamOutTime);
                        stroke.GetComponent<SpriteRenderer>().color = Color.white - new Color(0, 0, 0, 1) * (Time.deltaTime / BeamOutTime);
                    }
                    else
                    {
                        //call tentacle;
                        stroke.SetActive(false);
                        step = 3;
                        timer = 0;
                    }
                    break;
                case 3:
                    EyebeamReturn();
                    Stop();
                    break;
                default:
                    break;
            }
        }

    }

}
