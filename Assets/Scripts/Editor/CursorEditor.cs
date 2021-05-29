using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cursor))]
public class CursorEditor : Editor
{
    bool showHistory = true, showSnapping = true, showRoom = true, showRectTool = true;


    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInspectorGUI()
    {
        Cursor cursor = (Cursor)target;

        EditorStyles.foldout.fontStyle = FontStyle.Bold;

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((Cursor)target), typeof(Cursor), false);
        GUI.enabled = true;

        cursor.UILayer = EditorGUILayout.LayerField("UI Layer", cursor.UILayer);

        showSnapping = EditorGUILayout.Foldout(showSnapping, "Snap Settings");
        if (showSnapping)
        {
            cursor.snapping = EditorGUILayout.Toggle("Snapping", cursor.snapping);
            cursor.snapFraction = EditorGUILayout.IntField("Snap Fraction", cursor.snapFraction);
        }

        showHistory = EditorGUILayout.Foldout(showHistory, "History Settings");
        if (showHistory)
            cursor.storedActions = EditorGUILayout.IntField("Stored Actions", cursor.storedActions);

        showRoom = EditorGUILayout.Foldout(showRoom, "Room Customization");
        if (showRoom)
        {
            cursor.roomsParent = (Transform)EditorGUILayout.ObjectField("Rooms Parent", cursor.roomsParent, typeof(Transform), true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Box Outline Wide");
            cursor.boxOutlineWide = (Sprite)EditorGUILayout.ObjectField(cursor.boxOutlineWide, typeof(Sprite), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
    cursor.currentTool = (Cursor.ToolType)EditorGUILayout.EnumPopup("Current Tool", cursor.currentTool);
        if (showRectTool && cursor.currentTool == Cursor.ToolType.Rectangle)
        {
            cursor.boxOutline = (Texture2D)EditorGUILayout.ObjectField("Rooms Parent", cursor.boxOutline, typeof(Texture2D), true);
        }

        //public Texture2D boxOutline;
        //     public Vector2 dragStart = Vector2.negativeInfinity, dragEnd = Vector2.negativeInfinity;
        //     private Vector2 cursorPos;
        //     private Rect wallRect;

        DrawDefaultInspector();
    }
}
