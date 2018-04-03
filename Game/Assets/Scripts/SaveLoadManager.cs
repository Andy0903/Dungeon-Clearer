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

    /// <summary>
    /// Adds an inventory item and saves it to the savefile
    /// </summary>
    /// <param name="item"></param>
    public void AddInventoryItem(Item item)
    {
        LoadedData.InventoryItems.AddLast(item);
        SaveFile();
    }

    /// <summary>
    /// Adds a cleared dungeon count and saves it to the savefile
    /// </summary>
    /// <param name="amount"></param>
    public void AddDungeonCleared(int amount = 1)
    {
        LoadedData.DungeonsCleared += amount;
        SaveFile();
    }

    


    /// <summary>
    /// Saves the current loaded data into the savefile
    /// </summary>
    public void SaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.OpenOrCreate);

        bf.Serialize(file, LoadedData);
        file.Close();
    }

    /// <summary>
    /// Loads the savedata from a savedatafile if it exits, 
    /// otherwise an empty GameData is created and saved into a new savefile
    /// </summary>
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
            SaveFile();
        }
    }

    
    
}

[Serializable]
public class GameData
{
    public int DungeonsCleared;
    public LinkedList<Item> InventoryItems = new LinkedList<Item>();
    public LinkedList<Item> EquippedItems = new LinkedList<Item>();
}