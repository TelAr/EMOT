using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Just caculation FPS info 
public class FPSShow : MonoBehaviour
{
    public float ShowDelay = 1f;
    private float DisplayDeltaTime = 0f;

    static private FPSShow instance = null;

    private void Awake()
    {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        StartCoroutine(ShowDelta());
    }

    IEnumerator ShowDelta() {

        while (true) {
            yield return new WaitForSeconds(ShowDelay);
            DisplayDeltaTime = Time.unscaledDeltaTime;
        }
        
    }

    static public float FPS {

        get {
            return 1.0f/ instance.DisplayDeltaTime;
        }
    }

}
