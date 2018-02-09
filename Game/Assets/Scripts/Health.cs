using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public bool isAlive
    {
        get
        {
            return currentHealth > 0;
        }
    }

    private int currentHealth;

    [SerializeField]
    private int MaxHealth = 100;

	void Start ()
    {
        currentHealth = MaxHealth;
	}
	
    public void DealDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Damage dealt to " + tag + " hp is now: " + currentHealth);
    }

    public void Heal(int amount)
    {
        //Makes sure to not overheal MaxHealth
        currentHealth += (MaxHealth % currentHealth > amount) ? amount : MaxHealth % currentHealth;
    }

}
