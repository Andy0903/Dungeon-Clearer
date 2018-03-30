using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    float force;
    List<Rigidbody2D> bodies;
    ParticleSystem ps;

    private void Awake()
    {
        bodies = new List<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
    }

    public void Initialize(float force, int repeatMin, int repeatMax)
    {
        this.force = force;
        InvokeRepeating("WindPush", Random.Range(repeatMin, repeatMax), Random.Range(repeatMin, repeatMax));
    }

    void WindPush()
    {
        ps.Play();
        foreach (Rigidbody2D rb in bodies)
        {
            if (rb != null)
                rb.AddForce(force * rb.transform.right, ForceMode2D.Impulse);
        }
        bodies.TrimExcess();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            bodies.Add(rb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null && bodies.Contains(rb))
        {
            bodies.Remove(rb);
        }
    }
}
