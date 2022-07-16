using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeShower : PatternDefault
{
    public Vector3 OriginPosition;
    public float RepositionTime;
    public GameObject ShockWaveModel;
    public GameObject LightBeamModel;
    public float LightBeamTime, SlashStartTime, SlashEndTime;
    public int SlashCount;
    public Color BeforeColor, AfterColor;

    private Vector3 nowPos;
    private float timer = 0;
    private GameObject[] shockWaves = new GameObject[2] { null, null };
    private GameObject lightBeam = null;
    private List<GameObject> SlashList = new();
    private int step = 0;

    public override void Setting()
    {
        nowPos = gameObject.transform.position;
        timer = 0;
        for (int t = 0; t < 2; t++) {
            if (shockWaves[t] == null) { 
            
                shockWaves[t]=Instantiate(ShockWaveModel);
                shockWaves[t].SetActive(false);
            }
        }
        if (lightBeam == null) {

            lightBeam=Instantiate(LightBeamModel);
        }

        SlashList = new();
        step = 0;
    }

    public override void Stop()
    {
        base.Stop();
        foreach (GameObject slash in SlashList) if (slash.activeSelf) slash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            switch (step) { 
            
                case 0://go to OriginPosition
                    break;
                case 1://go down
                    break;
                case 2://lightBeam
                    break;
                case 3://Slash Open
                    break;
                case 4://slash close
                    break;
                case 5://stop
                    Stop();
                    break;
            }
        }
    }
}
