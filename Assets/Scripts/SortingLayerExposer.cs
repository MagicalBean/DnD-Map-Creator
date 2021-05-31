using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SortingLayerExposer : MonoBehaviour
{

    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    void Awake()
    {
        gameObject.GetComponent <MeshRenderer > ().sortingLayerName = SortingLayerName;
        gameObject.GetComponent <MeshRenderer > ().sortingOrder = SortingOrder;
    }
}