using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDefault : MonoBehaviour
{
    public Sprite Normal;

    protected SpriteRenderer sr;
    protected CapsuleCollider2D capColli2D = null;
    protected BoxCollider2D boxColli2D = null;

    protected Vector2 size;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (GetComponent<CapsuleCollider2D>() != null)
        {
            capColli2D = GetComponent<CapsuleCollider2D>();
            size = capColli2D.size;
        }
        if (GetComponent<BoxCollider2D>() != null)
        {
            boxColli2D = GetComponent<BoxCollider2D>();
            size = boxColli2D.size;
        }

        if (Normal == null)
        {
            Normal = sr.sprite;
        }
        sr.sprite = Normal;
        SpriteResize();
    }

    protected void SpriteResize()
    {
        if (GetComponent<CapsuleCollider2D>() != null)
        {
            capColli2D = GetComponent<CapsuleCollider2D>();
            size = capColli2D.size;
        }
        if (GetComponent<BoxCollider2D>() != null)
        {
            boxColli2D = GetComponent<BoxCollider2D>();
            size = boxColli2D.size;
        }

        if (capColli2D != null || boxColli2D != null)
        {
            sr.size = new Vector2(
                (sr.sprite.rect.size.x / sr.sprite.rect.size.y) * (size.y / size.x),
                size.y
            );
        }
    }
}
