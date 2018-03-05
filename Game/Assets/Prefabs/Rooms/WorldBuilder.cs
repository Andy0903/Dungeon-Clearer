using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
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
        Vector2Int dungeonPos = Vector2Int.zero;
        RoomExit[] doorPoints = trigger.parent.GetComponentsInChildren<RoomExit>();
        int offsetX = 0;
        int offsetY = 0;

        switch (direction)
        {
            case RoomExit.EDirection.North:
                offsetX = -1;
                offsetY = 0;
                dungeonPos = new Vector2Int(parentDungeonPos.x, parentDungeonPos.y + 1);
                break;
            case RoomExit.EDirection.East:
                offsetX = 0;
                offsetY = -1;
                dungeonPos = new Vector2Int(parentDungeonPos.x + 1, parentDungeonPos.y);
                break;
            case RoomExit.EDirection.South:
                offsetX = -1;
                offsetY = -2;
                dungeonPos = new Vector2Int(parentDungeonPos.x, parentDungeonPos.y - 1);
                break;
            case RoomExit.EDirection.West:
                offsetX = -2;
                offsetY = -1;
                dungeonPos = new Vector2Int(parentDungeonPos.x - 1, parentDungeonPos.y);
                break;
        }

        Vector3 pos = new Vector3(trigger.position.x + offsetX, trigger.position.y + offsetY, trigger.position.z) + trigger.transform.localPosition;

        //Check if there are neighboring rooms with doors etc and choose another prefab.

        GameObject go = AddRoom(dungeonPos.x, dungeonPos.y, roomPrefabs[0], pos.ToVector3IntOnGrid());

        RoomExit[] newDoorPoints = go.GetComponentsInChildren<RoomExit>();
        foreach (RoomExit newExit in newDoorPoints)
        {
            if (newExit.Direction == Opposite(direction))
            {
                Destroy(newExit.gameObject);
            }
        }
    }

    private RoomExit.EDirection Opposite(RoomExit.EDirection direction)
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
