using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkin : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public void SetSkin(Sprite newSkin)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSkin;
        }
    }
}
