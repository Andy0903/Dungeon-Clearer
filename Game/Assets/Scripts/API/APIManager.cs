using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; private set; }
    public WeatherAPIClient Weather { get; private set; }
    public TimeAPIClient Time { get; private set; }
    public bool Ready { get; private set; }

    void Awake()
    {
        Ready = false;
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
            yield break;
        Input.location.Start(); //(1, 0.1f); //random values (of accuracy)

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("No GPS");
            yield break;
        }
        else
        {
            float lat;
            float lng;
            lat = Input.location.lastData.latitude;
            lng = Input.location.lastData.longitude;

            Weather = new WeatherAPIClient(lat, lng);
            Time = new TimeAPIClient(lat, lng);

            Weather.AutoRefresh(this, 10);
            Time.Refresh(this);
        }
    }

    private void Update()
    {
        if (Weather != null && Time != null)
        {
            if (Weather.Data != null && Time.Data != null)
            {
                Ready = true;
                enabled = false;
            }
        }
    }
}
