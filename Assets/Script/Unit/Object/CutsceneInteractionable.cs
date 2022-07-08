using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInteractionable : InteractionableUnit
{
    public CutsceneDefault cd;

    public void Awake()
    {
        cd.enabled = false;
    }

    public override void GetInteractiveDown() {

        if (!cd.isActiveAndEnabled) { 
        
            cd.enabled = true;
        }

        if (cd.IsCanRead) {

            cd.GetNextCutscene();
        }
    }
}
