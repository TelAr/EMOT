using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCatch : PatternDefault
{
    [Header("* ShadowCatch Pattern Value")]
    public float PreDelay;
    public float TargettingTime;
    public GameObject PlayerShadowModel;
    public float ExplosionAudioPreDelay = 1.9f;

    private float timer = 0;
    private GameObject playerShadow = null;
    private GameObject esperExplosion = null;
    private Vector3 explosionOffset = Vector3.zero;
    private bool isAudioPlay = false;
    public override void Setting()
    {
        if (playerShadow == null) {

            playerShadow = Instantiate(PlayerShadowModel);
            playerShadow.SetActive(false);
        }

        timer = 0;
        isAudioPlay = false;
    }

    public override void Run()
    {
        base.Run();
        
    }

    public override void Stop()
    {
        base.Stop();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsRun) {

            timer += Time.deltaTime;

            if (!isAudioPlay && timer >= PreDelay + TargettingTime - ExplosionAudioPreDelay)
            {

                //Caster.GetComponent<FearAudio>().ExplosionPlay(); // 구버전 사운드 시스템
                Caster.GetComponent<Sound_manager>().Play("explosion"); // 신버전 사운드 시스템
                isAudioPlay = true;
            }

            if (timer < PreDelay)
            {



            }
            else if (timer < PreDelay + TargettingTime)
            {

                if (!playerShadow.activeSelf)
                {
                    
                    playerShadow.GetComponent<SpriteRenderer>().sprite = GameController.GetPlayer.GetComponent<SpriteRenderer>().sprite;
                    playerShadow.GetComponent<SpriteRenderer>().size = GameController.GetPlayer.GetComponent<SpriteRenderer>().size;
                    playerShadow.GetComponent<SpriteRenderer>().flipX = GameController.GetPlayer.GetComponent<SpriteRenderer>().flipX;
                    playerShadow.GetComponent<SpriteRenderer>().color = Color.gray;
                    playerShadow.transform.position = GameController.GetPlayer.transform.position;
                    explosionOffset = (GameController.GetPlayer.transform.localScale.x / GameController.GetPlayer.transform.localScale.normalized.x)
                        * GameController.GetPlayer.GetComponent<Collider2D>().offset * 0.5f;
                    if (playerShadow.GetComponent<LayerOrder>() == null) {
                        playerShadow.AddComponent<LayerOrder>().value = 1;
                    }
                    playerShadow.SetActive(true);
                }

                playerShadow.GetComponent<SpriteRenderer>().color = Color.gray - new Color(0.5f, 0.5f, 0.5f, 0) * (timer - TargettingTime) / PreDelay;

            }
            else {

                esperExplosion=EffectPoolingController.Instance.GetPsychicExplosion();
                esperExplosion.transform.position = playerShadow.transform.position+ explosionOffset;
                esperExplosion.transform.localScale *= 4f;
                playerShadow.SetActive(false);
                Stop();
            }
        }
    }
}
