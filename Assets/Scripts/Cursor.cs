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

    public enum ToolType { None = 0, Select = 1, Rectangle = 2, Door = 3, }
    public ToolType currentTool;

    // History
    public int storedActions = 10;
    public List<Action> actionHistory = new List<Action>();
    public List<Action> actionRedo = new List<Action>();
    
    public enum ActionType { CreateRoom, CreateDoor };
    public struct Action
    {
        public Action(ActionType _type, GameObject _object) // For CreateRoom && CreateDoor
        {
            actionType = _type;
            createdObject = _object;
        }

        public ActionType actionType;
        public GameObject createdObject;
    }

    public Vector2 dragStart = Vector2.negativeInfinity, dragEnd = Vector2.negativeInfinity;

    // Rectangle Tool
    public Transform roomsParent;
    public GameObject roomPrefab;
    public Texture2D boxOutline;
    private Vector2 cursorPos;
    private Rect wallRect;

    // Door Tool
    public Transform doorsParent;
    public GameObject doorPrefab;

    // Select Tool
    public GameObject currentSelection;

    private Camera mainCam;
    public PropertiesEditor propertiesEditor;

    // Start is called before the first frame update
    void Start()
    {
        propertiesEditor = GameObject.FindObjectOfType<PropertiesEditor>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCursor();
        CheckInput();
    }

    void CheckInput()
    {
        if (IsPointerOverUiElement()) return;

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("z"))
        {
            HandleUndo();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("y"))
        {
            HandleRedo();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentTool == ToolType.Rectangle || currentTool == ToolType.Door) dragStart = cursorPos;
            if (currentTool == ToolType.Select) SelectObject(cursorPos);
        }

        if (Input.GetMouseButton(0))
        {
            if (currentTool == ToolType.Rectangle || currentTool == ToolType.Door) dragEnd = cursorPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentTool == ToolType.Rectangle)
            {
                DrawBox();
                dragStart = Vector2.negativeInfinity;
                dragEnd = Vector2.negativeInfinity;
            }

            if (currentTool == ToolType.Door)
            {
                DrawDoor();
                dragStart = Vector2.negativeInfinity;
                dragEnd = Vector2.negativeInfinity;
            }
        }
    }

    void HandleUndo()
    {
        Action lastAction = actionHistory[actionHistory.Count - 1];

        switch (lastAction.actionType)
        {
            case ActionType.CreateRoom:
            case ActionType.CreateDoor:
                actionHistory.RemoveAt(actionHistory.Count - 1);
                lastAction.createdObject.SetActive(false);
                actionRedo.Add(lastAction);
                break;
            default: break;
        }
    }

    void HandleRedo()
    {
        Action lastAction = actionRedo[actionRedo.Count - 1];

        switch (lastAction.actionType)
        {
            case ActionType.CreateRoom:
            case ActionType.CreateDoor:
                actionRedo.RemoveAt(actionRedo.Count - 1);
                lastAction.createdObject.SetActive(true);
                actionHistory.Add(lastAction);
                break;
            default: break;
        }
    }

    void SelectObject(Vector2 cursorPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(cursorPos, Vector2.zero);

        if (hit.collider != null)
        {
            propertiesEditor.OpenPropertiesFor(hit.collider.gameObject);
        }
    }

    void DrawBox()
    {
        // Create box to add
        //         GameObject newBox = new GameObject("Room");
        //         newBox.transform.localScale = new Vector3(2, 2, 1);
        //         SpriteRenderer spriteRenderer = newBox.AddComponent<SpriteRenderer>();
        //         spriteRenderer.sprite = boxOutlineWide;
        //         spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        //         newBox.AddComponent<BoxCollider2D>();

        GameObject newBox = Instantiate(roomPrefab);
        newBox.transform.parent = roomsParent;
        newBox.name = "Room";

        // Resize box to correct sizes
        Vector3 startPos = (new Vector3(wallRect.x, wallRect.y));
        Vector3 endPos = (new Vector3(wallRect.x + wallRect.width, wallRect.y - wallRect.height));

        Vector3 centerPos = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, 0) / 2;
        Vector2 size = new Vector2((dragStart.x - dragEnd.x), dragStart.y - dragEnd.y) / 2;

        newBox.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        newBox.GetComponentsInChildren<SpriteRenderer>()[1].size = new Vector2(Mathf.Abs(size.x) * 2, Mathf.Abs(size.y) * 2);

        if (size.x >= 0) size.x += 0.06f; // positive x
        if (size.x <= 0) size.x -= 0.06f; // negative x
        if (size.y >= 0) size.y += 0.06f; // positive y
        if (size.y <= 0) size.y -= 0.06f; // negative y

        newBox.transform.position = centerPos;
        newBox.GetComponent<SpriteRenderer>().size = size;
        newBox.SetActive(true);
        actionHistory.Add(new Action(ActionType.CreateRoom, newBox));
    }

    void DrawDoor()
    {
        GameObject door = Instantiate(doorPrefab);
        door.transform.parent = doorsParent;

        // Resize box to correct sizes
        Vector3 startPos = (new Vector3(wallRect.x, wallRect.y));
        Vector3 endPos = (new Vector3(wallRect.x + wallRect.width, wallRect.y - wallRect.height));

        Vector2 size = new Vector2((dragStart.x - dragEnd.x), dragStart.y - dragEnd.y) / 2;

        Vector3 centerPos = Vector3.one;
        float xScale, yScale;

        if (Mathf.Abs(size.x) > Mathf.Abs(size.y)) // Horizontal line
        {
            door.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            xScale = startPos.x - endPos.x;
            door.transform.localScale = new Vector3(xScale, 2, 1);
            centerPos = new Vector3(startPos.x + endPos.x, startPos.y * 2, -2) / 2;
        }
        if (Mathf.Abs(size.x) < Mathf.Abs(size.y)) // Vertical line
        {
            door.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            yScale = startPos.y - endPos.y;
            door.transform.localScale = new Vector3(yScale, 2, 1);
            centerPos = new Vector3(startPos.x * 2, startPos.y + endPos.y, -2) / 2;
        }

        door.name = "Door";
        door.transform.position = new Vector3(centerPos.x, centerPos.y, -1);
        door.SetActive(true);
        actionHistory.Add(new Action(ActionType.CreateDoor, door));

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
        transform.position = new Vector3(x, y, -3);
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
            if (currentTool == ToolType.Rectangle)
            {
                GUI.Box(boxRect, "", style);
                GUI.Label(new Rect(topLeftScreen.x, Screen.height - topLeftScreen.y - 35, -width, 50), boxLabel, labelStyle);
            }

            if (currentTool == ToolType.Door)
            {
                if (Mathf.Abs(boxRect.width) > Mathf.Abs(boxRect.height)) // Horizontal line
                    GUI.Box(new Rect(boxRect.x, boxRect.y - 5, -width, 10), "", style);
                if (Mathf.Abs(boxRect.width) < Mathf.Abs(boxRect.height)) // Vertical line
                    GUI.Box(new Rect(boxRect.x - 5, boxRect.y, 10, height), "", style);
            }
        }
    }

    void OnDrawGizmos()
    {

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
