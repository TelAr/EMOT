using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDefault : MonoBehaviour
{
    public Sprite Normal;

    protected SpriteRenderer sr;
    protected BoxCollider2D colli2D = null;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (GetComponent<BoxCollider2D>() != null) colli2D = GetComponent<BoxCollider2D>();
        if (Normal == null)
        {
            Normal = sr.sprite;
        }
        sr.sprite = Normal;
        SpriteResize();
    }

    protected void SpriteResize()
    {
        if (colli2D != null)
        {
            sr.size = new Vector2((sr.sprite.rect.size.x / sr.sprite.rect.size.y) * (colli2D.size.y / colli2D.size.x), colli2D.size.y);
        }
    }
}
