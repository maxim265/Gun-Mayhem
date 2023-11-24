using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, Interacts
{
    enum State
    {
        Close,
        Full,
        Empty
    }
    public Sprite closeSprite;
    public Sprite fullSprite;
    public Sprite emptySprite;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeSprite;
    }

    public void Interact()
    {
        ColseTip();
        if (spriteRenderer.sprite == closeSprite)
        {
            if (Random.Range(0, 1) > 0.3)
            {
                spriteRenderer.sprite = fullSprite;
            }
            else
            {
                spriteRenderer.sprite = emptySprite;
            }
        }
        if (spriteRenderer.sprite == emptySprite)
        {
            gameObject.tag = "Untagged";
            Destroy(this);
        }
        if (spriteRenderer.sprite == fullSprite)
        {
            spriteRenderer.sprite = emptySprite;
        }

    }

    public void Tip()
    {
    }

    public void ColseTip()
    {
    }
}