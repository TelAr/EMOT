using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolume : MonoBehaviour
{
    static public GameObject GlobalVolumeObject = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (GlobalVolumeObject != null) { 
        
            DestroyImmediate(this.gameObject);
            return;
        }
        GlobalVolumeObject = gameObject;   
    }



}
