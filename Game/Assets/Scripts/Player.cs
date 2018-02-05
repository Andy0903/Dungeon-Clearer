using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private int speed;

    private Vector2 input;

	// Use this for initialization
	void Start ()
    {
        input = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleInput();
        Movement();
	}

    void Movement()
    {
        Vector3 movement = input * speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + movement, speed * Time.deltaTime);
    }

    void HandleInput()
    {
        input.x = joystick.Horizontal;
        input.y = joystick.Vertical;
    }
}
