using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkenFear : PatternDefault
{

    public GameObject SpawnerModel;
    public Vector3 SpawnOffset;

    private GameObject spawner=null;

    public override void Setting()
    {

        if (spawner == null) { 
        
            spawner = Instantiate(SpawnerModel);
            spawner.SetActive(false);
        }
    }

    public override void Run()
    {
        base.Run();
        spawner.transform.position = GameController.GetPlayer.transform.position + SpawnOffset;
        spawner.SetActive(true);
        Stop();
    }

}
