using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingGrenade : PatternDefault
{

    public List<GameObject> GrenadeList = new();
    public GameObject GrenadeModel;
    public int FireTimes;
    public float FireDelay, TargettingDelay;
    public float EvasionVelocity, EvasionDistance;
    public float AngleDistance;
    public float GrenadeSpeed;
    public float SeperateAngle, SeperateHeightRatio;

    private float timer, subTimer;
    private int counter;
    private int status;
    private Rigidbody2D rb;
    private Vector3 offset, firePos, targetPos;
    private float direction;
    private bool is_seperate;
    private Queue<GameObject> firedGrenade=new();


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Setting()
    {
        timer = 0;
        subTimer = 0;
        counter = 0;
        status = -1;
        offset = Vector3.zero;
    }

    public override void Run()
    {
        
        base.Run();
        Caster.statement = "TatgettingGrenade";
        int rand = Random.Range(0, 100);
        is_seperate = (rand > 50);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (status == 0||IsRun)
        {
            rb.velocity = Vector3.zero;
            status = 1;
        }

    }


    private void FixedUpdate()
    {
        if (status == 0)
        {

            rb.velocity += new Vector2(0, GameController.GRAVITY)*Time.fixedDeltaTime;
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
            if (status == -1)
            {

                direction = gameObject.transform.position.x < GameController.GetPlayer().transform.position.x ? -1 : 1;
                rb.velocity = Ballistics.Ballistic(new Vector2(EvasionDistance * direction, 0), EvasionVelocity, GameController.GRAVITY);
                status = 0;
            }
            else if (status == 1)
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
                                grenade.SetActive(true);
                                break;
                            }
                        }
                        if (grenade == null)
                        {

                            grenade = Instantiate(GrenadeModel);
                            MissileDefault option = grenade.GetComponent<MissileDefault>();
                            option.Reset();
                            option.impact = grenade.GetComponent<ImpactDefault>();
                            grenade.AddComponent<TargettingGrenadeSeperateCase>();
                            GrenadeList.Add(grenade);
                        }

                        //분열 여부는 이후 스프라이트 따라 달라질 수 있음
                        if (is_seperate)
                        {

                            grenade.GetComponent<SpriteRenderer>().color = Color.red;
                        }
                        else { 
                        
                            grenade.GetComponent<SpriteRenderer>().color = Color.green;
                        }

                        grenade.transform.position = gameObject.transform.position + offset;
                        grenade.GetComponent<Rigidbody2D>().velocity = new Vector3(direction * -1, 5, 0) * 12f;
                        firedGrenade.Enqueue(grenade);
                        counter++;
                        subTimer = 0;
                    }
                    else
                    {
                        firePos = gameObject.transform.position + offset + new Vector3(direction * -1, 5, 0) * 4f;
                        subTimer = 0;
                        status = 2;
                    }
                }
            }
            else if (status == 2)
            {

                timer += Time.deltaTime;
                targetPos = GameController.GetPlayer().transform.position;
                //타게팅 유닛 추가
                if (timer > TargettingDelay)
                {


                    status = 3;
                }
            }
            else if (status == 3)
            {

                Vector3 targetVector = (targetPos - firePos);
                float FireAngle = Mathf.Rad2Deg * Mathf.Atan2(targetVector.y, targetVector.x);
                FireAngle -= AngleDistance * (FireTimes - 1) * 0.5f;

                while (firedGrenade.Count > 0)
                {

                    GameObject grenade = firedGrenade.Dequeue();
                    if (is_seperate)
                    {
                        
                        grenade.GetComponent<TargettingGrenadeSeperateCase>().SeperateAngel = SeperateAngle;
                        grenade.GetComponent<TargettingGrenadeSeperateCase>().SeperateHeight = firePos.y * SeperateHeightRatio + targetPos.y * (1 - SeperateHeightRatio);
                        grenade.GetComponent<TargettingGrenadeSeperateCase>().ChildGrenadeModel = grenade;
                        grenade.GetComponent<TargettingGrenadeSeperateCase>().IsActive = true;
                    }
                    else {

                        grenade.GetComponent<TargettingGrenadeSeperateCase>().IsActive = false;
                    }
                    grenade.SetActive(true);
                    grenade.transform.position = firePos;
                    grenade.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(Mathf.Deg2Rad*FireAngle), Mathf.Sin(Mathf.Deg2Rad*FireAngle), 0) * GrenadeSpeed;
                    FireAngle += AngleDistance;
                }
                status = 4;
                timer = 0;
            }
            else if (status == 4)
            {

                Stop();
            }
        }
    }
}
