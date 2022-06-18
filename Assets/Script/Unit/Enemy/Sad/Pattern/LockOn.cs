using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : PatternDefault
{
    [Header("* LockOn Pattern Value")]
    public int HowMuch;
    public float StartDelay;
    [Tooltip("Delay which is targetting player")]
    public float TargettingDelay;
    [Tooltip("Delay which already targgetting, and wait for explosion")]
    public float TargettingFixedDelay;
    [Tooltip("Delay which already explosion and wait next shoot")]
    public float NextShootDelay;
    public GameObject SnipingModel;
    public GameObject HUDModel;
    public Color HUDColor;

    private int counter;
    private float timer;
    private List<GameObject> snipingList;
    private GameObject HUD = null;
    private Vector3 startPos;
    private const float jumpScale = 20f;
    private RapidFireDependent rfd;
    private bool isTargetting;
    private int status;
    public override void Setting()
    {
        counter = 0;
        timer = 0;
        status = 0;
        isTargetting = false;
        if (HUD == null) {

            HUD=Instantiate(HUDModel);
            HUD.GetComponent<HUD>().Initiate();
            HUD.SetActive(false);
        }
    }
    private void Reset()
    {
        Cooldown = 25f;
        Stack = 1;
        MaxDistance = 10000;
        MinDistance = 0;
        counter = 0;
        timer = 0;
        HowMuch = 3;
        StartDelay = 2f;
        TargettingDelay = 2f;
        TargettingFixedDelay = 1f;
        NextShootDelay = 2f;
        PatternPostDelay = 2f;

    }
    void Awake()
    {
        snipingList= new List<GameObject>();
        rfd = gameObject.GetComponent<RapidFireDependent>();
    }

    public override void Run()
    {
        base.Run();
        Caster.GetComponent<EnemyDefault>().Statement = "LockOn";

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
        Caster.DefaultPhysicalForcedEnable = false;
    }

    public void SetTargetting(bool value) {

        isTargetting = value;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            if (status == 0)
            {

                if (timer < StartDelay)
                {

                    gameObject.transform.position = startPos + new Vector3(0, jumpScale) * (1 - Mathf.Pow((timer / StartDelay) - 1, 2));
                }
                else
                {

                    //ȯ�� ��ü ���� ����
                    isTargetting = true;//�ӽ�


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
                            targetting.transform.position = GameController.GetPlayer.transform.position;
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
                //ȯ�� ��ü�� ������ ���� ���� �޼��� ������
                timer += Time.deltaTime;
                if (timer >= TargettingDelay+TargettingFixedDelay) {

                    if (!HUD.GetComponent<HUD>().Is_end) HUD.GetComponent<HUD>().Is_end = true;
                }

                if (!HUD.activeSelf) {
                    

                    if (isTargetting) {
                        transform.position = new Vector3(rfd.ZeroOffset.x, transform.position.y, 0);
                        Caster.DefaultPhysicalForcedEnable = true;
                        rfd.Run();
                    }
                    isTargetting = false;

                    if (!rfd.IsRun)
                    {
                        Stop();
                    }

                }
            }
            
        }
    }
}
