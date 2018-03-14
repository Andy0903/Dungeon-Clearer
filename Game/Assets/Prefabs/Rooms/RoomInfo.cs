using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public Vector2Int DungeonPosition { get; set; }
    Dictionary<EDirection, bool> doorsExist;

    public float MaxTemp { get { return maxTemp; } }
    public float MinTemp { get { return minTemp; } }

    [SerializeField]
    float maxTemp;
    [SerializeField]
    float minTemp;

    public List<EDirection> GetDoors()
    {
        List<EDirection> doors = new List<EDirection>();
        foreach (RoomExit exit in GetComponentsInChildren<RoomExit>())
        {
            doors.Add(exit.Direction);
        }

        return doors;
    }

    public void Start()
    {
        doorsExist = new Dictionary<EDirection, bool>();

        RoomExit[] exits = GetComponentsInChildren<RoomExit>();
        foreach (RoomExit exit in exits)
        {
            doorsExist[exit.Direction] = true;
        }
    }

    public bool HasDoorAt(EDirection exitDirection)
    {
        bool exists = false;
        doorsExist.TryGetValue(exitDirection, out exists);
        return exists;
    }
}
