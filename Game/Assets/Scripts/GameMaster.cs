using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private Player player;

    private Enemy boss;
    private bool hasBossSpawned;

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
        if(hasBossSpawned && !boss.GetComponent<Health>().isAlive)
        {
            //Change menu to "win screen"
        }
    }
}
