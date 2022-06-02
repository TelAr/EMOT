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

    private PlayerPhysical pp;

    public void OnFire() {

        GameObject fireBullet = null;

        if (bulletAmount <= 0) { 
        
            //�ҹ� ���� ���
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

        Debug.Log("OnParrying");
    }


    // Start is called before the first frame update
    void Awake()
    {
        pp=GetComponent<PlayerPhysical>();
        bulletAmount = BulletMax;
        for (int t = 0; t < BulletMax; t++) {

            GameObject bullet = Instantiate(BulletModel);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (bulletAmount <= 0)
        {
            reloadTimer+=Time.deltaTime;

            if (reloadTimer > BulletReloadDelay) {

                //������ ����
                bulletAmount = BulletMax;
                reloadTimer = 0;
            }

        }
        


    }
}
