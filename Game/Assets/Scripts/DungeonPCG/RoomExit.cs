using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    EDirection direction;
    public bool CanSpawnRoom { get; set; }

    /// <summary>
    /// The set is used instead of Awake, never change direction of anything that wasn't spawned but a moment ago.
    /// </summary>
    public EDirection Direction
    {
        get
        {
            return direction;
        }
        set
        {
            CanSpawnRoom = true;
            direction = value;
            Vector2 dirVec = Vector2.zero;

            switch (value)
            {
                case EDirection.North:
                    transform.localPosition = new Vector3(0.5f, 6.5f, 0f);
                    dirVec = Vector2.up;
                    break;
                case EDirection.East:
                    transform.localPosition = new Vector3(11.5f, 0.5f, 0f);
                    dirVec = Vector2.right;
                    break;
                case EDirection.South:
                    transform.localPosition = new Vector3(0.5f, -5.5f, 0f);
                    dirVec = -Vector2.up;
                    break;
                case EDirection.West:
                    transform.localPosition = new Vector3(-10.5f, 0.5f, 0f);
                    dirVec = -Vector2.right;
                    break;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec, 2f);
            if (hit && hit.collider.tag == "DoorPoint" || hit && hit.transform.parent.tag == "DoorPoint")
            {
                RoomExit r;
                Quest q;
                if (hit.collider.tag == "DoorPoint")
                {
                    r = hit.collider.gameObject.GetComponent<RoomExit>();
                    q = GetComponentInChildren<Quest>();
                }
                else
                {
                    r = hit.transform.parent.gameObject.GetComponent<RoomExit>();
                    q = GetComponentInChildren<Quest>();
                }

                q.RemoveQuest();
                r.CanSpawnRoom = false;
                CanSpawnRoom = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (CanSpawnRoom)
            {
                WorldBuilder.Instance.SpawnRoom(transform, Direction);
            }
            CanSpawnRoom = false;
        }
    }
}
