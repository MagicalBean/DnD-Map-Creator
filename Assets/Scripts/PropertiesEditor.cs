using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropertiesEditor : MonoBehaviour
{
    [Header("Door Stuff")]
    public GameObject doorStuff;
    public ToggleSlider openToggle, lockedToggle;

    [Header("Room Stuff")]
    public GameObject roomStuff;
    public TMP_InputField nameField, numberField;

    [Header("Stairs Stuff")]
    public GameObject stairsStuff;
    public ToggleSlider directionToggle;

    public PropertiesObject selectedObj;

    public GameObject deleteButton;

    public enum ObjectTypes { Unknown, Room, Door, Stairs }
    public struct PropertiesObject
    {
        public PropertiesObject(GameObject _obj, ObjectTypes _type)
        {
            obj = _obj;
            type = _type;
        }

        public GameObject obj;
        public ObjectTypes type;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedObj.obj != null)
        {
            switch(selectedObj.type)
            {
                case ObjectTypes.Door:
                    Door doorScript = selectedObj.obj.GetComponent<Door>();
                    doorScript.isOpen = openToggle.state;
                    doorScript.isLocked = lockedToggle.state;
                    break;
                case ObjectTypes.Room:
                    Room roomScript = selectedObj.obj.GetComponent<Room>();
                    roomScript.roomName = nameField.text;
                    roomScript.roomNumber = int.Parse(numberField.text);
                    break;
                case ObjectTypes.Stairs:
                    Stair stairsScript = selectedObj.obj.GetComponent<Stair>();
                    stairsScript.switchDirection = directionToggle.state;
                    break;
                case ObjectTypes.Unknown:
                default: break;
            }
        }
    }

    public void OpenPropertiesFor(GameObject gameObj)
    {
        switch (gameObj.name)
        {
            case "Door":
                doorStuff.SetActive(true);
                roomStuff.SetActive(false);
                stairsStuff.SetActive(false);
                deleteButton.SetActive(true);
                selectedObj = new PropertiesObject(gameObj, ObjectTypes.Door);
                ResetDoorProperties();
                break;
            case "Room":
                doorStuff.SetActive(false);
                roomStuff.SetActive(true);
                stairsStuff.SetActive(false);
                deleteButton.SetActive(true);
                selectedObj = new PropertiesObject(gameObj, ObjectTypes.Room);
                ResetRoomProperties();
                break;
            case "Stairs":
                doorStuff.SetActive(false);
                roomStuff.SetActive(false);
                stairsStuff.SetActive(true);
                deleteButton.SetActive(true);
                selectedObj = new PropertiesObject(gameObj, ObjectTypes.Stairs);
                ResetStairsProperties();
                break;
            default:
                doorStuff.SetActive(false);
                roomStuff.SetActive(false);
                stairsStuff.SetActive(false);
                deleteButton.SetActive(false);
                break;
        }
    }

    void OnDeleteClick()
    {
        selectedObj.obj.SetActive(false);
    }

    void ResetDoorProperties()
    {
        Door doorScript = selectedObj.obj.GetComponent<Door>();
        openToggle.state = doorScript.isOpen;
        lockedToggle.state = doorScript.isLocked;
    }

    void ResetRoomProperties()
    {
        Room roomScript = selectedObj.obj.GetComponent<Room>();
        nameField.text = roomScript.roomName;
        numberField.text = roomScript.roomNumber.ToString();
    }

    void ResetStairsProperties()
    {
        Stair stairsScript = selectedObj.obj.GetComponent<Stair>();
        directionToggle.state = stairsScript.switchDirection;
    }
}
