using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeImpact : ImpactDefault
{
    public GameObject SmokeAreaModel;
    private GameObject SmokeArea = null;
    public override void Impact(GameObject target = null)
    {
        SmokeArea = Instantiate(SmokeAreaModel);
        SmokeArea.transform.position = transform.position + new Vector3(0, 0, -1f);
    }

}
