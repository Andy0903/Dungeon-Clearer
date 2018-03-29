using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffectFactory : MonoBehaviour
{
    List<DateTime> times;
    List<float> alphaValues;

    [SerializeField]
    GameObject effectPrefab;

    public void Awake()
    {
        times = new List<DateTime>();
        alphaValues = new List<float>();

        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.astronomical_twilight_begin));
        alphaValues.Add(0.5f);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.nautical_twilight_begin));
        alphaValues.Add(0.4f);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.civil_twilight_begin));
        alphaValues.Add(0.2f);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.sunrise));
        alphaValues.Add(0.1f);
        //Day
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.solar_noon));
        alphaValues.Add(0);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.sunset));
        alphaValues.Add(0.2f);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.civil_twilight_end));
        alphaValues.Add(0.4f);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.nautical_twilight_end));
        alphaValues.Add(0.5f);
        times.Add(APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.astronomical_twilight_end));
        alphaValues.Add(0.6f);
        //Night
    }

    public GameObject AddTimeFilter(GameObject room)
    {
        DateTime currentTime = DateTime.Now;
        bool biggerThanAll = true;
        for (int i = 0; i < times.Count; i++)
        {
            if (currentTime >= times[i])
            {
                Color c = effectPrefab.GetComponent<SpriteRenderer>().color;
                c.a = alphaValues[i];
                effectPrefab.GetComponent<SpriteRenderer>().color = c;
            }
            else
            {
                biggerThanAll = false;
            }
        }

        if (biggerThanAll)
        {
            APIManager.Instance.Time.Refresh(this);
            Awake();
        }

        return GameObject.Instantiate(effectPrefab, room.transform, false);
    }
}
