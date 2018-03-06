using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public Vector2Int DungeonPosition { get; set; }
    Dictionary<RoomExit.EDirection, bool> doorsExist;

    public List<RoomExit.EDirection> GetDoors()
    {
        List<RoomExit.EDirection> doors = new List<RoomExit.EDirection>();
        foreach (RoomExit exit in GetComponentsInChildren<RoomExit>())
        {
            doors.Add(exit.Direction);
        }

        return doors;
    }

    public void Start()
    {
        doorsExist = new Dictionary<RoomExit.EDirection, bool>();

        RoomExit[] exits = GetComponentsInChildren<RoomExit>();
        foreach (RoomExit exit in exits)
        {
            doorsExist[exit.Direction] = true;
        }
    }

    public bool HasDoorAt(RoomExit.EDirection exitDirection)
    {
        bool exists = false;
        doorsExist.TryGetValue(exitDirection, out exists);
        return exists;
    }
}
