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

        Input.location.Start(1, 0.1f); //random values (of accuracy)

        float lat;
        float lng;
#if UNITY_EDITOR
        lat = 56f;     //Input.location.lastData.latitude;
        lng = 13f;    //Input.location.lastData.longitude;
#else
        lat = Input.location.lastData.latitude;
        lng = Input.location.lastData.longitude;
#endif

        Weather = new WeatherAPIClient(lat, lng);
        Time = new TimeAPIClient(lat, lng);

        Weather.AutoRefresh(this, 10);
        Time.Refresh(this);
    }

    private void Update()
    {
        if (Weather.Data != null && Time.Data != null)
        {
            Ready = true;
            enabled = false;
        }
    }
}
