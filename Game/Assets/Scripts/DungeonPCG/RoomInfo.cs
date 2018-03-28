using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public Vector2Int DungeonPosition { get; set; }
    List<EDirection> doors = new List<EDirection>();

    public float MaxTemp { get { return maxTemp; } }
    public float MinTemp { get { return minTemp; } }

    GameObject cameraPoint;
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

    [SerializeField]
    GameObject LockPrefab;

    List<Vector3> doorPointPositions = new List<Vector3>();

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
        doorPointPositions.Add(go.transform.position);
    }

    public void Awake()
    {
        if (cameraPoint == null)
            cameraPoint = (GameObject)Resources.Load("RoomPoints/CameraPoint");
        if (doorPoint == null)
            doorPoint = (GameObject)Resources.Load("RoomPoints/DoorPoint");

        doors = new List<EDirection>();
        if (northDoor)
            InstantiateDoorPoint(EDirection.North);
        if (eastDoor)
            InstantiateDoorPoint(EDirection.East);
        if (southDoor)
            InstantiateDoorPoint(EDirection.South);
        if (westDoor)
            InstantiateDoorPoint(EDirection.West);

        GameObject.Instantiate(cameraPoint, transform);
    }

    public bool HasDoorAt(EDirection exitDirection)
    {
        return doors.Contains(exitDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Locked room collision with " + collision.tag);
        if (collision.tag == "Player")
        {
            LockRoom();
        }
    }

    private void LockRoom()
    {
        Debug.Log("Locked Room!");
        doorPointPositions.ForEach(p => GameObject.Instantiate(LockPrefab, p, Quaternion.identity, transform));
    }
}
