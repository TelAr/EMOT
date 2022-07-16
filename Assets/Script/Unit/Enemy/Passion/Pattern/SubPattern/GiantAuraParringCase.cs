using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAuraParringCase : ParryingActiveDefault
{
    public override void ActiveFunc()
    {
        gameObject.GetComponent<Animator>().SetBool("IsLoop", false);
    }

}
