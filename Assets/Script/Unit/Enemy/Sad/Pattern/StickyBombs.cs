using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBombs : PatternDefault
{
    public float KaBoom;
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
        KaBoom = GlobarBoomTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_run) {

            fireTimer += Time.deltaTime;
            KaBoom -= Time.deltaTime;
            if (fireTimer > FireDelay && counter < FireCount) {

                GameObject bomb = null;
                foreach (GameObject p in StickyBombList) {

                    if (!p.activeSelf) { 
                    
                        bomb = p;
                        bomb.SetActive(true);
                        break;
                    }
                }
                if (bomb == null) { 
                
                    bomb=Instantiate(StickyBombModel);
                    bomb.GetComponent<StickyBomb>().BombController = this;
                    StickyBombList.Add(bomb);
                }

                bomb.transform.position = offset + gameObject.transform.position;
                bomb.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(GameController.GetPlayer().transform.position - (offset + transform.position), FireVelocity, GameController.GRAVITY);

                fireTimer = 0;
                counter++;
            }

            if (KaBoom <= 0) {

                Stop();
            }
        }
    }
}
