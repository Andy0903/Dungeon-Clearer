using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

//Debug.Log("Rise: " + Time.GetDateTime(Time.Root.results.sunrise));
////Day
//Debug.Log("Set: " + Time.GetDateTime(Time.Root.results.sunset));

//Debug.Log("CivilEnd: " + Time.GetDateTime(Time.Root.results.civil_twilight_end));
//Debug.Log("NautEnd: " + Time.GetDateTime(Time.Root.results.nautical_twilight_end));
//Debug.Log("AstroEnd: " + Time.GetDateTime(Time.Root.results.astronomical_twilight_end));

//Debug.Log("AstroBeg: " + Time.GetDateTime(Time.Root.results.astronomical_twilight_begin));
//Debug.Log("NautBeg: " + Time.GetDateTime(Time.Root.results.nautical_twilight_begin));
//Debug.Log("CivilBeg: " + Time.GetDateTime(Time.Root.results.civil_twilight_begin));

////Night

public class APIManager : MonoBehaviour
{
    public class WeatherAPI
    {
        [Serializable]
        public class ResultRoot
        {
            public Coord coord;
            public Weather[] weather;
            public string _base;
            public Main main;
            public Wind wind;
            public Clouds clouds;
            public int dt;
            public Sys sys;
            public int id;
            public string name;
            public int cod;

            [Serializable]
            public class Coord
            {
                public int lon;
                public float lat;
            }

            [Serializable]
            public class Weather
            {
                public int id;
                public string main;
                public string description;
                public string icon;
            }

            [Serializable]
            public class Main
            {
                public float temp;
                public float pressure;
                public int humidity;
                public float temp_min;
                public float temp_max;
                public float sea_level;
                public float grnd_level;
            }

            [Serializable]
            public class Wind
            {
                public float speed;
                public float deg;
            }

            [Serializable]
            public class Clouds
            {
                public int all;
            }

            [Serializable]
            public class Sys
            {
                public float message;
                public int sunrise;
                public int sunset;
            }
        }
        public ResultRoot Root { get; private set; }

        public WeatherAPI()
        {
            Instance.StartCoroutine(GetInfo());
        }

        IEnumerator GetInfo()
        {
            while (true)
            {
                string apiURL = "http://api.openweathermap.org/data/2.5/weather?";
                string lat = "lat=" + Instance.Latitude;
                string lon = "&lon=" + Instance.Longitude;
                string units = "&units=metric";
                string apiKey = "&APPID=6d0ca2d0a164ac45b010d816d834a6a1";

                using (UnityWebRequest www = UnityWebRequest.Get(apiURL + lat + lon + units + apiKey))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError) { Debug.Log(www.error); }
                    else { Root = JsonUtility.FromJson<ResultRoot>(www.downloadHandler.text); }
                }
                yield return new WaitForSeconds(10);
            }
        }
    }
    public class TimeAPI
    {
        [Serializable]
        public class ResultRoot
        {
            public Result results;
            public string status;

            [Serializable]
            public class Result
            {
                public string sunrise;
                public string sunset;
                public string solar_noon;
                public string day_length;
                public string civil_twilight_begin;
                public string civil_twilight_end;
                public string nautical_twilight_begin;
                public string nautical_twilight_end;
                public string astronomical_twilight_begin;
                public string astronomical_twilight_end;
            }
        }
        public ResultRoot Root { get; private set; }

        public TimeAPI()
        {
            Instance.StartCoroutine(GetInfo());
        }

        IEnumerator GetInfo()
        {
            string apiURL = "https://api.sunrise-sunset.org/json?";
            string lat = "lat=" + string.Format("{0:0.00}", Instance.Latitude);
            string lon = "&lng=" + string.Format("{0:0.00}", Instance.Longitude);
            string format = "&formatted=0";

            using (UnityWebRequest www = UnityWebRequest.Get(apiURL + lat + lon + format))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError) { Debug.Log(www.error); }
                else { Root = JsonUtility.FromJson<ResultRoot>(www.downloadHandler.text); }
            }
        }

        public string GetDateTime(string ApiText)
        {
            DateTime dateTime = DateTime.Parse(ApiText, null, System.Globalization.DateTimeStyles.RoundtripKind);
            return dateTime.ToString();
        }
    }

    public static APIManager Instance { get; private set; }
    public WeatherAPI Weather { get; private set; }
    public TimeAPI Time { get; private set; }

    public float Latitude { get; private set; }
    public float Longitude { get; private set; }

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
        Latitude = 56f;     //Input.location.lastData.latitude;
        Longitude = 13f;    //Input.location.lastData.longitude;

        Weather = new WeatherAPI();
        Time = new TimeAPI();
    }
}
