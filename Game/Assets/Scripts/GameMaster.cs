using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private Player player;

    private Enemy boss;
    private bool hasBossSpawned;

    private enum GameState
    {
        MainMenu,
        PausMenu,
        Playing
    }

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
	}

    void Restart()
    {
        //Player position needs to be reset
        player.Reset();
    }

    void CheckWinCondition()
    {
        if(hasBossSpawned && !boss.GetComponent<Health>().isAlive)
        {
            //Change menu to "win screen"
        }
    }
}
