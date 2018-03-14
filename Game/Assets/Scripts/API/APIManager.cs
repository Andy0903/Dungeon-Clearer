using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/*
    SunRise
    //Day
    SunSet
    CivilEnd
    NautEnd
    AstroEnd

    AstroBeg
    NautBeg
    CivilBeg
    //Night
*/

public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; private set; }
    public WeatherAPIClient Weather { get; private set; }
    public TimeAPIClient Time { get; private set; }

    void Awake()
    {
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
        float lat = 56f;     //Input.location.lastData.latitude;
        float lng = 13f;    //Input.location.lastData.longitude;

        Weather = new WeatherAPIClient(lat, lng);
        Time = new TimeAPIClient(lat, lng);

        Weather.AutoRefresh(this, 10);
        Time.Refresh(this);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log(Weather.Data.main.temp);
            Debug.Log(Time.ConvertDateTime(Time.Data.results.astronomical_twilight_begin));
        }
    }
}
