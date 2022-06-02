using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthDefault
{

    static public GameObject Player=null;

    public Slider HealthSlider = null;
    public PlayerPhysical playerPhysical;
    public PlayerAction playerAction;
    private SpriteRenderer spriteRenderer;
    private PlayerAudio PlayerAudio;

    protected override void Awake()
    {

        if (Player == null)
        {
            Player = gameObject;
        }
        if (HealthSlider == null)
        {
            HealthSlider = GameController.GetGameController().GetComponent<GameController>().HealthGraph;
        }
        base.Awake();
        playerPhysical = gameObject.GetComponent<PlayerPhysical>();
        playerAction = gameObject.GetComponent<PlayerAction>();
        PlayerAudio =gameObject.GetComponent<PlayerAudio>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public override void Hurt(int damage = 0, float immuneTime = 2)
    {
        if (playerAction.IsParrying()) {

            immunTimer = playerAction.ParryingImmuneTime > immunTimer ? playerAction.ParryingImmuneTime : immunTimer;
            return;
        }

        base.Hurt(damage, immuneTime);
    }

    public override void HealthSetting(int value)
    {
        base.HealthSetting(value);
        HealthSlider.maxValue = value;
    }

    void Update()
    {
        
        HealthSlider.value = health;

        if (immunTimer > 0)
        {
            immunTimer -= Time.deltaTime;
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
