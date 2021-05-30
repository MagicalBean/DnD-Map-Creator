using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject settingsMenu;
    public Cursor cursor;

    public Image[] toolButtons;
    public Color selectedColor = new Color(178, 178, 178);

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
        foreach (Image image in toolButtons)
        {
            if (image == toolButtons[tool - 1]) image.color = selectedColor;
            else image.color = Color.white;
        }
    }
}
