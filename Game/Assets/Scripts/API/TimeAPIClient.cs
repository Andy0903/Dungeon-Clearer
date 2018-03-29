using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAPIClient : APIClient<TimeAPIClient.Response>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public TimeAPIClient(double lat, double lng)
    {
        Latitude = lat;
        Longitude = lng;
    }

    protected override string URL
    {
        get
        {
            return string.Format(
                "https://api.sunrise-sunset.org/json"
                + "?formatted=0"
                + "&lat={0:0.00}"
                + "&lng={1:0.00}",
                Latitude, Longitude);
        }
    }

    public DateTime ConvertDateTime(string datetime)
    {
        return DateTime.Parse(datetime, null,
            System.Globalization.DateTimeStyles.RoundtripKind);
    }

    [Serializable]
    public class Response
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
}