using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BuffDebuff
{
    public Stun(string name, float stunTime) : base(name, stunTime)
    {
        SpeedRatio = -1;
        DelegateContainers.Add(StunFunc);
    }

    private void StunFunc(GameObject Target, BuffDebuff state)
    {
        if (Target.GetComponent<PlayerAction>() != null)
        {
            if (state.Timer > 0)
            {
                Target.GetComponent<PlayerAction>().IsLimited = true;
            }
            else
            {
                Target.GetComponent<PlayerAction>().IsLimited = false;
            }
        }
    }
}
