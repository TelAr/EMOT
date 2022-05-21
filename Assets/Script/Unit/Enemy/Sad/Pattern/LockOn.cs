using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : PatternDefault
{
    public int HowMuch;
    public float StartDelay;
    public float TargettingDelay, TargettingFixedDelay, NextShootDelay;
    public GameObject SnipingModel;
    public bool isTargetting;
    public AudioClip ExplosionSound;

    private int counter;
    public float timer;
    private List<GameObject> snipingList;
    private Vector3 startPos;
    private const float jumpScale = 20f;
    public int status;
    public override void Setting()
    {
        counter = 0;
        timer = 0;
        status = 0;
        isTargetting = false;
    }
    private void Reset()
    {
        cooldown = 25f;
        stack = 1;
        max_distance = 10000;
        min_distance = 0;
        counter = 0;
        timer = 0;
        HowMuch = 3;
        StartDelay = 2f;
        TargettingDelay = 2f;
        TargettingFixedDelay = 1f;
        NextShootDelay = 2f;
        post_delay = 2f;

    }
    void Awake()
    {
        snipingList= new List<GameObject>();

    }

    public override void Run()
    {
        base.Run();
        caster.GetComponent<EnemyDefault>().statement = "LockOn";

        for (int i = snipingList.Count; i > HowMuch; i++) { 
        
            GameObject go = Instantiate(SnipingModel);
            go.SetActive(false);
            snipingList.Add(go);
        }
        startPos = gameObject.transform.position;
        status = 0;
        isTargetting = false;
    }

    public override void Stop()
    {
        base.Stop();

    }

    public void SetTargetting(bool value) {

        isTargetting = value;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (is_run) {

            timer += Time.deltaTime;
            if (status == 0)
            {

                if (timer < StartDelay)
                {

                    gameObject.transform.position = startPos + new Vector3(0, jumpScale) * (1 - Mathf.Pow((timer / StartDelay) - 1, 2));
                }
                else
                {

                    //환경 매체 연출 시작
                    isTargetting = true;//임시


                    if (isTargetting) {
                        status = 1;
                        timer = NextShootDelay;
                    }
                }
            }
            else if (status == 1)
            {

                if (counter < HowMuch)
                {
                    if (timer > NextShootDelay)
                    {

                        GameObject targetting = null;
                        foreach (GameObject go in snipingList)
                        {

                            if (!go.activeSelf)
                            {

                                targetting = go;
                                break;
                            }
                        }
                        if (targetting == null)
                        {

                            targetting = Instantiate(SnipingModel);
                            snipingList.Add(targetting);
                        }
                        targetting.transform.position = GameController.GetPlayer().transform.position;
                        targetting.SetActive(true);
                        targetting.GetComponent<SnipingTargetting>().Initiate(TargettingDelay, TargettingFixedDelay);
                        timer = 0;
                        counter++;
                    }
                }
                else
                {

                    status = 2;
                    timer = 0;
                }

            }
            else {
                //환경 매체에 마지막 연출 시작 메세지 보내기
                if (counter >= HowMuch) { 
                
                    counter = 0;
                }
                isTargetting = false;//임시


                if (!isTargetting) { 
                
                    Stop();
                }
            }
            
        }
    }
}
