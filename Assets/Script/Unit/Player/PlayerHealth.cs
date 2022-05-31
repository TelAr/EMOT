using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthDefault
{

    static public GameObject Player=null;

    public Slider HealthSlider = null;
    public PlayerPhysical playerPhysical;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
