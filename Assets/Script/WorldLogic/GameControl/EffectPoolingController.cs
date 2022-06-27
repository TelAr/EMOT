using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolingController : MonoBehaviour
{
    static private EffectPoolingController EffectObjectController = null;


    public GameObject ExplosionModel;
    public GameObject PsychicExplosionModel;
    public GameObject LineRendererModel;

    private List<GameObject> ExplosionList = new List<GameObject>();
    private List<GameObject> PsychicExplosionList = new List<GameObject>();
    private List<GameObject> LineRendererList = new List<GameObject>();

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


    public GameObject GetPsychicExplosion(float SoundVolume = 1f, AudioClip audioClip = null)
    {

        GameObject explosion = null;
        foreach (GameObject obj in PsychicExplosionList)
        {

            if (!obj.activeSelf)
            {

                explosion = obj;
                break;
            }
        }
        if (explosion == null)
        {
            explosion = Instantiate(PsychicExplosionModel, gameObject.transform);
            PsychicExplosionList.Add(explosion);
        }


        if (explosion.GetComponent<Explosion>() != null) {
            explosion.GetComponent<Explosion>().Initiation(SoundVolume, audioClip);
        }
        explosion.transform.localScale = Vector3.one;
        explosion.SetActive(true);

        return explosion;
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
            explosion = Instantiate(ExplosionModel, gameObject.transform);
            ExplosionList.Add(explosion);
        }


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




    public void DestroyAllEffectObject() {

        int count = transform.childCount;
        for (int t = count-1; t >= 0; t--) { 
        
            Destroy(transform.GetChild(t).gameObject);
        }

        ExplosionList = new();
        LineRendererList = new();

    }
}
