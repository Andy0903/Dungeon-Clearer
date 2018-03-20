using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    const float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

    Transform player;
    int currentPointIndex;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentPointIndex = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnPoint");

        float distance = float.PositiveInfinity;
        Vector3 bestPos = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 pos = points[i].transform.position;
            float newDist = Vector3.Distance(player.position, pos);

            if (distance > newDist)
            {
                distance = newDist;
                bestPos = new Vector3(pos.x, pos.y, -10f);
                points[currentPointIndex].transform.parent.tag = "Untagged";
                points[i].transform.parent.tag = "BackgroundLayer";
                currentPointIndex = i;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, bestPos, ref velocity, smoothTime);
    }
}
