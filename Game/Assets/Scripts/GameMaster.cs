using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private Player player;

    private Enemy boss;
    private bool hasBossSpawned;
    bool won;

    /*
    private enum GameState
    {
        MainMenu,
        PausMenu,
        Playing
    }
    */

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
        }
    }

    void CheckWinCondition()
    {
        if(hasBossSpawned && boss == null)   //!boss.GetComponent<Health>().isAlive
        {
            //Change menu to "win screen"
            Restart();
            won = true;
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
        if (scene.buildIndex == 1 && won)
        {
            won = false;
            GameObject.FindGameObjectWithTag("InventoryPanel").GetComponent<Inventory>().PutItemToFirstEmptySlot();
        }
    }
}
