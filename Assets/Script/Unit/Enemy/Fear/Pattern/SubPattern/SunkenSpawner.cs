using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkenSpawner : UnitDefault
{

    public float Speed=10;
    public GameObject SunkenModel;
    public SunkenFear Caster;

    private GameObject SunkenPrefab = null;
    private Rigidbody2D rb = null;



    protected override void OnEnable()
    {
        base.OnEnable();
        if (rb == null) {

            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = new Vector3(0,-Speed);

        if (SunkenPrefab == null) {

            SunkenPrefab=Instantiate(SunkenModel);
            SunkenPrefab.SetActive(false);
        }
        isFall = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 SpawnPoint = Vector2.zero;

        foreach (var obj in collision.contacts) {

            SpawnPoint += obj.point;
        }
        SpawnPoint/=collision.contacts.Length;

        SunkenPrefab.transform.position = SpawnPoint;
        SunkenPrefab.SetActive(true);

        gameObject.SetActive(false);
    }

    protected override void Update() { 
    
        base.Update();
        if (isFall) {

            isFall = false;
            //추락 시 예외처리
            gameObject.SetActive(false);
        }
    }

}
