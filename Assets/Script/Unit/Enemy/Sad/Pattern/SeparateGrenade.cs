using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateGrenade : PatternDefault
{
    [Header("* SeparateGrenade Pattern Value")]
    public float PreDelay;
    public float ThrowPower;
    public GameObject GrenadeModel;



    private GameObject GrenadeInstance = null;
    private Vector2 offset;
    private float timer;
    public override void Setting()
    {
        offset = Vector2.zero;
        if (GrenadeInstance == null)
        {
            GrenadeInstance = Instantiate(GrenadeModel);
            GrenadeInstance.SetActive(false);
        }
        GrenadeInstance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        timer = 0;
    }

    public override void Run()
    {

        base.Run();
        Caster.GetComponent<EnemyDefault>().Statement = "SeparateGrenade";
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GrenadeInstance == null)
        {
            GrenadeInstance = Instantiate(GrenadeModel);
            GrenadeInstance.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsRun)
        {
            timer += Time.fixedDeltaTime;
            if (timer > PreDelay)
            {
                if(GetComponent<SadAudio>()!=null) GetComponent<SadAudio>().SwingPlay();
                GrenadeInstance.transform.position = (Vector3)offset + transform.position;
                GrenadeInstance.SetActive(true);
                GrenadeInstance.GetComponent<Rigidbody2D>().velocity 
                    = Ballistics.Ballistic(GameController.GetPlayer.transform.position - ((Vector3)offset + transform.position), 
                                            ThrowPower, GameController.GetGameController().GRAVITY);
                GrenadeInstance.GetComponent<Rigidbody2D>().angularVelocity = 360f;
                GetComponent<SadAudio>().GrenadeFirePlay();
                Stop();
            }
        }
    }
}
