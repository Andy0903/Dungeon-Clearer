using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public enum EDirection
    {
        North,
        East,
        South,
        West,
    }

    [SerializeField]
    EDirection direction;

    public EDirection Direction { get { return direction; } }

    private void Awake()
    {
        Vector2 direction = Vector2.zero;
        switch (Direction)
        {
            case EDirection.North:
                direction = Vector2.up;
                break;
            case EDirection.East:
                direction = Vector2.right;
                break;
            case EDirection.South:
                direction = -Vector2.up;
                break;
            case EDirection.West:
                direction = -Vector2.right;
                break;
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 2f);
        if (hit.collider.tag == "DoorPoint")
        {
            Destroy(hit.collider.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldBuilder.Instance.SpawnRoom(transform, direction);
        Destroy(gameObject);
    }
}
