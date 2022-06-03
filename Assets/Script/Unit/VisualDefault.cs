using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDefault : MonoBehaviour
{
    public Sprite Normal;

    protected SpriteRenderer sr;
    protected BoxCollider2D collider2D;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (GetComponent<BoxCollider2D>() != null) collider2D = GetComponent<BoxCollider2D>();
        sr.sprite = Normal;
    }

    protected void SpriteResize()
    {

        sr.size = new Vector2((sr.sprite.rect.size.x / sr.sprite.rect.size.y) * (collider2D.size.y / collider2D.size.x), collider2D.size.y);
    }
}
