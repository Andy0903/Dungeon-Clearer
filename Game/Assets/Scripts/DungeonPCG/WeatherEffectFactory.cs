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

    public Health.EAttackType AttackType
    {
        get;
        private set;
    }

    public void AddWeather(GameObject room)
    {
        string description = "heavy thunderstorm";//APIManager.Instance.Weather.Data.weather[0].description;

        AddWind(room);

        switch (description)
        {
            case "thunderstorm with light rain":
                AddThunder(room, 2);
                AddRain(room, 1);
                AttackType = Health.EAttackType.Electric;
                break;
            case "thunderstorm with rain":
                AddThunder(room, 2);
                AddRain(room, 2);
                AttackType = Health.EAttackType.Electric;
                break;
            case "thunderstorm with heavy rain":
                AddThunder(room, 2);
                AddRain(room, 4);
                AttackType = Health.EAttackType.Electric;
                break;
            case "light thunderstorm":
                AddThunder(room, 1);
                AddRain(room, 1);
                AttackType = Health.EAttackType.Electric;
                break;
            case "thunderstorm":
                AddThunder(room, 2);
                AddRain(room, 2);
                AttackType = Health.EAttackType.Electric;
                break;
            case "heavy thunderstorm":
                AddThunder(room, 3);
                AddRain(room, 2);
                AttackType = Health.EAttackType.Electric;
                break;
            case "ragged thunderstorm":
                AddThunder(room, 3);
                AttackType = Health.EAttackType.Electric;
                break;
            case "thunderstorm with light drizzle":
                AddThunder(room, 2);
                AddDrizzle(room, 1);
                AttackType = Health.EAttackType.Electric;
                break;
            case "thunderstorm with drizzle":
                AddThunder(room, 2);
                AddDrizzle(room, 2);
                AttackType = Health.EAttackType.Electric;
                break;
            case "thunderstorm with heavy drizzle":
                AddThunder(room, 2);
                AddDrizzle(room, 3);
                AttackType = Health.EAttackType.Electric;
                break;

            case "light intensity drizzle":
                AddDrizzle(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "drizzle":
                AddDrizzle(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy intensity drizzle":
                AddDrizzle(room, 3);
                AttackType = Health.EAttackType.Water;
                break;
            case "light intensity drizzle rain":
                AddDrizzle(room, 1);
                AddRain(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "drizzle rain":
                AddDrizzle(room, 2);
                AddRain(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy intensity drizzle rain":
                AddDrizzle(room, 3);
                AddRain(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "shower rain and drizzle":
                AddDrizzle(room, 2);
                AddRain(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy shower rain and drizzle":
                AddDrizzle(room, 2);
                AddRain(room, 3);
                AttackType = Health.EAttackType.Water;
                break;
            case "shower drizzle":
                AddDrizzle(room, 2);
                AttackType = Health.EAttackType.Water;
                break;

            case "light rain":
                AddRain(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "moderate rain":
                AddRain(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy intensity rain":
                AddRain(room, 3);
                AttackType = Health.EAttackType.Water;
                break;
            case "very heavy rain":
                AddRain(room, 4);
                AttackType = Health.EAttackType.Water;
                break;
            case "extreme rain":
                AddRain(room, 5);
                AttackType = Health.EAttackType.Water;
                break;
            case "freezing rain":
                AddRain(room, 2);
                AddSnow(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "light intensity shower rain":
                AddRain(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "shower rain":
                AddRain(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy intensity shower rain":
                AddRain(room, 3);
                AttackType = Health.EAttackType.Water;
                break;
            case "ragged shower rain":
                AddRain(room, 4);
                AttackType = Health.EAttackType.Water;
                break;

            case "light snow":
                AddSnow(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "snow":
                AddSnow(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy snow":
                AddSnow(room, 3);
                AttackType = Health.EAttackType.Water;
                break;
            case "sleet":
                AddRain(room, 1);
                AddSnow(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "shower sleet":
                AddRain(room, 1);
                AddSnow(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "light rain and snow":
                AddRain(room, 1);
                AddSnow(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "rain and snow":
                AddRain(room, 2);
                AddSnow(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "light shower snow":
                AddSnow(room, 1);
                AttackType = Health.EAttackType.Water;
                break;
            case "shower snow":
                AddSnow(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "heavy shower snow":
                AddSnow(room, 3);
                AttackType = Health.EAttackType.Water;
                break;
            case "mist":
                AddClouds(room, 2);
                AttackType = Health.EAttackType.Water;
                break;
            case "fog":
                AddClouds(room, 3);
                AttackType = Health.EAttackType.Physical;
                break;

            case "smoke":
            case "haze":
            case "sand, dust whirls":
            case "sand": 
            case "dust":
            case "volcanic ash":
            case "squalls": 
            case "tornado":
            case "clear sky":
                AttackType = Health.EAttackType.Physical;
                AddClear(room);
                break;

            case "few clouds":
                AddClouds(room, 1);
                AttackType = Health.EAttackType.Physical;
                break;
            case "scattered clouds":
                AddClouds(room, 2);
                AttackType = Health.EAttackType.Physical;
                break;
            case "broken clouds":
                AddClouds(room, 3);
                AttackType = Health.EAttackType.Physical;
                break;
            case "overcast clouds":
                AddClouds(room, 4);
                AttackType = Health.EAttackType.Physical;
                break;

            //case "tornado":
            case "tropical storm":
            case "hurricane":
            case "hot":
                AttackType = Health.EAttackType.Fire;
                break;

            case "windy":
            case "calm":
            case "light breeze":
            case "gentle breeze":
            case "moderate breeze":
            case "fresh breeze":
            case "strong breeze":
            case "high wind, near gale":
                AttackType = Health.EAttackType.Physical;
                break;

            case "gale":
            case "severe gale":
            case "storm":
            case "violent storm":
            case "hail":
            case "cold":
                AttackType = Health.EAttackType.Water;
                break;
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
