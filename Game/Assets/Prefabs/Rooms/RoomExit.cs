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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldBuilder.Instance.SpawnRoom(transform, direction);
        Destroy(gameObject.GetComponent<Collider2D>());
    }
}
