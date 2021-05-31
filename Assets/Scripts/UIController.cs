using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Cursor cursor;
    public SaveLoad saveLoad;

    public Image[] toolButtons;
    public Color selectedColor = new Color(178, 178, 178);

    public GameObject fileMenu, resetConformation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TestButton(string test)
    {
        print(test);
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void SaveOpen(bool isSave)
    {
        fileMenu.SetActive(false);
        if (isSave) saveLoad.SerializableScene();
        else saveLoad.LoadScene();
    }

    public void ClearButton()
    {
        fileMenu.SetActive(false);
        resetConformation.SetActive(true);
    }

    public void ResetConformation(bool reset)
    {
        resetConformation.SetActive(false);
        if (reset)
            saveLoad.ResetScene();
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
