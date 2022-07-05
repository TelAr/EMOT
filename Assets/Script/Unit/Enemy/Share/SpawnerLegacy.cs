using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Obsolete]
public class SpawnerLegacy : UnitDefault
{
    public float Speed=50;
    public GameObject SpawnedObjectModel;
    public PatternDefault Caster;
    public bool IsCollisionTriggerObject;

    private bool spawnActive = false;
    private GameObject spawnedObject = null;
    private Rigidbody2D rb = null;
    private Vector2 SpawnPoint;

    //function that need to call which need infomation of spawned position
    public delegate void setSpowndPosition(Vector3 target);
    private setSpowndPosition sp = null;
    public setSpowndPosition SetSpawnPositionFunction
    {
        set { 
        
            sp = value;
        }
    }

    public bool SetSpawnActive
    {
       
        set { 
        
            spawnActive = value;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (rb == null) {

            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = new Vector3(0,-Speed);
        

        if (spawnedObject == null) {

            spawnedObject=Instantiate(SpawnedObjectModel);
            spawnedObject.GetComponent<SpawnedObject>().Caster = Caster;
            spawnedObject.SetActive(false);
        }

        spawnActive = false;
        isFall = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnPoint = Vector2.zero;

        foreach (var obj in collision.contacts) {

            SpawnPoint += obj.point;
        }
        SpawnPoint /= collision.contacts.Length;

        if (IsCollisionTriggerObject) {

            spawnActive = true;
        }

        if (sp != null) {
            sp(SpawnPoint);
        }

    }


    protected override void Update() { 
    
        base.Update();
        if (isFall) {

            isFall = false;
            Caster.Stop();
            gameObject.SetActive(false);
        }

        if (spawnActive) {

            spawnedObject.transform.position = SpawnPoint;
            spawnedObject.SetActive(true);

            gameObject.SetActive(false);
            return;
        }
    }

}
