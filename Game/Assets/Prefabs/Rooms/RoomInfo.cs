using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public Vector2Int DungeonPosition { get; set; }
    List<EDirection> doors = new List<EDirection>();

    public float MaxTemp { get { return maxTemp; } }
    public float MinTemp { get { return minTemp; } }

    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    GameObject doorPoint;

    [SerializeField]
    float maxTemp;
    [SerializeField]
    float minTemp;

    [SerializeField]
    bool northDoor;
    [SerializeField]
    bool eastDoor;
    [SerializeField]
    bool southDoor;
    [SerializeField]
    bool westDoor;

    public List<EDirection> GetDoors()
    {
        if (northDoor && !doors.Contains(EDirection.North))
            doors.Add(EDirection.North);
        if (eastDoor && !doors.Contains(EDirection.East))
            doors.Add(EDirection.East);
        if (southDoor && !doors.Contains(EDirection.South))
            doors.Add(EDirection.South);
        if (westDoor && !doors.Contains(EDirection.West))
            doors.Add(EDirection.West);

        return doors;
    }

    private void InstantiateDoorPoint(EDirection direction)
    {
        if (!doors.Contains(direction))
            doors.Add(direction);

        GameObject go = GameObject.Instantiate(doorPoint, transform);
        go.GetComponent<RoomExit>().Direction = direction;
    }

    public void Awake()
    {
        Debug.Log("Awake in RoomInfo");
        doors = new List<EDirection>();
        if (northDoor)
            InstantiateDoorPoint(EDirection.North);
        if (eastDoor)
            InstantiateDoorPoint(EDirection.East);
        if (southDoor)
            InstantiateDoorPoint(EDirection.South);
        if (westDoor)
            InstantiateDoorPoint(EDirection.West);
        
        GameObject.Instantiate(spawnPoint, transform);
    }

    public bool HasDoorAt(EDirection exitDirection)
    {
        return doors.Contains(exitDirection);
    }
}
