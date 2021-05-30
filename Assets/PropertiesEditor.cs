﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesEditor : MonoBehaviour
{
    [Header("Door Stuff")]
    public GameObject doorStuff;
    public ToggleSlider openToggle, lockedToggle;

    [Header("Room Stuff")]
    public GameObject roomStuff;

    public PropertiesObject selectedObj;

    public enum ObjectTypes { Unknown, Room, Door }
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
                selectedObj = new PropertiesObject(gameObj, ObjectTypes.Door);
                ResetDoorProperties();
                break;
            case "Room":
                doorStuff.SetActive(false);
                roomStuff.SetActive(true);
                selectedObj = new PropertiesObject(gameObj, ObjectTypes.Room);
                break;
            default: break;
        }
    }

    void ResetDoorProperties()
    {
        Door doorScript = selectedObj.obj.GetComponent<Door>();
        openToggle.state = doorScript.isOpen;
        lockedToggle.state = doorScript.isLocked;
    }
}
