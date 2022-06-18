using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSwing : PatternDefault
{
    [Header("* BoomerangSwing Pattern Value")]
    public GameObject Boomerang_model;
    public bool IsFixedDistance, IsFixedAVGVelocity;
    public float FixedDistance, FixedAVGVelocity;
    private Vector3 CasterOffsetPos, TargetOffsetPos;
    private GameObject boomerang_object;
    private float timer;
    private float targetting_time = 1f;
    private float flight_one_way_time = 2f;
    private Vector3 target_pos;
    public override void Setting()
    {
        timer = 0;
    }

    private void Reset()
    {
        Cooldown = 7f;
        Stack = 1;
        MaxDistance = 10;
        MinDistance = 0;
        timer = 0;
        IsFixedAVGVelocity = false;
        IsFixedDistance = false;
        FixedAVGVelocity = 5;
        FixedDistance = 10;
    }


    // Start is called before the first frame update
    void Awake()
    {
        boomerang_object = Instantiate(Boomerang_model);
        boomerang_object.SetActive(false);
        
    }

    public override void Run()
    {
        base.Run();
        Caster.GetComponent<EnemyDefault>().Statement = "Boomerang";
        if (boomerang_object.activeSelf) {
            Caster.GetComponent<EnemyDefault>().Statement = "Deny pattern";
            Stop();
            return;
        }


    }

    public override void Stop()
    {
        base.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Caster.GetFall())
        {

            boomerang_object.SetActive(false);
            Stop();
        }

        if (IsRun) {

            timer += Time.deltaTime;

            if (timer > targetting_time) {

                target_pos = GameController.GetPlayer.transform.position + TargetOffsetPos;

                if (IsFixedDistance)
                {

                    target_pos = (target_pos - (gameObject.transform.position+CasterOffsetPos)).normalized * FixedDistance + (gameObject.transform.position + CasterOffsetPos);
                }

                if (IsFixedAVGVelocity)
                {

                    flight_one_way_time = (target_pos - gameObject.transform.position).magnitude / FixedAVGVelocity;
                }




                if (boomerang_object.GetComponent<Boomerang>() == null)
                {
                    boomerang_object.AddComponent<Boomerang>();
                }
                boomerang_object.GetComponent<Boomerang>().Initiating(Caster.gameObject, target_pos, flight_one_way_time, CasterOffsetPos);
                boomerang_object.SetActive(true);


                Stop();
            }
        }
        


    }


}
