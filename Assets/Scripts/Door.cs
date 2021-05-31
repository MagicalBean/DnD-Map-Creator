using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public bool isOpen = false, isLocked = false;
    public Sprite closedSprite, openedSprite, lockedSprite;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UpdateData();
    }

    public void UpdateData()
    {
        rotation = gameObject.transform.rotation.eulerAngles;
        position = gameObject.transform.position;
        scale = gameObject.transform.localScale;
    }

    void Update()
    {
        if (isLocked)
        {
            spriteRenderer.sprite = lockedSprite;
            isOpen = false;
        }
        else if (isOpen)
        {
            spriteRenderer.sprite = openedSprite;
        }
        else
        {
            spriteRenderer.sprite = closedSprite;
        }
    }
}
