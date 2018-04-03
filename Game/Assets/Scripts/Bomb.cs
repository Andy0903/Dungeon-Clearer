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

    private Animator animator;
    private bool hasExploded;
    private float timer;
	
    void Start()
    {
        animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer > timeToExplode && !hasExploded)
        {
            Explode();
            hasExploded = true; 
        }

        //Once animation has reached exit state we delete the object
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
        {
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        animator.SetTrigger("Explode");

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
