using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : PatternDefault
{

    public List<GameObject> GrenadeList = new();
    public GameObject GrenadeModel;
    public float BackstepDistance, BackstepTime;
    public int FireTimes;
    public float FireDelay;
    public float AngleRange;
    public float DistanceRange;
    public float FlyingTime, BoomTime;
    public Vector3 TargettingOffset;
    public float SoundVolumeOffset = 0.5f;

    private float timer, subTimer;
    private bool startMoving;
    private int counter;
    private int status;
    private Rigidbody2D rb;
    private Vector3 offset;
    private float direction;
    private Queue<GameObject> firedGrenade = new();


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Setting()
    {
        timer = 0;
        subTimer = 0;
        counter = 0;
        status = 0;
        offset = Vector3.zero;
    }

    public override void Run()
    {

        base.Run();
        Caster.Statement = "RapidFire";
        startMoving = false;
    }


    private void FixedUpdate()
    {
        if (status == 0 && IsRun)
        {
            timer += Time.fixedDeltaTime;

            if (!startMoving)
            {

                direction = gameObject.transform.position.x < GameController.GetPlayer().transform.position.x ? -1 : 1;
                startMoving = true;
            }
            rb.velocity = (new Vector2((float)direction, 0) * BackstepDistance / BackstepTime) * 2 * (BackstepTime - timer) / BackstepTime;


            if (BackstepTime < timer) {

                status = 1;
                rb.velocity = Vector2.zero;
            }

        }
    }


    private void Update()
    {

        if (IsRun)
        {
            if (Caster.GetFall())
            {
                Debug.Log("falling");
                Stop();
                return;
            }

            if (status == 0|| status == 1)
            {
                subTimer += Time.deltaTime;

                if (subTimer >= FireDelay)
                {

                    if (counter < FireTimes)
                    {

                        GameObject grenade = null;
                        foreach (var g in GrenadeList)
                        {

                            if (!g.activeSelf)
                            {

                                grenade = g;
                                break;
                            }
                        }
                        if (grenade == null)
                        {

                            grenade = Instantiate(GrenadeModel);
                            grenade.GetComponent<MissileDefault>().enabled = false;
                            grenade.GetComponent<Collider2D>().isTrigger = true;
                            grenade.AddComponent<RapidFireBullet>();
                            GrenadeList.Add(grenade);
                            grenade.SetActive(false);
                        }
                        grenade.transform.position = transform.position;
                        grenade.GetComponent<RapidFireBullet>().FlyTimer = FlyingTime;
                        grenade.GetComponent<RapidFireBullet>().BoomTimer = BoomTime;
                        grenade.GetComponent<RapidFireBullet>().Volume = SoundVolumeOffset;
                        Vector3 targettingVector = (GameController.GetPlayer().transform.position+ TargettingOffset) - transform.position;
                        float rotation = Mathf.Rad2Deg*Mathf.Atan2(targettingVector.y, targettingVector.x);
                        rotation += Random.Range(-AngleRange, AngleRange);
                        Vector3 targgettingPos = transform.position + new Vector3((float)Mathf.Cos(Mathf.Deg2Rad * rotation), (float)Mathf.Sin(Mathf.Deg2Rad * rotation), 0) 
                            * (targettingVector.magnitude + Random.Range(-DistanceRange, DistanceRange));
                        grenade.GetComponent<RapidFireBullet>().EndPos = targgettingPos;
                        grenade.SetActive(true);
                        GetComponent<SadAudio>().GrenadeFirePlay(0.5f);
                        counter++;
                    }
                    else
                    {
                        if (status == 1) {
                            status = 2;
                        }
                    }
                    subTimer = 0;
                }
            }
            else if (status == 2)
            {

                Stop();

            }
        }
    }
}
