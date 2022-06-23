using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : HealthDefault
{

    static public GameObject Player=null;

    [HideInInspector]
    public PlayerPhysical playerPhysical;
    [HideInInspector]
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
        if (healthSlider == null && GameController.GetGameController.GetComponent<GameController>() != null)
        {
            healthSlider = GameController.GetGameController.GetComponent<GameController>().HealthGraph;
        }
        base.Awake();
        playerPhysical = gameObject.GetComponent<PlayerPhysical>();
        playerAction = gameObject.GetComponent<PlayerAction>();
        PlayerAudio = gameObject.GetComponent<PlayerAudio>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        DontDestroyOnLoad(gameObject);
        healthSlider.maxValue = HealthMax;
    }


    public override void Hurt(int damage = 0, float immuneTime = 2)
    {
        if (playerAction.IsParrying()) {

            immunTimer = playerAction.ParryingImmuneTime > immunTimer ? playerAction.ParryingImmuneTime : immunTimer;
            return;
        }

        if (immunTimer <= 0) {

            PlayerAudio.HurtPlay();
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

    class HealthData { 
    
        public int health;


    }

    public string GetJsonData() {


        HealthData data = new HealthData();

        data.health = health;

        string json = JsonUtility.ToJson(data);
        return json;
    }

    public void SetJsonData(string json)
    {

        HealthData data = JsonUtility.FromJson<HealthData>(json);

        health = data.health;

    }
}
