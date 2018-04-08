using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public enum EAttackType
    {
        Physical,
        Electric,
        Fire,
        Water
    }

    private float timeInvincible;

    [SerializeField]
    private float MaxInvincibilityTime;

    public bool isAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }

    public bool isInvincible
    {
        get;
        private set;
    }

    private int currentHealth;

    [SerializeField]
    private int baseHealth = 100;

    int maxHealth;

    private const float HpIncreaseConstant = 0.85f;

	void Start ()
    {
        GameData ld = GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().LoadedData;
        maxHealth = baseHealth;
        maxHealth += (int)(ld.DungeonsCleared * HpIncreaseConstant + 0.5f);

        currentHealth = maxHealth;
    }
	
    void Update()
    {
        if (gameObject.tag == "Player" && SceneManager.GetActiveScene().buildIndex == 1)
        {
            maxHealth = baseHealth + GetComponent<Player>().Stats[Stats.EType.Health];
            currentHealth = maxHealth;
        }

        if(isInvincible)
        {
            timeInvincible += Time.deltaTime;

            if (timeInvincible > MaxInvincibilityTime)
            {
                isInvincible = false;
            }
        }
    }

    public void DealDamage(int amount, EAttackType type = EAttackType.Physical)
    {
        if (tag == "Player")
        {
            AudioManager.Instance.Play("PlayerHurt", true);
            amount -= CalculateResistanceDecrease(amount, type);
        }

        if (currentHealth < 0 && tag == "Enemy" || tag == "Destroyable")
        {
            GameObject.Find("Player").GetComponent<Player>().AddKilledEnemy();
            AudioManager.Instance.Play("Death", true);
            Destroy(gameObject);
        }
        currentHealth -= amount;
        Debug.Log(type + " damage " + amount + " dealt to " + tag + " hp is now: " + currentHealth);
    }

    private int CalculateResistanceDecrease(int amount, EAttackType type)
    {
        int res = GetComponent<Player>().Stats.GetResistance(type);
        if(res != 0 && amount/res > 0.8f)
        {
            return (int)(amount * 0.2f +1); //Will cap resistance at 80% and at least deal 20% of damage
        }
        else
        {
            return res;
        }
        
    }

    public void ActivateInvincibility()
    {
        isInvincible = true;
        timeInvincible = 0;
    }

    public void Heal(int amount)
    {
        //Makes sure to not overheal MaxHealth
        currentHealth += (maxHealth % currentHealth > amount) ? amount : maxHealth % currentHealth;
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        isInvincible = false;
        timeInvincible = 0;
    }

}
