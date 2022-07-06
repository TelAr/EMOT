using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAura : PatternDefault
{
    public List<Vector3> PatternPositionList;
    public GameObject GiantAuraSpawnerModel;
    public float PreDelay;
    public Vector3 SpanwnOffsetPosition;

    private float timer = 0;
    private int step = 0;
    private int numOfPosition = 0;
    private GameObject giantAuraSpawner = null;
    private bool animaionActive = false;

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.collider.CompareTag("Block") && step == 0)  {

            step = 1;
        }
    }

    private void getSpawnerGround(Vector3 vec) {

        animaionActive = true;
    }

    public override void Setting()
    {
        timer = 0;
        step = 0;
        animaionActive = false;
        numOfPosition = Random.Range(0, PatternPositionList.Count);
        if (giantAuraSpawner == null) {

            giantAuraSpawner = Instantiate(GiantAuraSpawnerModel);
            giantAuraSpawner.SetActive(false);
            giantAuraSpawner.GetComponent<Spawner>().SetSpawnPositionFunction = getSpawnerGround;
        }
    }

    public override void Run()
    {
        base.Run();
        float length = (gameObject.transform.position - PatternPositionList[numOfPosition]).magnitude;
        float vel = Mathf.Sqrt(GameController.GetGameController.GRAVITY * length) + 5f;
        gameObject.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(PatternPositionList[numOfPosition] - gameObject.transform.position, 
            vel, GameController.GetGameController.GRAVITY);

    }

    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            switch (step) { 
            
                case 0:
                    if (Caster.GetFall()) { 
                    
                        step = 1;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (timer > PreDelay) {

                        giantAuraSpawner.transform.position = transform.position + SpanwnOffsetPosition;
                        giantAuraSpawner.SetActive(true);


                        if (animaionActive) {

                            step = 2;
                        }
                    }
                    break;
                case 2:
                    //animation???
                    if (true) { 
                    
                        Stop();
                    }
                    break;
            }
        }
    }
}
