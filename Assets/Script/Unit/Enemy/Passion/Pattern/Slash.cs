using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : PatternDefault
{
    public GameObject SlasMainModel;
    public float SlashMainDistance = 1f;
    public GameObject SlashJudgeModel;
    public float SlashJudgeDistance = 2f;
    public float Predelay=0.5f;
    public float ExlopsionDelay=2f;
    public float DamageJudgeTime = 0.1f;

    private GameObject slashMain = null;
    private GameObject slashJudge = null;
    private float transparencyDelayRatio=0.9f;
    private float timer = 0;
    private float phisicalTimer = 0;
    private int step = 0;
    private int direction = 1;

    public override void Setting()
    {
        timer = 0;
        phisicalTimer = 0;
        step = 0;
        if (slashMain == null) {

            slashMain = Instantiate(SlasMainModel);
            slashMain.SetActive(false);
        }
        if (slashJudge == null)
        {
            slashJudge = Instantiate(SlashJudgeModel);
            slashJudge.SetActive(false);
        }
        slashMain.GetComponent<Damage>().IsEffected = true;
        slashJudge.GetComponent<Damage>().IsEffected = true;
        slashJudge.GetComponent<Animator>().SetBool("IsLoop", true);

    }

    private void FixedUpdate()
    {
        if (step == 1 || step == 2) {

            phisicalTimer += Time.fixedDeltaTime;
            if (phisicalTimer > DamageJudgeTime) {

                slashMain.GetComponent<Damage>().IsEffected = false;
                slashJudge.GetComponent<Damage>().IsEffected = false;
            }
        }
    }

    public override void Run()
    {
        base.Run();
        if (transform.position.x < GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos.x)
        {

            direction = 1;
        }
        else { 
        
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            timer += Time.deltaTime;

            if (slashMain.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {

                slashMain.SetActive(false);
            }
            switch (step) { 
            
                //pre delay
                case 0:
                    if (timer > Predelay) {
                        slashMain.transform.position = transform.position + new Vector3(SlashMainDistance, 0) * direction;
                        slashJudge.transform.position = transform.position + new Vector3(SlashJudgeDistance, 0) * direction;

                        slashMain.transform.localScale = new Vector3(Mathf.Abs(slashMain.transform.localScale.x) * direction * (-1),
                            slashMain.transform.localScale.y, slashMain.transform.localScale.z);
                        slashJudge.transform.localScale = new Vector3(Mathf.Abs(slashJudge.transform.localScale.x) * direction,
                            slashJudge.transform.localScale.y, slashJudge.transform.localScale.z);

                        slashMain.SetActive(true);
                        slashJudge.SetActive(true);
                        step = 1;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (timer < ExlopsionDelay * transparencyDelayRatio)
                    {

                        slashJudge.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (1f - timer / (ExlopsionDelay * transparencyDelayRatio)) * 0.9f + 0.1f);
                    }
                    else {

                        slashJudge.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, ((timer- ExlopsionDelay * transparencyDelayRatio) / (ExlopsionDelay * (1f- transparencyDelayRatio))) * 0.9f + 0.1f);
                    }
                    if (timer > ExlopsionDelay) {

                        slashJudge.GetComponent<Damage>().IsEffected = true;
                        slashJudge.GetComponent<Animator>().SetBool("IsLoop", false);
                        step = 2;
                        timer = 0;
                        phisicalTimer = 0;
                    }
                    break;
                case 2:
                    
                    if (slashJudge.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("End")) {
                        slashJudge.SetActive(false);
                        Stop();
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }


}
