using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombs : PatternDefault
{
    public GameObject StickyBombModel;
    public int FireCount;
    public float FireDelay, GlobarBoomTime;
    public float FireVelocity;

    private List<GameObject> StickyBombList = new List<GameObject>();
    private float fireTimer;
    private int counter;
    private Vector3 offset;
    public override void Setting()
    {
        fireTimer = 0;
        counter = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            fireTimer += Time.deltaTime;
            if (fireTimer > FireDelay && counter < FireCount)
            {

                GameObject bomb = null;
                foreach (GameObject p in StickyBombList)
                {

                    if (!p.activeSelf)
                    {

                        bomb = p;
                        bomb.SetActive(true);
                        break;
                    }
                }
                if (bomb == null)
                {

                    bomb = Instantiate(StickyBombModel);
                    bomb.GetComponent<StickyBomb>().BombController = this;
                    StickyBombList.Add(bomb);
                }
                bomb.GetComponent<StickyBomb>().SetTimer(GlobarBoomTime);
                bomb.GetComponent<FixedJoint2D>().connectedBody = null;
                bomb.transform.position = offset + gameObject.transform.position + new Vector3(0, 0, -1);
                bomb.GetComponent<Rigidbody2D>().velocity 
                    = Ballistics.Ballistic(GameController.GetPlayer.transform.position - (offset + transform.position), 
                                            FireVelocity, GameController.GetGameController().GRAVITY);
                fireTimer = 0;
                counter++;
                GetComponent<SadAudio>().GrenadeFirePlay();
            }
            else if(counter >= FireCount) { 
            
                Stop();
            }
        }
    }
}
