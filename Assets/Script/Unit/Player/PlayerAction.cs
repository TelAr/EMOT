using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    public int StaminaMax;
    public float StaminaPerSecond;
    private int staminaValue;
    private Slider staminaSlider = null;
    private float staminaTimer;

    public int ParryingCost, DashCost;


    public GameObject BulletModel;
    private List<GameObject> bullets = new List<GameObject>();
    public float BulletSpeed = 20f;
    public int BulletMax = 7;
    private int bulletAmount = 0;
    public float ChargingDelayPerBullet = 0.52f, ChargingFireDelay = 0.07f;
    private bool isCharging = false;
    private float chargingTimer = 0;
    private int CharingCounter;
    public float BulletReloadDelay = 3f;
    private float reloadTimer = 0;
    public Vector3 OffsetPosition;


    public float ParryingJudgeTime = 0.1f;
    public float ParryingImmuneTime = 0.5f;
    private float parryingJudgeTimer = 0f;

    private PlayerPhysical pp;
    private PlayerHealth ph;
    private PlayerAudio pa;
    void Awake()
    {
        pp = GetComponent<PlayerPhysical>();
        ph = GetComponent<PlayerHealth>();
        pa = GetComponent<PlayerAudio>();
        bulletAmount = BulletMax;
        for (int t = 0; t < BulletMax; t++)
        {

            GameObject bullet = Instantiate(BulletModel);
            bullet.SetActive(false);
            bullets.Add(bullet);

        }
        if (GameController.GetGameController().GetComponent<GameController>().StaminaGraph != null) {

            staminaSlider = GameController.GetGameController().GetComponent<GameController>().StaminaGraph;
        }
        staminaSlider.maxValue = StaminaMax;
        staminaValue = StaminaMax;
        staminaTimer = 0;
    }

    public bool IsParrying() {

        if (parryingJudgeTimer > 0) {

            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnFire(InputValue value) {


        if (bulletAmount <= 0)
        {

            //불발 음원 재생
            return;
        }
        if (pp.IsUniquAction) {

            return;
        }

        if (value.isPressed)
        {
            chargingTimer = 0;
            CharingCounter = 1;
            isCharging = true;

        }
        else {
            chargingTimer = 0;
            isCharging = false;
        }
    }

    public void OnJump(InputValue value)
    {
        if (pp.IsUniquAction) {

            return;
        }
        pp.isJump = value.Get<float>() > 0;
    }

    public void OnMove(InputValue value)
    {
        if (pp.IsUniquAction) {

            return;
        }
        float moving = value.Get<Vector2>().x;
        if (moving < 0)
        {

            moving = -1;
        }
        if (moving > 0)
        {

            moving = 1;
        }

        pp.Moving(moving);
    }

    public void OnDash() {

        if (staminaValue < DashCost) {

            return;
        }

        staminaValue -= DashCost;

        pp.Dash();
    }

    public void OnParrying() {

        if (staminaValue < ParryingCost) {

            return;
        }

        staminaValue -= ParryingCost;

        parryingJudgeTimer = ParryingImmuneTime;
    }



    // Update is called once per frame
    void Update()
    {
        if (staminaValue < StaminaMax)
        {

            staminaTimer += Time.deltaTime;
            if (staminaTimer > 1 / StaminaPerSecond)
            {

                staminaValue++;
                staminaTimer = 0;
            }
        }
        else if (staminaValue > StaminaMax) {

            staminaValue = StaminaMax;
        }
        staminaSlider.value = staminaValue;


        if (bulletAmount <= 0)
        {
            reloadTimer+=Time.deltaTime;

            if (reloadTimer > BulletReloadDelay) {

                pa.ReloadPlay();
                bulletAmount = BulletMax;
                reloadTimer = 0;
            }
        }

        if (isCharging)
        {

            chargingTimer += Time.deltaTime;
            if (chargingTimer > ChargingDelayPerBullet && CharingCounter < bulletAmount)
            {

                CharingCounter++;
                chargingTimer = 0;
            }
        }
        else {

            chargingTimer += Time.deltaTime;

            if (chargingTimer> ChargingFireDelay && CharingCounter > 0) {

                GameObject fireBullet = null;
                foreach (GameObject bullet in bullets)
                {

                    if (!bullet.activeSelf)
                    {

                        fireBullet = bullet;
                        break;
                    }
                }
                if (fireBullet == null)
                {

                    fireBullet = Instantiate(BulletModel);
                    bullets.Add(fireBullet);
                }
                fireBullet.SetActive(true);
                fireBullet.transform.position = gameObject.transform.position + OffsetPosition;
                fireBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(BulletSpeed * pp.GetDirection(), 0);
                chargingTimer = 0;
                bulletAmount--;
                CharingCounter--;
                pa.FirePlay(0.5f);
            }
        }

    }

    private void FixedUpdate()
    {
        if (parryingJudgeTimer > 0)
        {
            parryingJudgeTimer -= Time.fixedDeltaTime;
        }
    }
}
