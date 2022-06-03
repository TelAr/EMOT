using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : VisualDefault
{
    public Sprite Dash, Sliding, Down;


    public void NormalSprite() { 
    
        sr.sprite = Normal;
        SpriteResize();
    }

    public void DashSprite() { 
    
        sr.sprite = Dash;
        SpriteResize();

    }

    public void SlidingSprite() { 
    
        sr.sprite = Sliding;
        SpriteResize();
    }

    public void DownSprite() {

        sr.sprite = Down;
        SpriteResize();
        sr.size *= 1.2f;
    }

}
