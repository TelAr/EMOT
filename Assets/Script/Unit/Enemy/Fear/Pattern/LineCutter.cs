using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCutter : PatternDefault
{
    public int LineNumber;
    public float DarkTime;
    public Vector3 MiddlePoint;
    public float Radius;

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

    // Update is called once per frame
    void Update()
    {
        if (IsRun) { 
        

        }
    }
}
