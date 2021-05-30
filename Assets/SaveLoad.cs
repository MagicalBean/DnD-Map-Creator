﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    [Header("Save")]
    public Transform doorsParent;
    public Transform roomsParent;

    [HideInInspector]
    public Door[] doors;
    [HideInInspector]
    public Room[] rooms;

    [Header("Load")]
    public GameObject roomPrefab;
    public GameObject doorPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Load 
    public void LoadScene()
    {
        if (File.Exists(Application.dataPath + "/Saved Maps/testmap.map"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/Saved Maps/testmap.map");
            
            SerializableClass data = JsonUtility.FromJson<SerializableClass>(saveString);
            CreateDoors(data.doors);
            CreateRooms(data.rooms);
        }
        else
        {
            print("No file found!");
        }
    }

    void CreateDoors(SerializableDoor[] doors)
    {
        foreach (SerializableDoor door in doors)
        {
            GameObject doorObj = Instantiate(doorPrefab);
            doorObj.name = "Door";
            doorObj.transform.parent = doorsParent;
            doorObj.transform.position = door.position;
            doorObj.transform.rotation = Quaternion.Euler(door.rotation);
            doorObj.transform.localScale = door.scale;

            Door script = doorObj.GetComponent<Door>();
            script.isOpen = door.isOpen;
            script.isLocked = door.isLocked;
        }
    }
    
    void CreateRooms(SerializableRoom[] rooms)
    {
        foreach (SerializableRoom room in rooms)
        {
            GameObject roomObj = Instantiate(roomPrefab);
            roomObj.name = "Room";
            roomObj.transform.parent = roomsParent;
            roomObj.transform.position = room.position;
            roomObj.GetComponent<SpriteRenderer>().size = room.size;
            roomObj.GetComponent<BoxCollider2D>().size = room.colliderSize;
            Room script = roomObj.GetComponent<Room>();
            script.roomName = room.roomName;
            script.roomNumber = room.roomNumber;
        }
    }
    #endregion

    public void SerializableScene()
    {
        GetDataToSeralize();

        SerializableDoor[] serializedDoors = new SerializableDoor[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            SerializableDoor door = new SerializableDoor();
            door.position = doors[i].position;
            door.rotation = doors[i].rotation;
            door.scale    = doors[i].scale;
            door.isLocked = doors[i].isLocked;
            door.isOpen   = doors[i].isOpen;
            serializedDoors[i] = door;
        }

        SerializableRoom[] serializedRooms = new SerializableRoom[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            SerializableRoom room = new SerializableRoom();
            room.position     = rooms[i].position;
            room.size         = rooms[i].size;
            room.colliderSize = rooms[i].colliderSize;
            room.roomName     = rooms[i].roomName;
            room.roomNumber   = rooms[i].roomNumber;
            serializedRooms[i] = room;
        }

        SerializableClass serializableClass = new SerializableClass();
        serializableClass.doors = serializedDoors;
        serializableClass.rooms = serializedRooms;

        string result = JsonUtility.ToJson(serializableClass);
        File.WriteAllText(Application.dataPath + "/Saved Maps/testmap.map", result);
    }

    void GetDataToSeralize()
    {
        doors = doorsParent.GetComponentsInChildren<Door>();
        rooms = roomsParent.GetComponentsInChildren<Room>();
    }

    #region SerializableTypes
    private class SerializableClass
    {
        public SerializableRoom[] rooms;
        public SerializableDoor[] doors;
    }

    [System.Serializable]
    private class SerializableRoom
    {
        public Vector3 position;
        public Vector2 size;
        public Vector2 colliderSize;
        public string roomName;
        public int roomNumber;
    }

    [System.Serializable]
    private class SerializableDoor
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public bool isOpen;
        public bool isLocked;
    }
    #endregion
}