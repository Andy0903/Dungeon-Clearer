using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator attackAnimator;
    private float animationTimer;

    private const float ExitTime = 0.75f;

    private Joystick joystick;
    private Joystick_Action joystickAction;
    [SerializeField]
    private int speed;

    private int attackRange = 3;
    private int attackDamage = 10;
    private int attackKnockback = 50;

    private float timeSinceSpriteChange = 0;
    private const float SpriteIntervall = 0.15f;

    public Stats Stats { get; private set; }

    private Vector2 input;
    //Normalized direction based on input
    private Vector2 viewDirection;

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);
        Stats = new Stats();
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

            GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                transform.position = GameObject.FindGameObjectWithTag("CameraPoint").transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick == null || joystickAction == null)
            return;

        HandleInput();
        HandleSprite();
        HandleAnimation();
        Movement();

        
    }

    void HandleAnimation()
    {
        if (!attackAnimator.GetBool("Attacking"))
        {
            attackAnimator.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            if(animationTimer > ExitTime)
            {
                animationTimer = 0f;
                attackAnimator.SetBool("Attacking", false);
            }
            else
            {
                animationTimer += Time.deltaTime;
            }
        }
    }

    void Movement()
    {
        if (attackAnimator.GetBool("Attacking"))
        {
            Vector3 movement = input * speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + movement, speed * Time.deltaTime);
        }
    }

    void TryDealDamage()
    {
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, viewDirection, attackRange);
        //Just to doublecheck hits
        Debug.DrawRay(transform.position, viewDirection * attackRange);

        if(hits.Length > 0)
        {
            attackAnimator.SetBool("Attacking", true);
            attackAnimator.GetComponent<SpriteRenderer>().enabled = true;
        }

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                Debug.Log("We tried to Decrease HP");
                hit.collider.GetComponent<Health>().DealDamage(attackDamage);

                //TODO: Might wanna move this to the Health scripts DealDamage() function?
                
                Vector2 knockDirection = -(transform.position - hit.transform.position).normalized;
                hit.collider.GetComponent<Rigidbody2D>().AddForce(knockDirection * attackKnockback);
                Debug.Log("We tried to knockback");
            }
        }
    }

    void HandleSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (GetComponent<Health>().isInvincible)
        {
            timeSinceSpriteChange += Time.deltaTime;
            if (timeSinceSpriteChange > SpriteIntervall)
            {
                sr.enabled = sr.enabled == true ? false : true;
                timeSinceSpriteChange = 0;
            }
        }
        else if(!sr.enabled)
        {
            //Makes sure SR is enabled if we're no longer invincible
            sr.enabled = true;
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
