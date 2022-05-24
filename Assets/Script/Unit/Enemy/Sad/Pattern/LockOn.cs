using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : PatternDefault
{
    public int HowMuch;
    public float StartDelay;
    public float TargettingDelay, TargettingFixedDelay, NextShootDelay;
    public GameObject SnipingModel;
    public GameObject HUDModel;
    public bool isTargetting;
    public Color HUDColor;

    private int counter;
    public float timer;
    private List<GameObject> snipingList;
    private GameObject HUD = null;
    private Vector3 startPos;
    private const float jumpScale = 20f;
    public int status;
    public override void Setting()
    {
        counter = 0;
        timer = 0;
        status = 0;
        isTargetting = false;
        if (HUD == null) {

            HUD=Instantiate(HUDModel);
            HUD.SetActive(false);
        }
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
                        timer = 0;
                        HUD.GetComponent<HUD>().Initiate();
                        HUD.GetComponent<HUD>().mainColor = HUDColor;
                        HUD.GetComponent<HUD>().TransitionDelay = TargettingDelay;
                        HUD.SetActive(true);
                    }
                }
            }
            else if (status == 1)
            {

                if (counter <= HowMuch)
                {
                    if (timer > NextShootDelay)
                    {
                        if (counter < HowMuch)
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
                            targetting.GetComponent<SnipingTargetting>().Initiate(TargettingDelay, TargettingFixedDelay);
                            targetting.GetComponent<SnipingTargetting>().effectOriginalColor = HUDColor;
                            targetting.SetActive(true);

                        }
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
                timer += Time.deltaTime;
                if (timer >= TargettingDelay+TargettingFixedDelay) {

                    if (!HUD.GetComponent<HUD>().Is_end) HUD.GetComponent<HUD>().Is_end = true;
                }

                if (!HUD.activeSelf) isTargetting = false;


                if (!isTargetting) { 
                
                    Stop();
                }
            }
            
        }
    }
}
