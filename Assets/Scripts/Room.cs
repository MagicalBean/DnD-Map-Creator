using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour
{
    public Vector3 position;
    public Vector2 size;
    public Vector2 colliderSize;
    public Vector2 floorSize;

    public int roomNumber;
    public string roomName = "New Room";

    public TextMesh textMesh;

    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        size = GetComponent<SpriteRenderer>().size;
        position = gameObject.transform.position;
        colliderSize = GetComponent<BoxCollider2D>().size;
        floorSize = GetComponentsInChildren<SpriteRenderer>()[1].size;
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = roomNumber.ToString();
    }
}
