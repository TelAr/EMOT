using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainAuraForExecute : MonoBehaviour
{
    public float RemainTime;

    private float timer;

    private void OnEnable()
    {
        timer = 0;
        gameObject.GetComponent<SpriteRenderer>().color= Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer>RemainTime*0.5f) {

            float ratio = (timer - RemainTime * 0.5f) / (RemainTime * 0.5f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (1 - ratio));
        }
        if (timer > RemainTime) {

            gameObject.SetActive(false);
        }
    }
}
