using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public abstract class APIClient<Response>
{
    protected abstract string URL { get; }
    public Response Data { get; private set; }

    public void Refresh(MonoBehaviour behaviour)
    {
        behaviour.StartCoroutine(Get());
    }

    public void AutoRefresh(MonoBehaviour behaviour, int seconds)
    {
        behaviour.StartCoroutine(Poll(seconds));
    }

    private IEnumerator Get()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Data = JsonUtility.FromJson<Response>(www.downloadHandler.text);
            }
        }
    }

    private IEnumerator Poll(int seconds)
    {
        while (true)
        {
            yield return Get();
            yield return new WaitForSeconds(seconds);
        }
    }
}