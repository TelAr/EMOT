using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public GameObject BulletModel;
    private List<GameObject> bullets = new List<GameObject>();
    public float BulletSpeed = 20f;
    public int BulletMax = 7;
    private int bulletAmount = 0;
    public float BulletReloadDelay = 3f;
    private float reloadTimer = 0;
    public Vector3 OffsetPosition;


    public float ParryingJudgeTime = 0.1f;
    public float ParryingImmuneTime = 0.5f;
    private float parryingJudgeTimer = 0f;

    private PlayerPhysical pp;
    private PlayerHealth ph;
    private PlayerAudio pa;
    void Awake()
    {
        pp = GetComponent<PlayerPhysical>();
        ph = GetComponent<PlayerHealth>();
        pa = GetComponent<PlayerAudio>();
        bulletAmount = BulletMax;
        for (int t = 0; t < BulletMax; t++)
        {

            GameObject bullet = Instantiate(BulletModel);
            bullet.SetActive(false);
            bullets.Add(bullet);

        }
    }

    public bool IsParrying() {

        if (parryingJudgeTimer > 0) {

            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnFire() {

        GameObject fireBullet = null;

        if (bulletAmount <= 0) { 
        
            //불발 음원 재생
            return;
        }


        foreach (GameObject bullet in bullets) {

            if (!bullet.activeSelf) {

                fireBullet = bullet;
                break;
            }
        }
        if (fireBullet == null) { 
        
            fireBullet=Instantiate(BulletModel);
            bullets.Add(fireBullet);
        }
        fireBullet.SetActive(true);
        fireBullet.transform.position = gameObject.transform.position + OffsetPosition;
        fireBullet.GetComponent<Rigidbody2D>().velocity=new Vector2(BulletSpeed*pp.GetDirection(),0);
        bulletAmount--;
        pa.FirePlay();
    }

    public void OnJump(InputValue value)
    {

        pp.isJump = value.Get<float>() > 0;
    }

    public void OnMove(InputValue value)
    {

        float moving = value.Get<Vector2>().x;
        if (moving < 0)
        {

            moving = -1;
        }
        if (moving > 0)
        {

            moving = 1;
        }

        pp.Moving(moving);
    }

    public void OnParrying() {

        parryingJudgeTimer = ParryingImmuneTime;
    }



    // Update is called once per frame
    void Update()
    {
        if (bulletAmount <= 0)
        {
            reloadTimer+=Time.deltaTime;

            if (reloadTimer > BulletReloadDelay) {

                pa.ReloadPlay();
                bulletAmount = BulletMax;
                reloadTimer = 0;
            }
        }

    }

    private void FixedUpdate()
    {
        if (parryingJudgeTimer > 0)
        {
            parryingJudgeTimer -= Time.fixedDeltaTime;
        }
    }
}
