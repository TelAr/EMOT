using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateBomb : PatternDefault
{
    public GameObject SmokeShellModel, SeparateGrenadeModel;

    public int SmokeCounter;
    public float minX, maxX, YField;
    public Vector3 spellPos;
    private List<GameObject> SmokeShellList = new List<GameObject>(), SeparateGrenadeModelList = new List<GameObject>();
    private Vector3 offset, beforePos;
    private float timer;
    private const float FIRST_DELAY=1, SPEEL_DELAY=2, VELOCITY=15;
    private int step;
    public override void Setting()
    {
        while (SmokeShellList.Count<SmokeCounter) {

            GameObject smoke = Instantiate(SmokeShellModel);
            smoke.SetActive(false);
            SmokeShellList.Add(smoke);
        }

        while (SeparateGrenadeModelList.Count < (2*SmokeCounter - 1)) 
        {

            GameObject gren = Instantiate(SeparateGrenadeModel);
            gren.SetActive(false);
            SeparateGrenadeModelList.Add(gren);
        }
        timer = 0;
        beforePos = transform.position;
        step = 0;

    }
    public void Reset()
    {
        spellPos = Vector3.zero + new Vector3(0, 3, 0);
        SmokeCounter = 4;
        minX = -13;
        maxX = 13;
        YField = -4.5f;
    }

    public override void Run()
    {
        base.Run();
        caster.GetComponent<EnemyDefault>().statement = "UltimateBomb";
    }

    // Update is called once per frame
    void Update()
    {
        if (is_run) {

            timer += Time.deltaTime;
            if (step > 0) {

                transform.position = spellPos;
            }
            switch (step) {
                case 0:
                    if (timer < FIRST_DELAY)
                    {

                        transform.position = beforePos * (FIRST_DELAY - timer) / FIRST_DELAY + spellPos * (timer) / FIRST_DELAY;
                    }
                    else { 
                        timer = 0;
                        step++;
                    }
                    break;
                case 1:
                    if (timer > SPEEL_DELAY) {
                        for (int t = 0; t < SmokeCounter; t++) {

                            GameObject smsh=null;
                            foreach (GameObject go in SmokeShellList) {

                                if (!go.activeSelf) { 
                                
                                    smsh = go;
                                    break;
                                }
                            }
                            if (smsh == null) {

                                smsh = Instantiate(SmokeShellModel);
                                SmokeShellList.Add(smsh);
                            }
                            smsh.SetActive(true);
                            smsh.transform.position = spellPos;
                            smsh.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(new Vector2(minX + t * (maxX - minX) / (SmokeCounter - 1),  YField - transform.position.y), VELOCITY, GameController.GRAVITY, true);
                        }
                        Joy.joyAudio.SwingPlay();
                        timer = 0;
                        step++;
                    }
                    break;
                case 2:
                    if (timer > SPEEL_DELAY)
                    {
                        for (int t = 0; t < SmokeCounter-1; t++)
                        {

                            GameObject spgr = null;
                            foreach (GameObject go in SeparateGrenadeModelList)
                            {
                                if (!go.activeSelf)
                                {

                                    spgr = go;
                                    break;
                                }
                            }
                            if (spgr == null)
                            {

                                spgr = Instantiate(SeparateGrenadeModel);
                                SeparateGrenadeModelList.Add(spgr);
                            }
                            spgr.SetActive(true);
                            spgr.transform.position = spellPos;
                            spgr.transform.localScale = SeparateGrenadeModel.transform.localScale;
                            spgr.GetComponent<GrenadeDefault>().ResetTimer();
                            spgr.GetComponent<GrenadeDefault>().IsDestroy = false;
                            spgr.GetComponent<SeparateGrenadeImpact>().SeparateCount = 1;
                            spgr.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(new Vector2(minX + t * (maxX - minX) / (SmokeCounter - 2), YField - transform.position.y), VELOCITY, GameController.GRAVITY, true);
                        }
                        Joy.joyAudio.SwingPlay();
                        timer = 0;
                        step++;
                    }
                    break;
                case 3:
                    if (timer > SPEEL_DELAY)
                    {
                        for (int t = 0; t < SmokeCounter-2; t++)
                        {

                            GameObject spgr = null;
                            foreach (GameObject go in SeparateGrenadeModelList)
                            {

                                if (!go.activeSelf)
                                {

                                    spgr = go;
                                    break;
                                }
                            }
                            if (spgr == null)
                            {

                                spgr = Instantiate(SeparateGrenadeModel);
                                SeparateGrenadeModelList.Add(spgr);
                            }
                            spgr.SetActive(true);
                            spgr.transform.position = spellPos;
                            spgr.transform.localScale = SeparateGrenadeModel.transform.localScale * 2;
                            spgr.GetComponent<GrenadeDefault>().ResetTimer();
                            spgr.GetComponent<GrenadeDefault>().IsDestroy = false;
                            spgr.GetComponent<SeparateGrenadeImpact>().SeparateCount = 2;
                            spgr.GetComponent<Rigidbody2D>().velocity = Ballistics.Ballistic(new Vector2(minX + t * (maxX - minX) / (SmokeCounter - 3), YField - transform.position.y), VELOCITY, GameController.GRAVITY, true);
                        }
                        Joy.joyAudio.SwingPlay();
                        timer = 0;
                        step++;
                    }
                    break;
                case 4:
                    if (timer > SPEEL_DELAY)
                    {
                        Stop();
                    }
                    break;
                default:
                    break;
            }
            

        }
    }
}
