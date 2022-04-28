using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeArea : MonoBehaviour
{

    public float AreaTime = 3f;
    public float FadeOut = 1f;
    public GameObject SmokeModel;
    static private GameObject Smoke = null;
    private Color color;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (Smoke == null)
            {

                Smoke = Instantiate(SmokeModel, collision.gameObject.transform);
            }
            Smoke.GetComponent<SpriteRenderer>().color = Color.white;
            Smoke.GetComponent<Smoke>().ResetTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer > AreaTime) {

            gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, (AreaTime+FadeOut-timer)/FadeOut);
            if (timer > AreaTime + FadeOut) { 
            
                Destroy(gameObject);
            }
        }
    }
}
