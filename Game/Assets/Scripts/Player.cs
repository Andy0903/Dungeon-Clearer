using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private Joystick_Action joystickAction;
    [SerializeField]
    private int speed;



    private int attackRange = 3;
    private int attackDamage = 10;

    private Vector2 input;
    //Normalized direction based on input
    private Vector2 viewDirection;

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

    void TryDealDamage()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, viewDirection, attackRange);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().TakeDamage(attackDamage);
            }
        }
    }

    void HandleInput()
    {
        input.x = joystick.Horizontal;
        input.y = joystick.Vertical;

        HandleDirection(); //Sets viewDirection after input

        //Only lets you attack once per button press and when button is down
        if(joystickAction.isAttackPressed)
        {
            TryDealDamage();
        }
    }

    void HandleDirection()
    {
        if(input != Vector2.zero)
        {
            viewDirection = Vector2.zero;
            Vector2 unsignedVector = input;
            if(unsignedVector.x < 0)
            {
                unsignedVector.x *= -1;
            }
            if (unsignedVector.y < 0)
            {
                unsignedVector.y *= -1;
            }
            if(unsignedVector.y > unsignedVector.x)
            {
                viewDirection.y = input.normalized.y;
            }
            else 
            {
                viewDirection.x = input.normalized.x;
            }
        }
    }
}
