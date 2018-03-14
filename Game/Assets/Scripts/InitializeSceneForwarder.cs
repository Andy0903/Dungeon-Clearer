using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeSceneForwarder : MonoBehaviour
{
    [SerializeField]
    int lvlIndex = 1;
    private void Start()
    {
        SceneManager.LoadScene(lvlIndex);
    }
}
