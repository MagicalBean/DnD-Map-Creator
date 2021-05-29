using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HSVPicker;

public class Brush : MonoBehaviour
{
    public SpriteRenderer brushRenderer;
    public GameObject brushObj, hoverObj;

    public ColorPicker picker;

    public LayerMask UILayer;

    // Start is called before the first frame update
    void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        picker.onValueChanged.AddListener(color =>
        {
            brushRenderer.material.color = color;
        });
        brushRenderer.material.color = picker.CurrentColor;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (IsPointerOverUiElement())
        {
            hoverObj.SetActive(false);
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -1f;
        gameObject.transform.position = mousePos;
        brushObj.SetActive(Input.GetMouseButton(0));
        hoverObj.SetActive(!Input.GetMouseButton(0));
    }

    public bool IsPointerOverUiElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer && curRaysastResult.gameObject.name != "RawImage")
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
