using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject settingsMenu;
    public Cursor cursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleSettingsMenu()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void ToggleSnapping(Toggle value)
    {
        cursor.ToggleSnapping(value.isOn);
    }

    public void SetTool(int tool)
    {
        cursor.SetTool((Cursor.ToolType)tool);
    }
}
