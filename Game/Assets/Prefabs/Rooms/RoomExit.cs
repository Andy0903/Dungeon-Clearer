using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldBuilder.Instance.SpawnRoom(transform);
        Destroy(gameObject.GetComponent<Collider2D>());
    }
}
