using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LonLatUpdater : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Text>().text = string.Format("Lng: {0}      Lat: {1}", APIManager.Instance.Lng, APIManager.Instance.Lat);
    }
}
