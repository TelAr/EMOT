using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCutter : PatternDefault
{
    public int LineNumber;
    public float DarkTime;
    public Vector3 MiddlePoint;
    public float Radius;
    public int Damage = 20;

    private int lineCounter = 0;
    private float timer = 0;
    private int step = 0;
    private List<GameObject> lines = new List<GameObject>();
    public override void Setting()
    {
        lineCounter = 0;
        timer = 0;
        step = 0;
    }

    private GameObject callLine(Vector3 sp, Vector3 ep) { 
    
        GameObject line;
        KeyValuePair<Vector3, Vector3> pair = new KeyValuePair<Vector3, Vector3>(sp, ep);
        line = EffectPoolingController.Instance.GetLineRenderer(pair);
        if (line.GetComponent<EdgeCollider2D>() == null)
        {

            line.AddComponent<EdgeCollider2D>();
        }
        else if (!line.GetComponent<EdgeCollider2D>().isActiveAndEnabled) {

            line.GetComponent<EdgeCollider2D>().enabled = true;
        }
        line.GetComponent<EdgeCollider2D>().isTrigger = true;
        line.GetComponent<EdgeCollider2D>().points= new Vector2[]{sp, ep};


        if (line.GetComponent<Damage>() == null)
        {
            line.AddComponent<Damage>();
        }
        else if (!line.GetComponent<Damage>().isActiveAndEnabled)
        {
            line.GetComponent<Damage>().enabled = true;
        }
        line.GetComponent<Damage>().DamageValue = Damage;

        return line;
    }


    // Update is called once per frame
    void Update()
    {
        if (IsRun) { 
        

        }
    }
}
