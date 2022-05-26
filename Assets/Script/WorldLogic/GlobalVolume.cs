using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolume : MonoBehaviour
{
    static public GameObject GlobalVolumeObject;

    // Start is called before the first frame update
    void Awake()
    {
        GlobalVolumeObject = gameObject;   
    }



}
