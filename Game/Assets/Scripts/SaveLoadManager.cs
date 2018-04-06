using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private const string FileName = "/GameInfo.dat";

    public GameData LoadedData
    {
        get;
        private set;
    }
    
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}

    public void SaveInventoryAndEquipment()
    {
        GameObject go = GameObject.Find("InventoryGrid");
        Item[] items = go.GetComponentsInChildren<Item>();

        go = GameObject.Find("EquipmentManager");
        EquipmentManager EM = go.GetComponent<EquipmentManager>();
        LoadedData.Equipment = EM.EquippedItems;
        SaveFile();
    }


    /// <summary>
    /// Adds a cleared dungeon count and saves it to the savefile
    /// </summary>
    /// <param name="amount"></param>
    public void AddDungeonCleared(int amountEnemiesKilled, int amountEnemiesSpawned, int dungeonsCleared = 1)
    {
        LoadedData.DungeonsCleared += dungeonsCleared;
        SaveFile();
    }


    public void SaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.OpenOrCreate);

        bf.Serialize(file, LoadedData);
        file.Close();
    }

    public void LoadFile()
    {
        if(File.Exists(Application.persistentDataPath + FileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.Open);
            LoadedData = bf.Deserialize(file) as GameData;
            file.Close();
        }
        else
        {
            LoadedData = new GameData();
            SaveFile(); //Creates and saves new file if it doesn't exist
        }
    }

}

[Serializable]
public class GameData
{
    public int DungeonsCleared;
    public Item[] Inventory;
    public Dictionary<string, EquipmentSlot> Equipment = new Dictionary<string, EquipmentSlot>();
    public int EnemiesKilled;
    public int EnemiesSpawned;
}