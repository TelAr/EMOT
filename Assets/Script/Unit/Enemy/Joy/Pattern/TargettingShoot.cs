using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingShoot : PatternDefault
{
    [Header("* TargettingShoot Pattern Value")]
    public GameObject BulletModel;
    public float BulletSpeed;
    public float BulletHeight;
    public float FloatingTime, TargetingTime;
    public float RandomPositionRange;
    public float RandomDelayRange;
    [Header("Realtime BulletCount: BulletCount+level*(numerator/denominator)")]
    public int BulletCount;
    public int BulletCountPerLevel_numerator;
    public int BulletCountPerLevel_denominator;


    private class BulletController {

        public float timer;
        public float startTime;
        public int Phase;

        public GameObject bullet;
        public Vector3 floatingPos, startPos;
        public bool running;

        bool active;

        public BulletController(GameObject Bullet) {

            bullet = Bullet;
            timer = 0;
            running = false;
            Phase = 0;
        }
        public void Reset(float StartTime) {

            startTime = StartTime;
            timer = 0;
            bullet.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            running = false;
            bullet.SetActive(false);
            Phase = 0;
            
    }
        public void SetActive(bool Active) {

            active = Active;
        }
        public bool IsActive() { 
        
            return active;
        }
        public void Tick()
        {
            timer += Time.fixedDeltaTime;
            if (Phase != 0 && !bullet.activeSelf)
            {

                SetActive(false);
            }
        }
        public void SetPhase(int PhaseValue) { 
        
            Phase = PhaseValue;
            
        }
    }
    private List<BulletController> BulletList;
    
    private Vector3 offset, target;

    private void Reset()
    {
        BulletSpeed = 5;
        BulletHeight = 5;
        RandomPositionRange = 1;
        RandomDelayRange = 1;
        FloatingTime = 1f;
        TargetingTime = 0.5f;
        BulletCountPerLevel_numerator = 1;
        BulletCountPerLevel_denominator = 2;
    }

    public override void Setting()
    {
        offset = new Vector2(0, 0);
        target = new Vector2(0, 0);
    }

    public override void Run() {
        Setting();
        for (int n = 0; n < BulletCount + (BulletCountPerLevel_numerator * GameController.Level / BulletCountPerLevel_denominator); n++)
        {

            bool Is_Awake = false;

            foreach (BulletController b in BulletList)
            {

                if (!b.IsActive())
                {
                    Is_Awake = true;
                    b.SetActive(true);
                    b.Reset(Random.Range(0, RandomDelayRange));
                    break;
                }
            }
            if (!Is_Awake)
            {

                BulletController bc = new BulletController(Instantiate(BulletModel));
                bc.SetActive(true);
                bc.Reset(Random.Range(0, RandomDelayRange));
                BulletList.Add(bc);
            }

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        BulletList=new List<BulletController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int counter = 0;
        foreach (BulletController b in BulletList)
        {
            if (b.IsActive()) {
                counter++;
                b.Tick();

                if (b.timer > b.startTime) {

                    if (b.Phase == 0) {

                        b.bullet.SetActive(true);
                        b.bullet.transform.position = b.startPos = transform.position + offset;
                        b.floatingPos = b.startPos + new Vector3(Random.Range(-RandomPositionRange, RandomPositionRange), 
                            BulletHeight + Random.Range(-RandomPositionRange, RandomPositionRange), 0);
                        b.SetPhase(1);
                    }
                    if (b.timer < b.startTime + FloatingTime)
                    {
                        b.bullet.transform.position = (b.startPos * (b.startTime + FloatingTime - b.timer) + b.floatingPos * (b.timer - b.startTime)) / FloatingTime;
                    }

                    if (b.timer > b.startTime + FloatingTime + TargetingTime) {

                        if (b.Phase == 1) {

                            b.bullet.GetComponent<Rigidbody2D>().velocity = 
                                (GameController.GetPlayer.transform.position - b.floatingPos).normalized * BulletSpeed;

                            b.SetPhase(2);
                        }
                    }

                }

            }
        }
        
    }
}
