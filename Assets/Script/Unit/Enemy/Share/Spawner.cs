using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : UnitDefault
{

    public float Speed=50;
    public GameObject SpawnedObjectModel;
    public PatternDefault Caster;
    public bool IsCollisionTriggerObject;

    private RaycastHit2D hit;
    private bool spawnActive = false;
    private bool isHit = false;
    private GameObject spawnedObject = null;
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

    public GameObject GetSpawnedObject {

        get { 
        
            return spawnedObject;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (spawnedObject == null) {

            spawnedObject=Instantiate(SpawnedObjectModel);
            spawnedObject.SetActive(false);
        }
        if (spawnedObject.GetComponent<SpawnedObject>() != null) {
            spawnedObject.GetComponent<SpawnedObject>().Caster = Caster;
        }
        isHit = false;
        spawnActive = false;

    }



    protected override void Update() { 
    
        
        if (!isHit) {
            hit = Physics2D.Raycast(transform.position, new Vector2(0, -1),100,LayerMask.GetMask("Environment"));
            
            if (hit.collider != null)
            {
                if (IsCollisionTriggerObject)
                {
                    spawnActive = true;
                }
                isHit = true;
                SpawnPoint = hit.point;
                sp(SpawnPoint);
            }
        }
        else if (spawnActive)
        {

            spawnedObject.transform.position = SpawnPoint;
            spawnedObject.SetActive(true);

            gameObject.SetActive(false);
            return;
        }

    }

}
