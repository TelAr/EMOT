using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionableUnit : MonoBehaviour
{
    private bool interactionableState = true;

    public bool IsInteractive
    {
        get { 
        
            return interactionableState;
        }
    }
    //Get down interactive case
    public virtual void GetInteractiveDown()
    {
    }
    //Get Up Or Away interactove case
    public virtual void GetInteractiveUp()
    {
    }
}
