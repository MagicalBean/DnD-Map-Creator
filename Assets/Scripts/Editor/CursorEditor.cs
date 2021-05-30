using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cursor))]
public class CursorEditor : Editor
{
    bool showHistory = true, showSnapping = true, showRoom = true, showTools = true;


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
        }

        showTools = EditorGUILayout.Foldout(showTools, "Tools");
        if (showTools)
        {
            cursor.currentTool = (Cursor.ToolType)EditorGUILayout.EnumPopup("Current Tool", cursor.currentTool);
            cursor.dragStart = EditorGUILayout.Vector2Field("Drag Start", cursor.dragStart);
            cursor.dragEnd = EditorGUILayout.Vector2Field("Drag End", cursor.dragEnd);
            if (cursor.currentTool == Cursor.ToolType.Rectangle)
            {
                cursor.roomsParent = (Transform)EditorGUILayout.ObjectField("Rooms Parent", cursor.roomsParent, typeof(Transform), true);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Room Prefab");
                cursor.roomPrefab = (GameObject)EditorGUILayout.ObjectField(cursor.roomPrefab, typeof(GameObject), allowSceneObjects: false);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Box Outline");
                cursor.boxOutline = (Texture2D)EditorGUILayout.ObjectField(cursor.boxOutline, typeof(Texture2D), allowSceneObjects: true);
                EditorGUILayout.EndHorizontal();
            }
            if (cursor.currentTool == Cursor.ToolType.Door)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Door Prefab");
                cursor.doorPrefab = (GameObject)EditorGUILayout.ObjectField(cursor.doorPrefab, typeof(GameObject), allowSceneObjects: false);
                EditorGUILayout.EndHorizontal();
            }
        }

        // DrawDefaultInspector();
    }
}
