using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : HealthDefault
{

    static public GameObject Player=null;

    public PlayerPhysical playerPhysical;
    public PlayerAction playerAction;
    private SpriteRenderer spriteRenderer;
    private PlayerAudio PlayerAudio;
    private Slider healthSlider = null;

    protected override void Awake()
    {

        if (Player == null)
        {
            Player = gameObject;
        }
        else {

            Destroy(gameObject);
        }
        if (healthSlider == null && GameController.GetGameController().GetComponent<GameController>() != null)
        {
            healthSlider = GameController.GetGameController().GetComponent<GameController>().HealthGraph;
        }
        base.Awake();
        playerPhysical = gameObject.GetComponent<PlayerPhysical>();
        playerAction = gameObject.GetComponent<PlayerAction>();
        PlayerAudio = gameObject.GetComponent<PlayerAudio>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        DontDestroyOnLoad(gameObject);
    }


    public override void Hurt(int damage = 0, float immuneTime = 2)
    {
        if (playerAction.IsParrying()) {

            immunTimer = playerAction.ParryingImmuneTime > immunTimer ? playerAction.ParryingImmuneTime : immunTimer;
            return;
        }

        base.Hurt(damage, immuneTime);
    }

    public override void SetMaxHealth(int value)
    {
        base.SetMaxHealth(value);
        healthSlider.maxValue = value;
    }

    void Update()
    {
        
        healthSlider.value = health;

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
