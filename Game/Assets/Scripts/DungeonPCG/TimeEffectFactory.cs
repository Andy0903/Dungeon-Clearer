using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffectFactory : MonoBehaviour
{
    public enum DayPhase
    {
        AstronomicalDawn,
        NauticalDawn,
        CivilDawn,
        Sunrise,
        SolarNoon,
        Sunset,
        CivilDusk,
        NauticalDusk,
        AstronomicalDusk,
    }

    Dictionary<DayPhase, DateTime> times;
    Dictionary<DayPhase, float> alphaValues;

    [SerializeField]
    GameObject effectPrefab;

    public void Awake()
    {
        times = new Dictionary<DayPhase, DateTime>()
        {
             { DayPhase.AstronomicalDawn, APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.astronomical_twilight_begin) },
             { DayPhase.NauticalDawn,     APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.nautical_twilight_begin)     },
             { DayPhase.CivilDawn,        APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.civil_twilight_begin)        },
             { DayPhase.Sunrise,          APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.sunrise)                     },
             { DayPhase.SolarNoon,        APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.solar_noon)                  },
             { DayPhase.Sunset,           APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.sunset)                      },
             { DayPhase.CivilDusk,        APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.civil_twilight_end)          },
             { DayPhase.NauticalDusk,     APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.nautical_twilight_end)       },
             { DayPhase.AstronomicalDusk, APIManager.Instance.Time.ConvertDateTime(APIManager.Instance.Time.Data.results.astronomical_twilight_end)   },
        };

        alphaValues = new Dictionary<DayPhase, float>()
        {
                { DayPhase.AstronomicalDawn, .5f },
                { DayPhase.NauticalDawn    , .4f },
                { DayPhase.CivilDawn       , .2f },
                { DayPhase.Sunrise         , .1f },
                { DayPhase.SolarNoon       , .0f },
                { DayPhase.Sunset          , .2f },
                { DayPhase.CivilDusk       , .4f },
                { DayPhase.NauticalDusk    , .5f },
                { DayPhase.AstronomicalDusk, .6f },
        };
    }

    public GameObject AddTimeFilter(GameObject room)
    {
        bool biggerThanAll = true;
        foreach (DayPhase p in Enum.GetValues(typeof(DayPhase)))
        {
            DateTime currentTime = DateTime.Now;

            if (currentTime >= times[p])
            {
                Color c = effectPrefab.GetComponent<SpriteRenderer>().color;
                c.a = alphaValues[p];
                effectPrefab.GetComponent<SpriteRenderer>().color = c;
                room.GetComponent<RoomInfo>().Time = p;
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
