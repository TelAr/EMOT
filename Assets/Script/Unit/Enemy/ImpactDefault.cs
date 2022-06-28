using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class which contain method that will be called object is collision OR trigger
public abstract class ImpactDefault : MonoBehaviour
{
    public abstract void Impact(GameObject target = null);
}
