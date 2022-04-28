using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    float timer;
    public float DefaultTime, FadeOutTime;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < DefaultTime)
        {

            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        else if (timer <= DefaultTime + FadeOutTime) {

            gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, color.a * (DefaultTime + FadeOutTime - timer) / FadeOutTime);
        }
    }

    public void ResetTimer() {

        timer = 0;
    }
}
