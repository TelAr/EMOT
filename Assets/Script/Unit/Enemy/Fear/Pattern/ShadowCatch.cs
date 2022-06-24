using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCatch : PatternDefault
{
    [Header("* ShadowCatch Pattern Value")]
    public float PreDelay;
    public float TargettingTime;
    public GameObject PlayerShadowModel;

    private float timer = 0;
    private GameObject playerShadow = null;
    private GameObject esperExplosion = null;
    private Vector3 explosionOffset = Vector3.zero;
    public override void Setting()
    {
        if (playerShadow == null) {

            playerShadow = Instantiate(PlayerShadowModel);
            playerShadow.SetActive(false);
        }
        timer = 0;
    }

    public override void Run()
    {
        base.Run();
        Debug.Log("RUN");
    }

    // Update is called once per frame
    void Update()
    {

        if (IsRun) {

            timer += Time.deltaTime;
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

                esperExplosion=EffectPoolingController.Instance.GetExplosion();
                esperExplosion.transform.position = playerShadow.transform.position+ explosionOffset;
                esperExplosion.transform.localScale *= 4f;
                playerShadow.SetActive(false);
                Stop();
            }
        }
    }
}
