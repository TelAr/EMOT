using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public enum State { 
    
        Global, PlayerTarget, Other 
    };

    static private LightController lightController = null;

    public GameObject Global, PlayerTarget;
    public GameObject SpotLightModel;
    public State state;

    private List<GameObject> spotLightObjects = new List<GameObject>();

    public static LightController Instance { 
    
        get { return lightController; }
    }

    public GameObject GetSpotLight() {

        GameObject returnObject = null;

        foreach (GameObject obj in spotLightObjects)
        {
            if (!obj.activeSelf) { 
            
                returnObject = obj;
                returnObject.SetActive(true);
                break;
            }
        }
        if (returnObject == null) {

            returnObject = Instantiate(SpotLightModel);
            spotLightObjects.Add(returnObject);
        }


        return returnObject;
    }
    void Awake()
    {
        if (lightController != null) { 
        
            DestroyImmediate(gameObject);
            return;
        }
        lightController = this;

        Global.GetComponent<Light2D>().intensity = 1;
        PlayerTarget.GetComponent<Light2D>().intensity = 0;
    }

    void LateUpdate()
    {
        PlayerTarget.transform.position = GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos;
    }
}
