using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector2 size;

    public bool switchDirection;

    public Vector3 originalRotation;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (switchDirection)
            originalRotation = transform.eulerAngles - new Vector3(0, 0, 180);
        else originalRotation = transform.eulerAngles;
        UpdateData();
    }

    public void UpdateData()
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        size = GetComponent<SpriteRenderer>().size;
    }

    // Update is called once per frame
    void Update()
    {
        if (switchDirection)
        {
            transform.eulerAngles = originalRotation + new Vector3(0, 0, 180);
        }
        else
        {
            transform.eulerAngles = originalRotation;
        }
    }
}
