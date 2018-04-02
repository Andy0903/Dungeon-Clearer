using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

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
    private int MaxHealth = 100;

	void Start ()
    {
        currentHealth = MaxHealth;
	}
	
    void Update()
    {
        if(isInvincible)
        {
            timeInvincible += Time.deltaTime;

            if (timeInvincible > MaxInvincibilityTime)
            {
                isInvincible = false;
            }
        }
    }

    public void DealDamage(int amount)
    {
        currentHealth -= amount;

        if (tag == "Player")
            AudioManager.Instance.Play("PlayerHurt", true);

        if(currentHealth < 0 && tag == "Enemy" || tag == "Destroyable")
        {
            AudioManager.Instance.Play("Death", true);
            Destroy(gameObject);
        }

        Debug.Log("Damage dealt to " + tag + " hp is now: " + currentHealth);
    }

    public void ActivateInvincibility()
    {
        isInvincible = true;
        timeInvincible = 0;
    }

    public void Heal(int amount)
    {
        //Makes sure to not overheal MaxHealth
        currentHealth += (MaxHealth % currentHealth > amount) ? amount : MaxHealth % currentHealth;
    }

    public void Reset()
    {
        currentHealth = MaxHealth;
        isInvincible = false;
        timeInvincible = 0;
    }

}
