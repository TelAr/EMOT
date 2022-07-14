using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Pattern type: Singleton
 * Only one instance manage this class
 * When called Get***, instance return effect object that initialization.
 */
public class EffectPoolingController : MonoBehaviour
{
    static private EffectPoolingController EffectObjectController = null;


    public GameObject ExplosionModel;
    public GameObject PsychicExplosionModel;
    public GameObject LineRendererModel;
    public GameObject WarningSignModel;

    private List<GameObject> ExplosionList = new();
    private List<GameObject> PsychicExplosionList = new();
    private List<GameObject> LineRendererList = new();
    private List <GameObject> WarningSignList = new();

    static public EffectPoolingController Instance {
        get {
            return EffectObjectController;
        }
    }

    private void Awake()
    {
        if (EffectObjectController != null) {

            Destroy(this);
            return;
        }
        EffectObjectController = this;
    }

    public GameObject GetExplosion(float SoundVolume = 1f, AudioClip audioClip = null)
    {

        GameObject explosion = CallObject(ref ExplosionList, ExplosionModel);

        if (explosion.GetComponent<Explosion>() != null)
        {
            explosion.GetComponent<Explosion>().Initiation(SoundVolume, audioClip);
        }
        explosion.transform.localScale = Vector3.one;
        explosion.SetActive(true);

        return explosion;
    }

    public GameObject GetPsychicExplosion(float SoundVolume = 1f, AudioClip audioClip = null)
    {

        GameObject explosion = CallObject(ref PsychicExplosionList, PsychicExplosionModel);

        if (explosion.GetComponent<Explosion>() != null)
        {
            explosion.GetComponent<Explosion>().Initiation(SoundVolume, audioClip);
        }
        explosion.transform.localScale = Vector3.one;
        explosion.SetActive(true);

        return explosion;
    }

    public GameObject GetLineRenderer(KeyValuePair<Vector3, Vector3>? value=null)
    {

        GameObject lineRenderer = CallObject(ref LineRendererList, LineRendererModel);

        if (lineRenderer.GetComponent<LineRenderer>() != null) {

            if (value.HasValue)
            {
                lineRenderer.GetComponent<LineRenderer>().SetPosition(0, value.Value.Key);
                lineRenderer.GetComponent<LineRenderer>().SetPosition(1, value.Value.Value);
            }
            else {

                lineRenderer.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                lineRenderer.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            }
        }
        lineRenderer.SetActive(true);
        return lineRenderer;
    }

    public GameObject GetWarningSign(Vector3 position, Vector3 scale, float solidTime, float blinkTime=0, float BlinkCount=0) { 
    
        GameObject warningSign = CallObject(ref WarningSignList, WarningSignModel);

        if (warningSign.GetComponent<WarningObject>() != null) {

            warningSign.GetComponent<WarningObject>().SolidTime = solidTime;
            warningSign.GetComponent<WarningObject>().BlinkTime = blinkTime;
            warningSign.GetComponent<WarningObject>().BlinkCount = BlinkCount;
        }
        warningSign.transform.position = position;
        warningSign.transform.localScale = scale;

        return warningSign;
    }



    private GameObject CallObject(ref List<GameObject> golist, GameObject model)
    {
        GameObject gameObj = null;

        foreach (GameObject obj in golist)
        {

            if (!obj.activeSelf)
            {

                gameObj = obj;
                break;
            }
        }
        if (gameObj == null)
        {
            gameObj = Instantiate(model, gameObject.transform);
            golist.Add(gameObj);
        }

        return gameObj;

    }

    public void DestroyAllEffectObject() {

        int count = transform.childCount;
        for (int t = count-1; t >= 0; t--) { 
        
            Destroy(transform.GetChild(t).gameObject);
        }

        ExplosionList = new();
        LineRendererList = new();

    }
}
