using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunicBind : PatternDefault
{

    public List<GameObject> RunicWords;
    public float Radius=3f;
    public float PreDelay=2f, Delay=1f;
    [Tooltip("Judge Timing is half of RushTime")]
    public float RushTime = 2f;
    public int DamagePerAttack = 25;
    public float AllowJedgeDelay = 0.1f;
    public float EndDelay = 1f;
    public Vector3 ResetPos;
    [Tooltip("Degree Per Second")]
    public float RotateSpeed = 30;

    private List<GameObject> RunicWordsList = new();
    private PlayerPhysical player = null;
    private Vector3 Target;
    private float timer = 0f;
    private int steps = 0;
    private int counter = 0;
    private SpriteRenderer spriteRenderer;
    private bool isAttack = false;
    private bool specialParrying = false;
    private float parryingTimer = 0f;
    private Color effectColorValue;
    private float rotateState = 0;

    public override void Setting()
    {
        
        timer = 0f;
        steps = 0;
        counter = 0;
        rotateState = Random.Range(0, 360);
        if (player == null)
        {

            player = GameController.GetPlayer.GetComponent<PlayerPhysical>();

        }
        Target = player.TargettingPos;

    }

    private void Awake()
    {
        
        foreach (GameObject word in RunicWords) {

            GameObject obj = Instantiate(word);
            effectColorValue = obj.GetComponent<SpriteRenderer>().color;
            obj.SetActive(false);
            RunicWordsList.Add(obj);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Run()
    {

        base.Run();
        Caster.Statement = "RunicBind";
        for (int t = 0; t < RunicWordsList.Count; t++) { 
        
            GameObject word = RunicWordsList[t];
            word.GetComponent<SpriteRenderer>().color = effectColorValue - new Color(0, 0, 0, effectColorValue.a);
            word.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            word.SetActive(true);
        }
        player.Bind(20f);
    }

    public void OnParrying()
    {
        Debug.Log("p");
        if (!specialParrying) {

            specialParrying = true;
            parryingTimer = AllowJedgeDelay;
        }
    }

    public override void Stop()
    {
        base.Stop();
        player.BindFree();
    }


    private void FixedUpdate()
    {
        if (IsRun && steps == 2)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0, GameController.GetGameController().GRAVITY * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        if (IsRun) 
        {

            timer += Time.deltaTime;
            parryingTimer -= Time.deltaTime;

            rotateState += Time.deltaTime * RotateSpeed;

            for (int t = 0; t < RunicWordsList.Count; t++)
            {
                GameObject word = RunicWordsList[t];
                word.transform.position = Target + new Vector3(Mathf.Cos(Mathf.PI * 2 * t / RunicWordsList.Count + Mathf.Deg2Rad * rotateState),
                    Mathf.Sin(Mathf.PI * 2 * t / RunicWordsList.Count + Mathf.Deg2Rad * rotateState)) * Radius;
            }

            switch (steps) 
            { 
                case 0:

                    if (timer < PreDelay)
                    {
                        float ratio = timer / PreDelay;
                        spriteRenderer.color = new Color(1, 1, 1, 1-ratio);
                        for (int t = 0; t < RunicWordsList.Count; t++)
                        {
                            GameObject word = RunicWordsList[t];
                            word.GetComponent<SpriteRenderer>().color = effectColorValue - (1-ratio)*new Color(0, 0, 0, effectColorValue.a);
                            word.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, ratio);
                            word.transform.position = Target + new Vector3(Mathf.Cos((1 - Mathf.Pow(1 - ratio, 2)) * Mathf.PI * 2 * t / RunicWordsList.Count + Mathf.Deg2Rad * rotateState),
                                Mathf.Sin((1 - Mathf.Pow(1 - ratio, 2)) * Mathf.PI * 2 * t / RunicWordsList.Count + Mathf.Deg2Rad * rotateState)) * Radius;
                        }
                    }
                    else 
                    {
                        steps = 1;
                        timer = 0;
                        specialParrying = false;
                    }
                    break;
                case 1:
                    if (timer < Delay)
                    {
                        float ratio = timer / Delay;
                        //연출용
                        spriteRenderer.color = new Color(1, 1, 1, ratio);
                        transform.position = RunicWordsList[counter].transform.position;
                        RunicWordsList[counter].SetActive(false);
                    }
                    else if (timer < Delay + RushTime)
                    {
                        float ratio = (timer - Delay) / RushTime;
                        transform.position = RunicWordsList[counter].transform.position 
                            + ratio * (Target - RunicWordsList[counter].transform.position).normalized * Radius * 2;

                        if (ratio > 0.5f && !isAttack) {

                            if (parryingTimer < 0)
                            {
                                player.gameObject.GetComponent<PlayerHealth>().Hurt(DamagePerAttack, 0);
                            }
                            else {

                                //성공 이펙트
                                player.GetComponent<PlayerAudio>().ParryingSuccessPlay();
                            }
                            
                            isAttack = true;

                        }

                        if (ratio < 0.5f)
                        {

                            spriteRenderer.color = Color.white;
                        }
                        else 
                        {
                            spriteRenderer.color = new Color(1, 1, 1, 1 - 2 * (ratio - 0.5f));
                        }

                    }
                    else {

                        counter++;
                        timer = 0;
                        isAttack = false;
                        specialParrying = false;
                        if (counter >= RunicWordsList.Count) {

                            transform.position = ResetPos;
                            steps = 2;
                        }
                    }
                    break;
                case 2:
                    if (timer < EndDelay)
                    {
                        float ratio=timer / EndDelay;
                        spriteRenderer.color = new Color(1, 1, 1, ratio);
                    }
                    else {

                        timer = 0;
                        steps = 3;
                    }
                    break;
                case 3:
                    Stop();
                    break;
                default:
                    break;


            }
        }
    }
}
