using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    public bool isOpen = false, isLocked = false;
    public Sprite closedSprite, openedSprite;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isOpen) spriteRenderer.sprite = openedSprite;
        else spriteRenderer.sprite = closedSprite;
    }
}
