using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBehaviour : MonoBehaviour
{
    SpriteRenderer renderer;
    int maxWaitTime;
    int minWaitTime;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;

        StartCoroutine(LightningFlash());
    }

    public void Initialize(int minWaitTime, int maxWaitTime)
    {
        this.minWaitTime = minWaitTime;
        this.maxWaitTime = maxWaitTime;
    }

    IEnumerator LightningFlash()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            renderer.enabled = true;
            AudioManager.Instance.Play("Thunder", true);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            renderer.enabled = false;
        }
    }
}
