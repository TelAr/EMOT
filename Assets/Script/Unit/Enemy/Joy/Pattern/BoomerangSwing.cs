using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSwing : PatternDefault
{
    
    public GameObject Boomerang_model;
    public bool IsFixedDistance, IsFixedAVGVelocity;
    public float FixedDistance, FixedAVGVelocity;
    private GameObject boomerang_object;
    public float timer;
    private float targetting_time = 1f;
    private float flight_one_way_time = 2f;
    private Vector3 target_pos, offset_pos;
    public override void Setting()
    {
        timer = 0;
    }

    private void Reset()
    {
        cooldown = 7f;
        stack = 1;
        max_distance = 10;
        min_distance = 0;
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
        caster.GetComponent<EnemyDefault>().statement = "Boomerang";
        if (boomerang_object.activeSelf) {
            caster.GetComponent<EnemyDefault>().statement = "Deny pattern";
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
        if (is_run) {

            timer += Time.deltaTime;

            if (timer > targetting_time) {

                target_pos = GameController.GetPlayer().transform.position;

                if (IsFixedDistance)
                {

                    target_pos = (target_pos - (gameObject.transform.position+offset_pos)).normalized * FixedDistance + (gameObject.transform.position + offset_pos);
                }

                if (IsFixedAVGVelocity)
                {

                    flight_one_way_time = (target_pos - boomerang_object.transform.position).magnitude / FixedAVGVelocity;
                }




                if (boomerang_object.GetComponent<Boomerang>() == null)
                {

                    boomerang_object.AddComponent<Boomerang>();
                }
                Debug.Log(target_pos);
                Debug.Log(flight_one_way_time);
                boomerang_object.GetComponent<Boomerang>().Initiating(caster.gameObject, target_pos, flight_one_way_time, offset_pos);
                boomerang_object.SetActive(true);


                Stop();
            }
                

            /*
            if (timer < targetting_time)
            {

                target_pos = GameController.GetPlayer().transform.position;
                boomerang_object.transform.position = offset_pos = gameObject.transform.position;
                if (IsFixedDistance)
                {

                    target_pos = (target_pos - boomerang_object.transform.position).normalized * FixedDistance + boomerang_object.transform.position;
                }
                if (IsFixedAVGVelocity) {

                    flight_one_way_time = (target_pos - boomerang_object.transform.position).magnitude / FixedAVGVelocity;
                }
                boomerang_object.SetActive(true);
                cooldown = targetting_time + flight_one_way_time * 2 + 2f;
            }
            else if (timer < flight_one_way_time + targetting_time)
            {

                boomerang_object.transform.position = offset_pos * Mathf.Pow((timer - (targetting_time + flight_one_way_time)) / flight_one_way_time, 2) 
                    + target_pos * (1 - Mathf.Pow((timer - (targetting_time + flight_one_way_time)) / flight_one_way_time, 2));

            }
            else if (timer < flight_one_way_time * 2 + targetting_time)
            {

                boomerang_object.transform.position = target_pos * (1 - Mathf.Pow((timer - (targetting_time + flight_one_way_time)) / flight_one_way_time, 2))
                + transform.position * Mathf.Pow((timer - (targetting_time + flight_one_way_time)) / flight_one_way_time, 2);
            }
            else {

                Stop();
            }
            */
        }
        


    }


}
