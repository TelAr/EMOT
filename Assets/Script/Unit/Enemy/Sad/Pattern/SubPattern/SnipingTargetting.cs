using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipingTargetting : MonoBehaviour
{
    public GameObject ExplosionModel;
    public float TargettingTime, FixedTime;
    public AudioClip ExplosionSound;
    public GameObject A, B, C, D, E;
    public float DisapearedDelay;
    public Color effectOriginalColor;

    private float timer;
    private Vector3 effectOriginalScale;
    private bool is_fire;
    private const int FREQUENCY = 32;
    // Start is called before the first frame update
    void Awake()
    {
        effectOriginalScale = E.transform.localScale;
    }
    public void Initiate(float tt, float ft) {

        timer = 0;
        TargettingTime = tt;
        FixedTime = ft;
        
        E.transform.localScale = effectOriginalScale;
        E.GetComponent<SpriteRenderer>().color = effectOriginalColor - new Color(0, 0, 0, 1);
        is_fire = false;
        A.transform.localScale = B.transform.localScale = C.transform.localScale = D.transform.localScale = E.transform.localScale = Vector3.zero;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) {
            if (is_fire)
            {

                timer -= Time.deltaTime * 2;
            }
            else {

                timer += Time.deltaTime;
            }
        }
        if (timer < TargettingTime)
        {
            if (!is_fire) transform.position = GameController.GetPlayer().transform.position;

            //A
            A.transform.localScale = Vector3.one * (1 - Mathf.Pow(1 - timer / TargettingTime, 2));
            A.GetComponent<SpriteRenderer>().color = effectOriginalColor - new Color(0, 0, 0, 1 - timer / TargettingTime);
            if (!is_fire) {
                A.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1) * ((int)Mathf.Floor(timer * FREQUENCY / TargettingTime) % 2);
            }
            //B
            B.transform.localScale = Vector3.one * (1 - Mathf.Pow(1 - timer / TargettingTime, 2));
            B.GetComponent<SpriteRenderer>().color = effectOriginalColor;
            //C
            C.transform.localScale = Vector3.one * (1 - Mathf.Pow(1 - timer / TargettingTime, 2));
            C.GetComponent<SpriteRenderer>().color = effectOriginalColor - new Color(0, 0, 0, 1-timer / TargettingTime);
            C.transform.rotation = Quaternion.Euler(0,0,-180* Mathf.Pow(1 - timer / TargettingTime, 2));
            if (!is_fire)
            {
                C.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1) * ((int)Mathf.Floor(timer * FREQUENCY / TargettingTime) % 2);
            }
            //D
            D.transform.localScale = Vector3.one * (1 - Mathf.Pow(1 - timer / TargettingTime, 2));
            D.GetComponent<SpriteRenderer>().color = effectOriginalColor;
        }
        else if (timer < TargettingTime + FixedTime)
        {
            A.GetComponent<SpriteRenderer>().color = B.GetComponent<SpriteRenderer>().color = C.GetComponent<SpriteRenderer>().color = effectOriginalColor;

            E.transform.localScale = ((TargettingTime + FixedTime - timer) / FixedTime) * effectOriginalScale;
            E.GetComponent<SpriteRenderer>().color = new Color(effectOriginalColor.r, effectOriginalColor.g, effectOriginalColor.b, (timer - (TargettingTime)) / (FixedTime));
        }
        else if(!is_fire){
            GameObject go;
            go = EffectPoolingController.EffectObjectController.GetComponent<EffectPoolingController>().GetExplosion(ExplosionSound);
            go.transform.position = transform.position;
            go.transform.localScale *= 9;
            is_fire = true;
            timer = TargettingTime;
        }

        if (is_fire && timer < 0) {

            gameObject.SetActive(false);
        }
    }
}
