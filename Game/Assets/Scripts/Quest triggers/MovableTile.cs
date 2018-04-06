using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : MonoBehaviour
{
    private GameObject tile;

    public bool isPlaced
    {
        get;
        private set;
    }

	void Start ()
    {
        tile = transform.GetChild(0).gameObject;
        isPlaced = false;
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == tile)
        {
            isPlaced = true;
            tile.transform.localPosition = Vector2.zero;
            tile.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
}
