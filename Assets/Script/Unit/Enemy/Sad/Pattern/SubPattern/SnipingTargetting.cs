using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipingTargetting : MonoBehaviour
{
    public GameObject ExplosionModel;
    public float TargettingTime, FixedTime;
    public AudioClip ExplosionSound;

    private float timer;
    private GameObject effect=null;
    private Vector3 effectOriginalScale;
    private Color effectOriginalColor;
    // Start is called before the first frame update
    void Start()
    {

        
    }
    public void Initiate(float tt, float ft) {

        timer = 0;
        TargettingTime = tt;
        FixedTime = ft;
        if (effect == null) {
            effect = gameObject.transform.GetChild(0).gameObject;
            effectOriginalScale = effect.transform.localScale;
            effectOriginalColor = effect.GetComponent<SpriteRenderer>().color;
        }
        effect.transform.localScale = effectOriginalScale;
        effect.GetComponent<SpriteRenderer>().color = effectOriginalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) {
            timer += Time.deltaTime;
        }
        if (timer < TargettingTime)
        {

            transform.position = GameController.GetPlayer().transform.position;
        }
        else if (timer < TargettingTime + FixedTime)
        {
            effect.transform.localScale = ((TargettingTime + FixedTime - timer) / FixedTime) * effectOriginalScale;
            effect.GetComponent<SpriteRenderer>().color = new Color(effectOriginalColor.r, effectOriginalColor.g, effectOriginalColor.b, (timer - (TargettingTime)) / (FixedTime));
        }
        else {
            GameObject go;
            go = Instantiate(ExplosionModel);
            if (go.GetComponent<AudioSource>() != null) {
                go.GetComponent<AudioSource>().clip = ExplosionSound;
                go.GetComponent<AudioSource>().Play();
            }
            go.transform.position = transform.position;
            go.transform.localScale *= 2;
            gameObject.SetActive(false);
        }
    }
}
