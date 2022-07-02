using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInteractionable : InteractionableUnit
{
    public CutsceneDefault cd;

    public override void GetInteractiveDown() {

        if (cd.IsCanRead) {

            cd.GetNextCutscene();
        }
    }
}
