using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    enum DoorStatus
    {
        Required,
        Optional,
        Forbidden,
    }

    public static WorldBuilder Instance { get; private set; }
    Dictionary<Vector2Int, GameObject> dungeon;

    [SerializeField]
    GameObject[] roomPrefabs;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        dungeon = new Dictionary<Vector2Int, GameObject>();
    }

    private GameObject AddRoom(int x, int y, GameObject room, Vector3 pos)
    {
        GameObject go = GameObject.Instantiate(room, pos, Quaternion.identity, gameObject.transform);
        dungeon.Add(new Vector2Int(x, y), go);
        go.GetComponentInChildren<RoomInfo>().DungeonPosition = new Vector2Int(x, y);

        return go;
    }

    private void Start()
    {
        if (transform.childCount == 0)
        {
            AddRoom(0, 0, roomPrefabs[0], new Vector3(0, 0, 0));
        }
    }

    public void SpawnRoom(Transform trigger, RoomExit.EDirection direction)
    {
        Vector2Int parentDungeonPos = trigger.parent.GetComponentInChildren<RoomInfo>().DungeonPosition;
        Vector2Int newRoomPosInDictionary = Vector2Int.zero;
        RoomExit[] doorPoints = trigger.parent.GetComponentsInChildren<RoomExit>();
        int offsetX = 0;
        int offsetY = 0;

        switch (direction)
        {
            case RoomExit.EDirection.North:
                offsetX = -1;
                offsetY = 0;
                newRoomPosInDictionary = new Vector2Int(parentDungeonPos.x, parentDungeonPos.y + 1);
                break;
            case RoomExit.EDirection.East:
                offsetX = 0;
                offsetY = -1;
                newRoomPosInDictionary = new Vector2Int(parentDungeonPos.x + 1, parentDungeonPos.y);
                break;
            case RoomExit.EDirection.South:
                offsetX = -1;
                offsetY = -2;
                newRoomPosInDictionary = new Vector2Int(parentDungeonPos.x, parentDungeonPos.y - 1);
                break;
            case RoomExit.EDirection.West:
                offsetX = -2;
                offsetY = -1;
                newRoomPosInDictionary = new Vector2Int(parentDungeonPos.x - 1, parentDungeonPos.y);
                break;
        }

        Vector3 pos = new Vector3(trigger.position.x + offsetX, trigger.position.y + offsetY, trigger.position.z) + trigger.transform.localPosition;
        Dictionary<RoomExit.EDirection, DoorStatus> doors = new Dictionary<RoomExit.EDirection, DoorStatus>();

        //Check if there are neighboring rooms with doors etc and choose another prefab.
        foreach (RoomExit.EDirection dir in RoomExit.EDirection.GetValues(typeof(RoomExit.EDirection)))
        {
            GameObject newRoomNeighbor = FindAdjacentRoom(newRoomPosInDictionary, dir);
            if (newRoomNeighbor == null)
                doors[dir] = DoorStatus.Optional;
            else if (newRoomNeighbor.GetComponent<RoomInfo>().HasDoorAt(OppositeDirection(dir)))
                doors[dir] = DoorStatus.Required;
            else
                doors[dir] = DoorStatus.Forbidden;
        }

        GameObject appropriateRoomPrefab = FindAppropriateRoom(doors);
        GameObject go = AddRoom(newRoomPosInDictionary.x, newRoomPosInDictionary.y, appropriateRoomPrefab, pos.ToVector3IntOnGrid());
        DestoryUnnecessaryExits(go, direction);
    }

    private GameObject FindAppropriateRoom(Dictionary<RoomExit.EDirection, DoorStatus> doors)
    {
        if (doors[RoomExit.EDirection.North] == DoorStatus.Forbidden || doors[RoomExit.EDirection.South] == DoorStatus.Forbidden)
        {
            return roomPrefabs[1];
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
               return roomPrefabs[0];
            }
            else
            {
                return roomPrefabs[1];
            }
        }

    }

    private void DestoryUnnecessaryExits(GameObject room, RoomExit.EDirection direction)
    {
        RoomExit[] newDoorPoints = room.GetComponentsInChildren<RoomExit>();
        foreach (RoomExit newExit in newDoorPoints)
        {
            if (newExit.Direction == OppositeDirection(direction))
            {
                Destroy(newExit.gameObject);
            }
        }
    }

    private GameObject FindAdjacentRoom(Vector2Int dictionaryPosition, RoomExit.EDirection direction)
    {
        GameObject adjacentRoom;
        Vector2Int pos = Vector2Int.zero;

        switch (direction)
        {
            case RoomExit.EDirection.North:
                pos = new Vector2Int(dictionaryPosition.x, dictionaryPosition.y + 1);
                break;
            case RoomExit.EDirection.East:
                pos = new Vector2Int(dictionaryPosition.x + 1, dictionaryPosition.y);
                break;
            case RoomExit.EDirection.South:
                pos = new Vector2Int(dictionaryPosition.x, dictionaryPosition.y - 1);
                break;
            case RoomExit.EDirection.West:
                pos = new Vector2Int(dictionaryPosition.x - 1, dictionaryPosition.y);
                break;
        }

        dungeon.TryGetValue(pos, out adjacentRoom);
        return adjacentRoom;
    }

    private RoomExit.EDirection OppositeDirection(RoomExit.EDirection direction)
    {
        switch (direction)
        {
            case RoomExit.EDirection.North:
                return RoomExit.EDirection.South;
            case RoomExit.EDirection.East:
                return RoomExit.EDirection.West;
            case RoomExit.EDirection.South:
                return RoomExit.EDirection.North;
            case RoomExit.EDirection.West:
                return RoomExit.EDirection.East;
        }

        return RoomExit.EDirection.North;
    }
}
