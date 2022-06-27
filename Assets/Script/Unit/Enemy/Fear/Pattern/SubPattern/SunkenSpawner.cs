using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkenSpawner : UnitDefault
{

    public float Speed=10;
    public GameObject SunkenModel;
    public SunkenFear Caster;

    private GameObject SunkenObject = null;
    private Rigidbody2D rb = null;
    private Vector2 SpawnPoint;


    protected override void OnEnable()
    {
        base.OnEnable();
        if (rb == null) {

            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = new Vector3(0,-Speed);

        if (SunkenObject == null) {

            SunkenObject=Instantiate(SunkenModel);
            SunkenObject.GetComponent<SunkenFearObject>().Caster = Caster;
            SunkenObject.SetActive(false);
        }
        isFall = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnPoint = Vector2.zero;

        foreach (var obj in collision.contacts) {

            SpawnPoint += obj.point;
        }
        SpawnPoint /= collision.contacts.Length;

        Caster.EyebeamCall(SpawnPoint);


    }

    protected override void Update() { 
    
        base.Update();
        if (isFall) {

            isFall = false;
            Caster.Stop();
            gameObject.SetActive(false);
        }

        if (Caster.GetStep == 3) {

            SunkenObject.transform.position = SpawnPoint;
            SunkenObject.SetActive(true);

            gameObject.SetActive(false);
            return;
        }
    }

}
