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
        if(transform.childCount == 0)
        {
            GameObject.Instantiate(roomPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        }
    }

    public void SpawnRoom(Transform trans, RoomExit.EDirection direction)
    {
        if (direction == RoomExit.EDirection.East)
        {
            RoomExit[] doorPoints = trans.parent.GetComponentsInChildren<RoomExit>();

            foreach (RoomExit exit in doorPoints)
            {
                if (exit.Direction != RoomExit.EDirection.East)
                    continue;
                
                Vector3 pos = new Vector3(trans.position.x, trans.position.y - 1, trans.position.z) + exit.transform.localPosition;
                GameObject go = GameObject.Instantiate(roomPrefabs[0], pos.ToVector3IntOnGrid(), Quaternion.identity, gameObject.transform);

                RoomExit[] newDoorPoints = go.GetComponentsInChildren<RoomExit>();
                foreach (RoomExit newExit in newDoorPoints)
                {
                    if (newExit.Direction == RoomExit.EDirection.West)
                    {
                        Destroy(newExit.gameObject);
                    }
                }
            }
        }

        if (direction == RoomExit.EDirection.West)
        {
            RoomExit[] doorPoints = trans.parent.GetComponentsInChildren<RoomExit>();

            foreach (RoomExit exit in doorPoints)
            {
                if (exit.Direction != RoomExit.EDirection.West)
                    continue;

                Vector3 pos = new Vector3(trans.position.x - 2, trans.position.y - 1, trans.position.z) + exit.transform.localPosition;
                GameObject go = GameObject.Instantiate(roomPrefabs[0], pos.ToVector3IntOnGrid(), Quaternion.identity, gameObject.transform);

                RoomExit[] newDoorPoints = go.GetComponentsInChildren<RoomExit>();
                foreach (RoomExit newExit in newDoorPoints)
                {
                    if (newExit.Direction == RoomExit.EDirection.East)
                    {
                        Destroy(newExit.gameObject);
                    }
                }
            }
        }

        if (direction == RoomExit.EDirection.North)
        {
            RoomExit[] doorPoints = trans.parent.GetComponentsInChildren<RoomExit>();

            foreach (RoomExit exit in doorPoints)
            {
                if (exit.Direction != RoomExit.EDirection.North)
                    continue;

                Vector3 pos = new Vector3(trans.position.x - 1, trans.position.y, trans.position.z) + exit.transform.localPosition;
                GameObject go = GameObject.Instantiate(roomPrefabs[0], pos.ToVector3IntOnGrid(), Quaternion.identity, gameObject.transform);

                RoomExit[] newDoorPoints = go.GetComponentsInChildren<RoomExit>();
                foreach (RoomExit newExit in newDoorPoints)
                {
                    if (newExit.Direction == RoomExit.EDirection.South)
                    {
                        Destroy(newExit.gameObject);
                    }
                }
            }
        }


        if (direction == RoomExit.EDirection.South)
        {
            RoomExit[] doorPoints = trans.parent.GetComponentsInChildren<RoomExit>();

            foreach (RoomExit exit in doorPoints)
            {
                if (exit.Direction != RoomExit.EDirection.South)
                    continue;

                Vector3 pos = new Vector3(trans.position.x - 1, trans.position.y - 2, trans.position.z) + exit.transform.localPosition;
                GameObject go = GameObject.Instantiate(roomPrefabs[0], pos.ToVector3IntOnGrid(), Quaternion.identity, gameObject.transform);

                RoomExit[] newDoorPoints = go.GetComponentsInChildren<RoomExit>();
                foreach (RoomExit newExit in newDoorPoints)
                {
                    if (newExit.Direction == RoomExit.EDirection.North)
                    {
                        Destroy(newExit.gameObject);
                    }
                }
            }
        }
    }
}
