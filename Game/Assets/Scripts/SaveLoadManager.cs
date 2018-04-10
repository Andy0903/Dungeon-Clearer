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

    public float PercentageKilled
    {
        get
        {
            if (LoadedData.EnemiesSpawned != 0)
                return LoadedData.EnemiesKilled / LoadedData.EnemiesSpawned;
            else
                return 0.5f;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadFile();
    }

    public void SaveInventoryAndEquipment()
    {
        Debug.Log("Saved items");
        Item[] items = GameObject.FindGameObjectWithTag("InventoryPanel").transform.Find("InventoryGrid").transform.GetComponentsInChildren<Item>();
        //EquipmentManager EM = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>();

        LoadedData.Inventory = new ItemComponentHolder[items.Length];

        for (int i = 0; i < LoadedData.Inventory.Length; i++)
        {
            LoadedData.Inventory[i] = new ItemComponentHolder();
            LoadedData.Inventory[i].type = items[i].Type;
            LoadedData.Inventory[i].components = items[i].GetItemComponents();
        }



        //LoadedData.Equipment = EM.EquippedItems;

        SaveFile();
    }


    /// <summary>
    /// Adds a cleared dungeon count and saves it to the savefile
    /// </summary>
    /// <param name="amount"></param>
    public void AddDungeonCleared(int amountEnemiesKilled, int amountEnemiesSpawned, int dungeonsCleared = 1)
    {
        LoadedData.DungeonsCleared += dungeonsCleared;
        LoadedData.EnemiesKilled += amountEnemiesKilled;
        LoadedData.EnemiesSpawned += amountEnemiesSpawned;
        SaveFile();
    }


    public void SaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.OpenOrCreate);

        bf.Serialize(file, LoadedData);
        file.Close();
    }

    public void LoadFile(bool loadInventory = false)
    {
        if (File.Exists(Application.persistentDataPath + FileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.Open);
            LoadedData = bf.Deserialize(file) as GameData;

            if (loadInventory)
            {
                foreach (ItemComponentHolder ih in LoadedData.Inventory)
                {
                    GameObject.FindGameObjectWithTag("InventoryPanel").GetComponent<Inventory>().PutSpecificItemIntoFirstEmptySlot(ih.type, ih.components);
                }

            }

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
    public ItemComponentHolder[] Inventory;
    public Dictionary<string, EquipmentSlot> Equipment = new Dictionary<string, EquipmentSlot>();
    public int EnemiesKilled;
    public int EnemiesSpawned;
}

[Serializable]
public class ItemComponentHolder
{
    public Item.EType type;
    public List<IItemComponent> components;
}