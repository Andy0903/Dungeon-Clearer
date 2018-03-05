using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public static WorldBuilder Instance { get; private set; }

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
    }

    private void Start()
    {
        if (transform.childCount == 0)
        {
            GameObject.Instantiate(roomPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        }
    }

    public void SpawnRoom(Transform trigger, RoomExit.EDirection direction)
    {
        RoomExit[] doorPoints = trigger.parent.GetComponentsInChildren<RoomExit>();
        int offsetX = 0;
        int offsetY = 0;

        switch (direction)
        {
            case RoomExit.EDirection.North: offsetX = -1; offsetY = 0; break;
            case RoomExit.EDirection.East: offsetX = 0; offsetY = -1; break;
            case RoomExit.EDirection.South: offsetX = -1; offsetY = -2; break;
            case RoomExit.EDirection.West: offsetX = -2; offsetY = -1; break;
        }

        foreach (RoomExit exit in doorPoints)
        {
            if (exit.Direction != direction)
                continue;

            Vector3 pos = new Vector3(trigger.position.x + offsetX, trigger.position.y + offsetY, trigger.position.z) + exit.transform.localPosition;
            GameObject go = GameObject.Instantiate(roomPrefabs[0], pos.ToVector3IntOnGrid(), Quaternion.identity, gameObject.transform);
            RoomExit[] newDoorPoints = go.GetComponentsInChildren<RoomExit>();

            foreach (RoomExit newExit in newDoorPoints)
            {
                if (newExit.Direction == Opposite(direction))
                {
                    Destroy(newExit.gameObject);
                }
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
