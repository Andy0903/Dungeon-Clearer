using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private void Update()
    {
        if (APIManager.Instance != null && APIManager.Instance.Ready)
        {
            GameObject play = GameObject.FindGameObjectWithTag("PlayButton");
            play.GetComponent<Button>().interactable = true;
            Text text = play.GetComponentInChildren<Text>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 255);
            enabled = false;
        }
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}
