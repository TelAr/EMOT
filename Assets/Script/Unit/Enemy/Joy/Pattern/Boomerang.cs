using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : PatternDefault
{
    
    public GameObject Boomerang_model;
    private GameObject boomerang_object;
    private float timer;
    public override void Setting()
    {
        timer = 0;
    }
    

    // Start is called before the first frame update
    void Awake()
    {
        cooldown = 5f;
        stack = 1;
        timer = 0;
        boomerang_object = Instantiate(Boomerang_model);
        boomerang_object.SetActive(false);
    }


    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
