using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    EDirection direction;

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
            if (hit && hit.collider.tag == "DoorPoint")
            {
                Destroy(hit.collider.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldBuilder.Instance.SpawnRoom(transform, Direction);
        Destroy(gameObject);
    }
}
