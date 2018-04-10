using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject playButton;

    private void Update()
    {
        if (APIManager.Instance != null && APIManager.Instance.Ready)
        {
            playButton.GetComponent<Button>().interactable = true;
            Text text = playButton.GetComponentInChildren<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 255);
            enabled = false;
        }
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
