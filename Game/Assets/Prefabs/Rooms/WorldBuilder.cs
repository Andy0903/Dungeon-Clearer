using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    [SerializeField]
    GameObject[] roomPrefabs;

    private void Start()
    {
        GameObject.Instantiate(roomPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        GameObject.Instantiate(roomPrefabs[1], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        GameObject.Instantiate(roomPrefabs[2], new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
    }
}
