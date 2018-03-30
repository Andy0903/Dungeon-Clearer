using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    private enum GameState
    {
        MainMenu,
        PausMenu,
        Playing
    }

	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
    {
        CheckWinCondition();
	}

    void CheckWinCondition()
    {

    }
}
