using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Joystick joystick;
    private Joystick_Action joystickAction;
    [SerializeField]
    private int speed;
    
    private int attackRange = 3;
    private int attackDamage = 10;
    Stats stats;

    private Vector2 input;
    //Normalized direction based on input
    private Vector2 viewDirection;

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);
        stats = new Stats();
        input = Vector2.zero;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 1)
        {
            joystick = GameObject.FindObjectOfType<Joystick>();
            joystickAction = GameObject.FindObjectOfType<Joystick_Action>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick == null || joystickAction == null)
            return;

        HandleInput();
        Movement();

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Strength " + Strength);
            //Debug.Log("Vitality " + Vitality);
        }
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
                hit.collider.GetComponent<Health>().DealDamage(attackDamage);
            }
        }
    }

    void HandleInput()
    {
        input.x = joystick.Horizontal;
        input.y = joystick.Vertical;

        HandleDirection(); //Sets viewDirection after input

        //Only lets you attack once per button press and when button is down
        if (joystickAction.isAttackPressed)
        {
            TryDealDamage();
        }
    }

    void HandleDirection()
    {
        if (input != Vector2.zero)
        {
            viewDirection = Vector2.zero;
            Vector2 unsignedVector = input;
            if (unsignedVector.x < 0)
            {
                unsignedVector.x *= -1;
            }
            if (unsignedVector.y < 0)
            {
                unsignedVector.y *= -1;
            }
            if (unsignedVector.y > unsignedVector.x)
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
