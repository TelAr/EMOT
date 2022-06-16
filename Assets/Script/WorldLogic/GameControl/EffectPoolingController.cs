using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolingController : MonoBehaviour
{
    static private EffectPoolingController EffectObjectController = null;


    public GameObject ExplosionModel;
    public GameObject LineRendererModel;

    private List<GameObject> ExplosionList = new List<GameObject>();
    private List<GameObject> LineRendererList = new List<GameObject>();

    static public EffectPoolingController Instance() {

        return EffectObjectController;
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

        GameObject explosion = null;
        foreach (GameObject obj in ExplosionList)
        {

            if (!obj.activeSelf)
            {

                explosion = obj;
                break;
            }
        }
        if (explosion == null)
        {
            explosion = Instantiate(ExplosionModel,gameObject.transform);
            ExplosionList.Add(explosion);
        }


        if (explosion.GetComponent<Explosion>() != null) {
            explosion.GetComponent<Explosion>().Initiation(SoundVolume, audioClip);
        }
        explosion.transform.localScale = Vector3.one;
        explosion.SetActive(true);

        return explosion;
    }


    public GameObject GetLineRenderer(KeyValuePair<Vector3, Vector3> value)
    {

        GameObject lineRenderer = null;
        foreach (GameObject obj in LineRendererList)
        {

            if (!obj.activeSelf)
            {

                lineRenderer = obj;
                break;
            }
        }
        if (lineRenderer == null)
        {
            lineRenderer = Instantiate(LineRendererModel, gameObject.transform);
            LineRendererList.Add(lineRenderer);
        }

        if (lineRenderer.GetComponent<LineRenderer>() != null) {

            lineRenderer.GetComponent<LineRenderer>().SetPosition(0, value.Key);
            lineRenderer.GetComponent<LineRenderer>().SetPosition(1, value.Value);
        }

        lineRenderer.SetActive(true);

        return lineRenderer;
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
