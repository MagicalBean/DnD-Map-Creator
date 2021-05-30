using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Room : MonoBehaviour
{

    public int roomNumber;
    public string roomName = "New Room";

    public TextMesh textMesh;

    // Start is called before the first frame update
    void OnEnable()
    {
        textMesh = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = roomNumber.ToString();
    }
}
