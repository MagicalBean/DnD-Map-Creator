using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class SaveLoad : MonoBehaviour
{
    [Header("Save")]
    public Transform doorsParent;
    public Transform roomsParent;
    public Transform stairsParent;

    [HideInInspector]
    public Door[] doors;
    [HideInInspector]
    public Room[] rooms;
    [HideInInspector]
    public Stair[] stairs;

    [Header("Load")]
    public GameObject roomPrefab;
    public GameObject doorPrefab;
    public GameObject stairsPrefab;

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

        var path = StandaloneFileBrowser.OpenFilePanel("Open Map", Application.dataPath + "/Saved Maps/", "map", false);

        if (File.Exists(path[0]))
        {
            string saveString = File.ReadAllText(path[0]);
            
            SerializableClass data = JsonUtility.FromJson<SerializableClass>(saveString);
            CreateDoors(data.doors);
            CreateRooms(data.rooms);
            CreateStairs(data.stairs);
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
            doorObj.SetActive(true);
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
            roomObj.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(room.size.x), Mathf.Abs(room.size.y));
            roomObj.GetComponentsInChildren<SpriteRenderer>()[1].size = room.floorSize;
            Room script = roomObj.GetComponent<Room>();
            script.roomName = room.roomName;
            script.roomNumber = room.roomNumber;
            roomObj.SetActive(true);
        }
    }

    void CreateStairs(SerializableStairs[] stairs)
    {
        foreach (SerializableStairs stair in stairs)
        {
            GameObject stairObj = Instantiate(stairsPrefab);
            stairObj.name = "Stairs";
            stairObj.transform.parent = stairsParent;
            stairObj.transform.position = stair.position;
            stairObj.transform.eulerAngles = stair.rotation;
            stairObj.GetComponent<SpriteRenderer>().size = stair.size;
            stairObj.GetComponent<BoxCollider2D>().size = new Vector2(Mathf.Abs(stair.size.x), Mathf.Abs(stair.size.y));
            Stair script = stairObj.GetComponent<Stair>();
            script.switchDirection = stair.switchDirection;
            stairObj.SetActive(true);
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
            doors[i].UpdateData();
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
            rooms[i].UpdateData();
            room.position     = rooms[i].position;
            room.size         = rooms[i].size;
            room.floorSize    = rooms[i].floorSize;
            room.roomName     = rooms[i].roomName;
            room.roomNumber   = rooms[i].roomNumber;
            serializedRooms[i] = room;
        }

        SerializableStairs[] serializedStairs = new SerializableStairs[stairs.Length];
        for (int i = 0; i < stairs.Length; i++)
        {
            SerializableStairs stair = new SerializableStairs();
            stairs[i].UpdateData();
            stair.position = stairs[i].position;
            stair.rotation = stairs[i].rotation;
            stair.size = stairs[i].size;
            stair.switchDirection = stairs[i].switchDirection;
            serializedStairs[i] = stair;
        }

        SerializableClass serializableClass = new SerializableClass();
        serializableClass.doors = serializedDoors;
        serializableClass.rooms = serializedRooms;
        serializableClass.stairs = serializedStairs;

        string result = JsonUtility.ToJson(serializableClass);

        var path = StandaloneFileBrowser.SaveFilePanel("Save Map", "", "New Map", "map");
        File.WriteAllText(path, result);
    }

    void GetDataToSeralize()
    {
        doors = doorsParent.GetComponentsInChildren<Door>();
        rooms = roomsParent.GetComponentsInChildren<Room>();
        stairs = stairsParent.GetComponentsInChildren<Stair>();
    }

    #region SerializableTypes
    private class SerializableClass
    {
        public SerializableRoom[] rooms;
        public SerializableDoor[] doors;
        public SerializableStairs[] stairs;
    }

    [System.Serializable]
    private class SerializableRoom
    {
        public Vector3 position;
        public Vector2 size;
        public Vector2 floorSize;
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
    
    [System.Serializable]
    private class SerializableStairs
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector2 size;
        public bool switchDirection;
    }
    #endregion

    public void ResetScene() // this doesn't belong here but it goes here for now
    {
        GetDataToSeralize();

        foreach (Door door in doors)
        {
            GameObject.Destroy(door.gameObject);
        }

        foreach (Room room in rooms)
        {
            GameObject.Destroy(room.gameObject);
        }
        
        foreach (Stair stair in stairs)
        {
            GameObject.Destroy(stair.gameObject);
        }
    }
}
