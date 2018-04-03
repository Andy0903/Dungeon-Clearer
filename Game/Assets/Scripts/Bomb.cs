using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField]
    private float radius;
    [SerializeField]
    private float timeToExplode;
    [SerializeField]
    private int damage;

    private bool hasExploded;
    private float timer;
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer > timeToExplode && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
	}

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        //TODO: Play a bomb animation, delete bomb after it has finished

        foreach (Collider2D hit in hits)
        {
            Health hp = hit.gameObject.GetComponent<Health>(); //Returns null if component isn't there
            if(hp != null) 
            {
                hp.DealDamage(damage);
            }
        }
    }
}
