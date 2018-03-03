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
}
