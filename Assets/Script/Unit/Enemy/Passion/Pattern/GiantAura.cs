using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAura : PatternDefault
{
    public List<Vector3> PatternPositionList;
    public GameObject GiantAuraSpawnerModel;
    public float PreDelay;
    public float AuraBeginVel, AuraAccelVel, AuraAccelDelay;
    public Vector3 SpanwnOffsetPosition;

    private float timer = 0;
    private int step = 0;
    private int numOfPosition = 0;
    private GameObject giantAuraSpawner = null;
    private bool animationActive = false;

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.collider.CompareTag("Block") && step == 0)  {

            step = 1;
        }
    }

    private void getSpawnerGround(Vector3 vec) {

        animationActive = true;
    }

    public override void Setting()
    {
        timer = 0;
        step = 0;
        animationActive = false;
        numOfPosition++;
        if (numOfPosition >= PatternPositionList.Count) { 
        
            numOfPosition = 0;
        }
        if (giantAuraSpawner == null) {

            giantAuraSpawner = Instantiate(GiantAuraSpawnerModel);
            giantAuraSpawner.GetComponent<Spawner>().Caster = this;
            giantAuraSpawner.GetComponent<Spawner>().SetSpawnPositionFunction = getSpawnerGround;
            giantAuraSpawner.SetActive(false);
        }
    }

    public override void Run()
    {
        if (giantAuraSpawner.GetComponent<Spawner>().GetSpawnedObject != null &&
            giantAuraSpawner.GetComponent<Spawner>().GetSpawnedObject.activeSelf) {

            return;
        }

        base.Run();
        float length = (gameObject.transform.position - PatternPositionList[numOfPosition]).magnitude;
        float vel = Mathf.Sqrt(Mathf.Abs(GameController.GetGameController.GRAVITY * length)) + 5f;

        gameObject.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(PatternPositionList[numOfPosition] - gameObject.transform.position, 
            vel, GameController.GetGameController.GRAVITY);

    }

    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            switch (step) { 
            
                case 0:
                    if (!Caster.GetFall()) {

                        Debug.Log("go to step1");
                        step = 1;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (timer > PreDelay) {

                        Vector3 direction;

                        direction = new Vector3(
                            gameObject.transform.position.x < GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos.x ? 1 : -1, 0);

                        giantAuraSpawner.transform.position = transform.position + SpanwnOffsetPosition;
                        giantAuraSpawner.SetActive(true);
                        giantAuraSpawner.GetComponent<Spawner>().GetSpawnedObject.
                            GetComponent<Aura>().Init(AuraBeginVel, AuraAccelVel, AuraAccelDelay, direction);
                        giantAuraSpawner.GetComponent<Spawner>().GetSpawnedObject.transform.localScale
                            = new Vector3(-direction.x, 1);
                        giantAuraSpawner.GetComponent<Spawner>().SetSpawnActive = true;

                        if (animationActive) {
                            Debug.Log("go to step2");
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

    private void FixedUpdate()
    {
        if (IsRun) {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, GameController.GetGameController.GRAVITY * Time.fixedDeltaTime);
        }
            
    }
}
