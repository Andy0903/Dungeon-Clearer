using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int enemiesKilledInDungeon;

    [SerializeField]
    private Animator attackAnimator;
    private float animationTimer;

    private float ExitTime = 0.5f;

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
    public int EnemiesKilled { get { return enemiesKilledInDungeon; } }

    private Vector2 input;
    //Normalized direction based on input
    private Vector2 viewDirection;

    private SaveLoadManager loadedInformation;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        Stats = new Stats();
        input = Vector2.zero;
        enemiesKilledInDungeon = 0;
        loadedInformation = GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>();
        ChangeTint(loadedInformation.PercentageKilled);
        
        //The exit time from this runs 1/3 of the clip again?
        //ExitTime = attackAnimator.runtimeAnimatorController.animationClips[0].length;
    }

    public void Reset()
    {
        GetComponent<Health>().Reset();
        enemiesKilledInDungeon = 0;
        ChangeTint(loadedInformation.PercentageKilled);
    }

    public void AddKilledEnemy() { enemiesKilledInDungeon++; }

    void ChangeTint(float percentage)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //Percentage is multiplied by 2 to change the ratios from 0-100% in both cases instead of 0-50%
        if(percentage > 0.5f)
        {
            sr.color = Color.Lerp(Color.white, Color.red, percentage * 2);
        }
        else
        {
            sr.color = Color.Lerp(Color.blue, Color.white, percentage * 2);
        }
        
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

    void Movement()
    {
        if (!attackAnimator.GetBool("Attacking"))
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

        AudioManager.Instance.Play("Slash", true);
        attackAnimator.SetBool("Attacking", true);
        attackAnimator.GetComponent<SpriteRenderer>().enabled = true;
        attackAnimator.GetComponent<Transform>().localPosition = viewDirection;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null) //&& hit.collider.tag == "Enemy"
            {
                Health hp = hit.collider.GetComponent<Health>();
                if (hp != null)
                {
                    hit.collider.GetComponent<Health>().DealDamage(attackDamage);
                }

                if (hit.collider.tag == "Enemy") //We only want to knockback enemies
                {
                    Vector2 knockDirection = -(transform.position - hit.transform.position).normalized;
                    hit.collider.GetComponent<Rigidbody2D>().AddForce(knockDirection * attackKnockback);
                }
                else if (hit.collider.tag == "Interactive")
                {
                    hit.collider.GetComponent<InteractiveObject>().RegisterHit();
                }
            }
        }
    }

    void HandleAnimation()
    {
        if (!attackAnimator.GetBool("Attacking"))
        {
            attackAnimator.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            if (animationTimer > ExitTime)
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
        else if (!sr.enabled)
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
