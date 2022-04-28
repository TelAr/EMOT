using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeShell : PatternDefault
{
    public float PreDelay;
    public float ThrowPower;
    public GameObject SmokeshellModel;



    private GameObject smokeshellInstance = null;
    private Vector2 offset;
    private float timer;
    public override void Setting()
    {
        offset = Vector2.zero;
        if (smokeshellInstance == null) {
            smokeshellInstance = Instantiate(SmokeshellModel);
            smokeshellInstance.SetActive(false);
        }
        smokeshellInstance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        timer = 0;
    }

    public override void Run() { 
    
        base.Run();
        caster.GetComponent<EnemyDefault>().statement = "SmokeShell";
    }

    // Start is called before the first frame update
    void Start()
    {
        if (smokeshellInstance == null)
        {
            smokeshellInstance = Instantiate(SmokeshellModel);
            smokeshellInstance.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (is_run) {
            timer += Time.fixedDeltaTime;
            if (timer > PreDelay) {
                smokeshellInstance.transform.position = (Vector3)offset+transform.position;
                smokeshellInstance.SetActive(true);
                smokeshellInstance.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(GameController.GetPlayer().transform.position-((Vector3)offset+transform.position), ThrowPower, GameController.GRAVITY);
                smokeshellInstance.GetComponent<Rigidbody2D>().angularVelocity = 360f;
                Stop();
            }
        }
    }
}
