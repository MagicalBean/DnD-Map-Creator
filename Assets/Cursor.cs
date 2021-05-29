using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Cursor : MonoBehaviour
{
    public bool snapping = true;
    public int snapFraction = 2;

    public LayerMask UILayer;

    public enum ToolType { None = 0, Rectangle = 1, Wall = 2, }
    public ToolType currentTool;

    // History
    public int storedActions = 10;
    public List<Action> actionHistory = new List<Action>();
    public enum ActionType { CreateRoom };
    public struct Action
    {
        public Action(ActionType _type, GameObject _object)
        {
            actionType = _type;
            createdObject = _object;
        }

        ActionType actionType;
        GameObject createdObject;
    }
     

    // Room Customization
    public Transform roomsParent;
    public Sprite boxOutlineWide;
    public Material wallMat;

    // Rectangle Tool
    public Texture2D boxOutline;
    public Vector2 dragStart = Vector2.negativeInfinity, dragEnd = Vector2.negativeInfinity;
    private Vector2 cursorPos;
    private Rect wallRect;

    private Camera mainCam;

    //[Header("Temp")]

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCursor();
        CheckMouse();
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentTool == ToolType.Rectangle) dragStart = cursorPos;
        }

        if (Input.GetMouseButton(0))
        {
            if (currentTool == ToolType.Rectangle) dragEnd = cursorPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentTool == ToolType.Rectangle)
            {
                DrawBox();
                dragStart = Vector2.negativeInfinity;
                dragEnd = Vector2.negativeInfinity;
            }
        }
    }

    void DrawBox()
    {
        // Create box to add
        GameObject newBox = new GameObject("Room");
        newBox.transform.parent = roomsParent;
        newBox.transform.localScale = new Vector3(2, 2, 1);
        SpriteRenderer spriteRenderer = newBox.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = boxOutlineWide;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;

        // Resize box to correct sizes
        Vector3 startPos = (new Vector3(wallRect.x, wallRect.y));
        Vector3 endPos = (new Vector3(wallRect.x + wallRect.width, wallRect.y - wallRect.height));

        Vector3 centerPos = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, -2) / 2;
        Vector2 size = new Vector2((dragStart.x - dragEnd.x), dragStart.y - dragEnd.y) / 2;

        if (size.x >= 0) size.x += 0.06f; // positive x
        if (size.x <= 0) size.x -= 0.06f; // negative x
        if (size.y >= 0) size.y += 0.06f; // positive y
        if (size.y <= 0) size.y -= 0.06f; // negative y

        newBox.transform.position = centerPos;
        newBox.GetComponent<SpriteRenderer>().size = size;
        actionHistory.Add(new Action(ActionType.CreateRoom, newBox));
    }

    void MoveCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = mousePos.x;
        float y = mousePos.y;
        if (snapping)
        {
            x = Mathf.Round(x * snapFraction) / snapFraction;
            y = Mathf.Round(y * snapFraction) / snapFraction;
        }
        transform.position = new Vector3(x, y, -1);
        cursorPos = new Vector2(x, y);
    }

    public void ToggleSnapping(bool value)
    {
        snapping = value;
    }

    public void SetTool(ToolType tool)
    {
        currentTool = tool;
    }

    public void ChangeSnapFraction(TMP_Dropdown dropdown)
    {
        snapFraction = dropdown.value + 1;
    }

    void OnGUI()
    {
        GUI.color = Color.blue;
        int borderSize = 4;
        GUIStyle style = new GUIStyle();
        style.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);
        style.normal.background = boxOutline;

        Vector3 topLeft = new Vector3(dragStart.x, dragStart.y, 0);
        Vector3 bottomRight = new Vector3(dragEnd.x, dragEnd.y, 0);
        wallRect = new Rect(topLeft.x, topLeft.y, -(topLeft.x - bottomRight.x), (topLeft.y - bottomRight.y));

        Vector3 topLeftScreen = Camera.main.WorldToScreenPoint(topLeft);
        Vector3 bottomRightScreen = Camera.main.WorldToScreenPoint(bottomRight);

        float width = topLeftScreen.x - bottomRightScreen.x;
        float height = topLeftScreen.y - bottomRightScreen.y;

        Rect boxRect = new Rect(topLeftScreen.x, Screen.height - topLeftScreen.y, -width, height);

        string boxLabel = Mathf.Abs(dragStart.x - dragEnd.x).ToString("f2") + " x " + Mathf.Abs(dragStart.y - dragEnd.y).ToString("f2");

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;

        if (topLeftScreen.x != 0)
        {
            GUI.Box(boxRect, "", style);
            GUI.Label(new Rect(topLeftScreen.x, Screen.height - topLeftScreen.y - 35, -width, 50), boxLabel, labelStyle);
        }
    }

    #region PointerOverUI
    public bool IsPointerOverUiElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
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
    #endregion
}
