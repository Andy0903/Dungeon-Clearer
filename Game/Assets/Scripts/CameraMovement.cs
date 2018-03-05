using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    const float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

    Transform player;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnPoint");

        float distance = float.PositiveInfinity;
        Vector3 bestPos = Vector3.zero;
        foreach (GameObject g in points)
        {
            Vector3 pos = g.transform.position;
            float newDist = Vector3.Distance(player.position, pos);

            if (distance > newDist)
            {
                distance = newDist;
                bestPos = new Vector3(pos.x, pos.y, -10f);
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, bestPos, ref velocity, smoothTime);
    }
}
