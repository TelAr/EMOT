using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolingController : MonoBehaviour
{
    static public GameObject EffectObjectController=null;


    public GameObject ExplosionModel;

    private List<GameObject> ExplosionList = new List<GameObject>();
    private void Awake()
    {
        EffectObjectController = gameObject;
    }


    public GameObject GetExplosion(AudioClip audioClip = null)
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
            explosion.GetComponent<Explosion>().Initiation(audioClip);
        }
        explosion.transform.localScale = Vector3.one;
        explosion.SetActive(true);

        return explosion;
    }
}
