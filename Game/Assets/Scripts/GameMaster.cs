using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private Player player;

    private Enemy boss;
    private bool hasBossSpawned;
    private int enemiesSpawned;
    bool won;

	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.Find("Player").GetComponent<Player>();
        hasBossSpawned = false;
	}

    public void SetBoss(Enemy boss)
    {
        this.boss = boss;
        hasBossSpawned = true;
    }

    public void AddEnemiesSpawned(int amount)
    {
        enemiesSpawned += amount;
    }

	void Update ()
    {
        CheckWinCondition();
        CheckLossCondition();
	}

    void Restart()
    {
        //Player position needs to be reset and maps needs to be reloaded
        player.Reset();
        hasBossSpawned = false;
        boss = null;
    }

    void CheckLossCondition()
    {
        if(!player.GetComponent<Health>().isAlive)
        {
            //TODO: We probably want this to be a Loss screen instead
            Restart();
            SceneManager.LoadScene(1);
        }
    }

    void CheckWinCondition()
    {
        if(hasBossSpawned && boss == null)   //!boss.GetComponent<Health>().isAlive
        {
            //Change menu to "win screen"
            int amountOfEnemiesKilled = player.EnemiesKilled;

            Restart();
            won = true;

            GameObject.FindObjectOfType<SaveLoadManager>().AddDungeonCleared(amountOfEnemiesKilled, enemiesSpawned);
            SceneManager.LoadScene(1);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void nDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            AudioManager.Instance.Stop("Fire");
            GameObject.FindObjectOfType<SaveLoadManager>().LoadFile(true);
        }

        if (won)
        {
            won = false;
            GameObject.FindGameObjectWithTag("InventoryPanel").GetComponent<Inventory>().PutItemToFirstEmptySlot();
            GameObject.FindObjectOfType<SaveLoadManager>().SaveInventoryAndEquipment();
        }
    }
}
