using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleShadowCatch : PatternDefault
{
    [Header("* MultipleShadowCatch Pattern Value")]
    public int Count = 3;
    public float FirstDelay = 1f;
    public float NextDelay = 0.3f;
    [Header("* Each ShadowCatch Pattern Value")]
    public float PreDelay;
    public float TargettingTime;
    public GameObject PlayerShadowModel;

    private float timer = 0;
    private int counter = 0;
    private int step = 0;
    private List<ShadowCatch> shadows = new List<ShadowCatch>();
    public override void Setting()
    {
        timer = 0;
        counter = 0;
        step = 0;

    }

    private ShadowCatch CallShadowCatch() {

        ShadowCatch shadow = null;

        foreach (ShadowCatch shadowCatch in shadows) {

            if (!shadowCatch.IsRun) {

                shadow=shadowCatch;
                break;
            }

        }
        if (shadow == null) { 
        
            shadow = gameObject.AddComponent<ShadowCatch>();
            shadow.IsIndependentPattern = false;
            shadow.IsMain = false;
            shadow.PreDelay = PreDelay;
            shadow.TargettingTime = TargettingTime;
            shadow.PlayerShadowModel = PlayerShadowModel;
            shadows.Add(shadow);
        }



        return shadow;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun) {
            timer += Time.deltaTime;
            switch (step) { 
            
                case 0:
                    CallShadowCatch().Run();
                    step++;
                    break;
                case 1:
                    if (timer > FirstDelay) {
                        step = 2;
                        timer = NextDelay;
                    }
                    break;
                case 2:
                    if (counter >= Count) { 
                    
                        Stop();
                    }
                    if (timer > NextDelay) {

                        CallShadowCatch().Run();
                        counter++;
                        timer = 0;
                    }
                    break;
                default:
                    break;
            }


        }
        
    }
}
