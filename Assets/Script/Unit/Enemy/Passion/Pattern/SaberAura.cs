using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberAura : PatternDefault
{
    public List<GameObject> AuraModelList;
    public float PreDelay, AuraDelay;
    public float AuraBeginVel, AuraAccelVel, AuraAccelDelay;
    public int MinimumLevelToTargetting;

    private List<GameObject> auraList=new List<GameObject>();
    private float timer = 0;
    private int step = 0;
    private int counter = 0;
    public override void Setting()
    {
        timer = 0;
        step = 0;
        counter = 0;

        while (auraList.Count < AuraModelList.Count) {

            if (AuraModelList[auraList.Count] != null)
            {
                GameObject aura = Instantiate(AuraModelList[auraList.Count]);
                aura.SetActive(false);
                auraList.Add(aura);
            }
            else {

                auraList.Add(null);
            }
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;
            switch (step) { 
            
                case 0:
                    if (timer > PreDelay) {

                        timer = AuraDelay;
                        step = 1;
                    }
                    break;
                case 1:
                    if (timer > AuraDelay) {

                        if (counter < auraList.Count) {
                            if (auraList[counter] != null) {
                                GameObject now = auraList[counter];
                                Vector3 target = GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos;
                                Vector3 direction = target - Caster.transform.position;
                                now.transform.position = gameObject.transform.position;
                                now.SetActive(true);
                                now.GetComponent<Aura>().Init(AuraBeginVel, AuraAccelVel, AuraAccelDelay,
                                    //target(when low level, just move horizontality, else targetting player)
                                    GameController.Level < MinimumLevelToTargetting ?
                                    new Vector3((direction.x > 0 ? 1 : -1), 0, 0) : direction);
                                now.transform.localScale = new Vector3((direction.x > 0 ? -1 : 1) * Mathf.Abs(now.transform.localScale.x),
                                    now.transform.localScale.y, now.transform.localScale.z);
                                now.transform.rotation = 
                                    Quaternion.Euler(new Vector3(0, 0, Mathf.Acos(Mathf.Abs(direction.normalized.x)) * Mathf.Rad2Deg * (direction.x > 0 ? 1 : -1)));
                            }
                            timer = 0;
                            counter++;
                        }
                        else
                        {
                            step = 2; 
                        }
                    }
                    break;
                case 2:
                    Stop();
                    break;
                default:
                    break;
            }

        }
        
    }
}
