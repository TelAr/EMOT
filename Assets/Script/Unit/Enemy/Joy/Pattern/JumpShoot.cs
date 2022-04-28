using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpShoot : PatternDefault
{
    public GameObject BulletModel;
    public float Bullet_speed, Jump_height;
    private List<GameObject> BulletList;
    private float timer, shoot_timer;
    private GameObject player;
    private const float JUMP_DELAY = 2f;
    private const float SHOOT_DELAY = 0.6f;
    private const float SHOOT_DISTANCE = 15; 
    private int shoot_count;
    private const int SHOOT_MAX = 3;
    private Vector2 offset, target, begin_pos;

    private void Reset()
    {
        Bullet_speed = 5;
        Jump_height = 5;
    }

    public override void Setting()
    {
        timer = 0;
        shoot_timer = 0;
        shoot_count = 0;
        offset=new Vector2(0,0);
        target=new Vector2(0,0);
    }


    public override void Run()
    {
        base.Run();
        player = GameController.GetPlayer();
        caster.GetComponent<EnemyDefault>().statement = "JumpShoot";
        begin_pos=gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        BulletList=new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction;
        Vector3 target_vector;
        if (is_run) {
            if (timer < JUMP_DELAY) {
                timer += Time.deltaTime;
                gameObject.transform.position = begin_pos + new Vector2(0, Jump_height - Mathf.Pow((timer - JUMP_DELAY) / JUMP_DELAY, 2) * Jump_height);
            }
            else {

                if (shoot_count < SHOOT_MAX) {

                    shoot_timer += Time.deltaTime;
                    if (shoot_timer > SHOOT_DELAY)
                    {

                        shoot_count++;
                        shoot_timer = 0;
                        target = player.transform.position;
                        target_vector = ((Vector3)target - (transform.position + (Vector3)offset)).normalized;
                        direction = Mathf.Rad2Deg * Mathf.Acos(target_vector.x);
                        if (target_vector.y < 0)
                        {

                            direction = 360-direction;
                        }
                        for (int i = 0; i < 3 + shoot_count % 2; i++)
                        {
                            shoot(direction - SHOOT_DISTANCE * (3 + shoot_count % 2 - 1) * 0.5f + i * SHOOT_DISTANCE);
                        }
                    }
                }
                else
                {

                    Stop();
                }
            }
        }
    }


    private void shoot(float targetting) {

        GameObject bullet = null;
        Vector3 direction;
        foreach (var item in BulletList)
        {

            if (!item.activeSelf)
            {

                bullet = item;
                break;
            }
        }
        if (bullet == null)
        {

            bullet = Instantiate(BulletModel);
            BulletList.Add(bullet);
        }
        bullet.SetActive(true);
        bullet.transform.position = transform.position + (Vector3)offset;
        direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * targetting), Mathf.Sin(Mathf.Deg2Rad * targetting), 0);
        bullet.GetComponent<Rigidbody2D>().velocity = (Vector2)direction * Bullet_speed;
    }

    private void shoot(Vector2 targetting) {

        GameObject bullet = null;
        Vector3 direction;
        foreach (var item in BulletList) {

            if (!item.activeSelf) { 
            
                bullet = item;
                break;
            }
        }
        if (bullet == null) {

            bullet = Instantiate(BulletModel);
            BulletList.Add(bullet);
        }
        bullet.SetActive(true);
        bullet.transform.position = transform.position+(Vector3)offset;
        direction = ((Vector3)targetting - (transform.position + (Vector3)offset)).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction*Bullet_speed;
    }
}
