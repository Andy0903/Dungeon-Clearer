using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeSceneForwarder : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
