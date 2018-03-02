using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAPIClient : APIClient<WeatherAPIClient.Response>
{
    private const string key = "6d0ca2d0a164ac45b010d816d834a6a1";

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public WeatherAPIClient(double lat, double lng)
    {
        Latitude = lat;
        Longitude = lng;
    }

    protected override string URL
    {
        get
        {
            return string.Format(
                "http://api.openweathermap.org/data/2.5/weather"
                + "?units=metric"
                + "&lat={0:0.00}"
                + "&lon={1:0.00}"
                + "&APPID={2}",
                Latitude, Longitude, key);
        }
    }

    [Serializable]
    public class Response
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
}