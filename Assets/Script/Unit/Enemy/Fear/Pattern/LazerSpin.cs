using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerSpin : PatternDefault
{
    public Color PreColor;
    public float PreWidth;
    public Material PreMaterial;
    public Color ActiveColor;
    public float ActiveWidth;
    public Material ActiveMaterial;
    public int LazerCount = 0;
    public int Damage = 10;
    public float ImmuneTime = 0.5f;

    private float timer = 0;
    private int step = 0;

    public override void Setting()
    {
        timer = 0;
        step = 0;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
