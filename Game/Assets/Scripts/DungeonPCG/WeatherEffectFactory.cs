using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffectFactory : MonoBehaviour
{
    [SerializeField]
    GameObject rainMistEffectPrefab;
    [SerializeField]
    GameObject lightningPrefab;
    [SerializeField]
    GameObject drizzleMistEffectPrefab;
    [SerializeField]
    GameObject snowParticleSystem;
    [SerializeField]
    GameObject[] lightPrefabs;
    [SerializeField]
    GameObject windArea;

    public void AddWeather(GameObject room)
    {
        string description = "extreme rain";//APIManager.Instance.Weather.Data.weather[0].description;

        if (Input.GetKey(KeyCode.K))
            description = "light rain";

        AddWind(room);

        switch (description)
        {
            case "thunderstorm with light rain":
                AddThunder(room, 2);
                AddRain(room, 1);
                break;
            case "thunderstorm with rain":
                AddThunder(room, 2);
                AddRain(room, 2);
                break;
            case "thunderstorm with heavy rain":
                AddThunder(room, 2);
                AddRain(room, 4);
                break;
            case "light thunderstorm":
                AddThunder(room, 1);
                break;
            case "thunderstorm":
                AddThunder(room, 2);
                break;
            case "heavy thunderstorm":
                AddThunder(room, 3);
                break;
            case "ragged thunderstorm":
                AddThunder(room, 3);
                break;
            case "thunderstorm with light drizzle":
                AddThunder(room, 2);
                AddDrizzle(room, 1);
                break;
            case "thunderstorm with drizzle":
                AddThunder(room, 2);
                AddDrizzle(room, 2);
                break;
            case "thunderstorm with heavy drizzle":
                AddThunder(room, 2);
                AddDrizzle(room, 3);
                break;

            case "light intensity drizzle":
                AddDrizzle(room, 1);
                break;
            case "drizzle":
                AddDrizzle(room, 2);
                break;
            case "heavy intensity drizzle":
                AddDrizzle(room, 3);
                break;
            case "light intensity drizzle rain":
                AddDrizzle(room, 1);
                AddRain(room, 1);
                break;
            case "drizzle rain":
                AddDrizzle(room, 2);
                AddRain(room, 2);
                break;
            case "heavy intensity drizzle rain":
                AddDrizzle(room, 3);
                AddRain(room, 2);
                break;
            case "shower rain and drizzle":
                AddDrizzle(room, 2);
                AddRain(room, 2);
                break;
            case "heavy shower rain and drizzle":
                AddDrizzle(room, 2);
                AddRain(room, 3);
                break;
            case "shower drizzle":
                AddDrizzle(room, 2);
                break;

            case "light rain":
                AddRain(room, 1);
                break;
            case "moderate rain":
                AddRain(room, 2);
                break;
            case "heavy intensity rain":
                AddRain(room, 3);
                break;
            case "very heavy rain":
                AddRain(room, 4);
                break;
            case "extreme rain":
                AddRain(room, 5);
                break;
            case "freezing rain":
                AddRain(room, 2);
                AddSnow(room, 1);
                break;
            case "light intensity shower rain":
                AddRain(room, 1);
                break;
            case "shower rain":
                AddRain(room, 2);
                break;
            case "heavy intensity shower rain":
                AddRain(room, 3);
                break;
            case "ragged shower rain":
                AddRain(room, 4);
                break;

            case "light snow":
                AddSnow(room, 1);
                break;
            case "snow":
                AddSnow(room, 2);
                break;
            case "heavy snow":
                AddSnow(room, 3);
                break;
            case "sleet":
                AddRain(room, 1);
                AddSnow(room, 1);
                break;
            case "shower sleet":
                AddRain(room, 1);
                AddSnow(room, 1);
                break;
            case "light rain and snow":
                AddRain(room, 1);
                AddSnow(room, 2);
                break;
            case "rain and snow":
                AddRain(room, 2);
                AddSnow(room, 2);
                break;
            case "light shower snow":
                AddSnow(room, 1);
                break;
            case "shower snow":
                AddSnow(room, 2);
                break;
            case "heavy shower snow":
                AddSnow(room, 3);
                break;

            case "mist":
                AddClouds(room, 2);
                break;
            case "smoke": break;
            case "haze": break;
            case "sand, dust whirls": break;
            case "fog":
                AddClouds(room, 3);
                break;
            case "sand": break;
            case "dust": break;
            case "volcanic ash": break;
            case "squalls": break;
            case "tornado": break;

            case "clear sky":
                AddClear(room);
                break;

            case "few clouds":
                AddClouds(room, 1);
                break;
            case "scattered clouds":
                AddClouds(room, 2);
                break;
            case "broken clouds":
                AddClouds(room, 3);
                break;
            case "overcast clouds":
                AddClouds(room, 4);
                break;

            //case "tornado":
            case "tropical storm": break;
            case "hurricane": break;
            case "cold": break;
            case "hot": break;
            case "windy": break;
            case "hail": break;

            case "calm": break;
            case "light breeze": break;
            case "gentle breeze": break;
            case "moderate breeze": break;
            case "fresh breeze": break;
            case "strong breeze": break;
            case "high wind, near gale": break;
            case "gale": break;
            case "severe gale": break;
            case "storm": break;
            case "violent storm": break;
                //case "hurricane":

        }
    }

    GameObject AddWind(GameObject room)
    {
        GameObject go = GameObject.Instantiate(windArea, room.transform, false);
        WindArea wa = go.GetComponent<WindArea>();

        float windSpeed = APIManager.Instance.Weather.Data.wind.speed;
        wa.Initialize(windSpeed * 15f, 4, 10);
        return go;
    }

    GameObject AddClear(GameObject room)
    {
        return GameObject.Instantiate(lightPrefabs[UnityEngine.Random.Range(0, lightPrefabs.Length - 1)], room.transform, false);
    }

    GameObject AddSnow(GameObject room, int intensity)
    {
        GameObject go = GameObject.Instantiate(snowParticleSystem, room.transform, false);
        int rate = 0;

        switch (intensity)
        {
            case 1: rate = 1; break;
            case 2: rate = 2; break;
            case 3: rate = 3; break;
            case 4: rate = 6; break;
            default: Debug.Log("Faulty intensity given"); break;
        }

        ParticleSystem[] ps = go.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in ps)
        {
            var emission = p.emission;
            emission.rateOverTime = rate;
        }

        return go;
    }

    GameObject AddDrizzle(GameObject room, int intensity)
    {
        GameObject go = GameObject.Instantiate(drizzleMistEffectPrefab, room.transform, false);
        RainScript2D d = go.GetComponent<RainScript2D>();

        switch (intensity)
        {
            case 1: d.RainIntensity = 0.024f; break;
            case 2: d.RainIntensity = 0.1f; break;
            case 3: d.RainIntensity = 0.4f; break;
            default: Debug.Log("Faulty intensity given"); break;
        }

        return go;
    }

    GameObject AddThunder(GameObject room, int intensity)
    {
        GameObject go = GameObject.Instantiate(lightningPrefab, room.transform, false);
        LightningBehaviour l = go.GetComponent<LightningBehaviour>();

        switch (intensity)
        {
            case 1: l.Initialize(10, 60); break;
            case 2: l.Initialize(10, 30); break;
            case 3: l.Initialize(10, 20); break;
            default: Debug.Log("Faulty intensity given"); break;
        }

        return go;
    }

    GameObject AddClouds(GameObject room, int intensity)
    {
        GameObject go = GameObject.Instantiate(rainMistEffectPrefab, room.transform, false);
        RainScript2D r = go.GetComponent<RainScript2D>();

        switch (intensity)
        {
            case 1: r.MistIntensity = 0.2f; break;
            case 2: r.MistIntensity = 0.4f; break;
            case 3: r.MistIntensity = 0.6f; break;
            case 4: r.MistIntensity = 0.8f; break;
            default: Debug.Log("Faulty intensity given"); break;
        }

        return go;
    }

    GameObject AddRain(GameObject room, int intensity)
    {
        GameObject go = GameObject.Instantiate(rainMistEffectPrefab, room.transform, false);
        RainScript2D r = go.GetComponent<RainScript2D>();

        switch (intensity)
        {
            case 1: r.RainIntensity = 0.024f; break;
            case 2: r.RainIntensity = 0.1f; break;
            case 3: r.RainIntensity = 0.4f; break;
            case 4: r.RainIntensity = 0.6f; break;
            case 5: r.RainIntensity = 1f; break;
            default: Debug.Log("Faulty intensity given"); break;
        }

        return go;
    }
}
