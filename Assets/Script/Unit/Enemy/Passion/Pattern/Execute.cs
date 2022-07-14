using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execute : PatternDefault
{
    public float Predelay;
    public Vector3 JumpOffset;
    public Vector3 DownOffset;
    public float SlashDelay;
    public GameObject SlashObjectModel, WarningSignObjectModel;

    private float timer = 0, fixedTimer = 0;
    private float step = 0;
    private GameObject slashObject = null, warningSignObject = null;


    public override void Setting()
    {
        timer = 0;
        fixedTimer = 0;
        step = 0;

        if (slashObject != null) { 
        
            slashObject = Instantiate(SlashObjectModel);
            slashObject.SetActive(false);
        }

        if (warningSignObject != null) { 
        
            warningSignObject = Instantiate(WarningSignObjectModel);
            warningSignObject.SetActive(false);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
