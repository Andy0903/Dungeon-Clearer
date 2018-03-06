using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    class RoomTemplate
    {
        public GameObject RoomPrefab { get; private set; }
        public List<RoomExit.EDirection> Doors { get; private set; }

        public RoomTemplate(GameObject prefab, List<RoomExit.EDirection> doors)
        {
            RoomPrefab = prefab;
            Doors = doors;
        }
    }

    enum DoorStatus
    {
        Required,
        Optional,
        Forbidden,
    }

    public static WorldBuilder Instance { get; private set; }
    Dictionary<Vector2Int, GameObject> dungeon;

    //TODO make readonly
    List<RoomTemplate> templates = new List<RoomTemplate>();

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

        foreach (GameObject room in roomPrefabs)
        {
            templates.Add(new RoomTemplate(room, room.GetComponent<RoomInfo>().GetDoors()));
        }
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
        foreach (RoomExit.EDirection dir in Enum.GetValues(typeof(RoomExit.EDirection)))
        {
            GameObject newRoomNeighbor = FindAdjacentRoom(newRoomPosInDictionary, dir);
            if (newRoomNeighbor == null)
                doors[dir] = DoorStatus.Optional;
            else if (newRoomNeighbor.GetComponent<RoomInfo>().HasDoorAt(OppositeDirection(dir)))
                doors[dir] = DoorStatus.Required;
            else
                doors[dir] = DoorStatus.Forbidden;
        }

        GameObject go = AddRoom(newRoomPosInDictionary.x, newRoomPosInDictionary.y, FindAppropriateRoom(doors), pos.ToVector3IntOnGrid());
        DestoryUnnecessaryExits(go, direction);
    }

    private GameObject FindAppropriateRoom(Dictionary<RoomExit.EDirection, DoorStatus> doors)
    {
        List<RoomTemplate> candidates = new List<RoomTemplate>(templates);

        //Remove dead ends if not needed
        int allowedDoors = doors.Values.Where(d => d != DoorStatus.Forbidden).Select(t => t).Count();
        if (allowedDoors > 1)
        {
            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                if (candidates[i].Doors.Count == 1)
                {
                    candidates.RemoveAt(i);
                }
            }
        }

        foreach (RoomExit.EDirection dir in Enum.GetValues(typeof(RoomExit.EDirection)))
        {
            if (doors[dir] == DoorStatus.Optional)
            {
                continue;
            }

            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                if ((doors[dir] == DoorStatus.Forbidden && candidates[i].Doors.Contains(dir)) ||
                    (doors[dir] == DoorStatus.Required && !candidates[i].Doors.Contains(dir)))
                {
                    candidates.RemoveAt(i);
                }
            }
        }

        candidates.TrimExcess();
        return candidates[UnityEngine.Random.Range(0, candidates.Count)].RoomPrefab;
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
