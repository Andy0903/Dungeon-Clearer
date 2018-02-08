using UnityEngine;
using UnityEngine.UI;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    Transform itemParent;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject Create(Item.EType type) //object?
    {
        GameObject go = GameObject.Instantiate(itemPrefab);
        Item item = go.GetComponent<Item>();
        go.transform.SetParent(itemParent);
        Image image = go.GetComponent<Image>();

        item.Type = Item.EType.Head;
        image.sprite = Resources.Load<Sprite>("oryx_16bit_fantasy_items_284");
        item.AddComponent(new StatComponent(EStat.Strength, 5));
        
        return go;
    }
}