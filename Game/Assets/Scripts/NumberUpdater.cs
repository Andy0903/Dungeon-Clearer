using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberUpdater : MonoBehaviour
{
    private void OnEnable()
    {
        GameData ld = GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().LoadedData;
        GetComponent<Text>().text = ld.DungeonsCleared.ToString();
    }
}
