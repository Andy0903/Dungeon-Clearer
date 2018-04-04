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

    public TimeEffectFactory.DayPhase Time { get; set; }

    [SerializeField]
    float maxTemp;
    [SerializeField]
    float minTemp;

    [SerializeField]
    bool northDoor;
    [SerializeField]
    Quest.Type northQuest = Quest.Type.None;
    [SerializeField]
    bool eastDoor;
    [SerializeField]
    Quest.Type eastQuest = Quest.Type.None;
    [SerializeField]
    bool southDoor;
    [SerializeField]
    Quest.Type southQuest = Quest.Type.None;
    [SerializeField]
    bool westDoor;
    [SerializeField]
    Quest.Type westQuest = Quest.Type.None;

    [SerializeField]
    GameObject LockPrefab;

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

    private void InstantiateDoorPoint(EDirection direction, Quest.Type questType)
    {
        if (!doors.Contains(direction))
            doors.Add(direction);

        GameObject go = GameObject.Instantiate(doorPoint, transform);
        go.GetComponent<RoomExit>().Direction = direction;
        go.GetComponentInChildren<Quest>().type = questType;
    }

    public void Awake()
    {
        if (cameraPoint == null)
            cameraPoint = (GameObject)Resources.Load("RoomPoints/CameraPoint");
        if (doorPoint == null)
            doorPoint = (GameObject)Resources.Load("RoomPoints/DoorPoint");

        doors = new List<EDirection>();
        if (northDoor)
            InstantiateDoorPoint(EDirection.North, northQuest);
        if (eastDoor)
            InstantiateDoorPoint(EDirection.East, eastQuest);
        if (southDoor)
            InstantiateDoorPoint(EDirection.South, southQuest);
        if (westDoor)
            InstantiateDoorPoint(EDirection.West, westQuest);

        GameObject.Instantiate(cameraPoint, transform);
    }

    public bool HasDoorAt(EDirection exitDirection)
    {
        return doors.Contains(exitDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LockRoom();
        }
    }

    private void LockRoom()
    {
        foreach (RoomExit exit in GetComponentsInChildren<RoomExit>())
        {
            GameObject.Instantiate(LockPrefab, exit.transform);
            AudioManager.Instance.Play("Fire");
        }
    }
}
