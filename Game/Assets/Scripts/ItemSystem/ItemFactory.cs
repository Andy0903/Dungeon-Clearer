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

    public GameObject Create(Item.EType type)
    {
        GameObject go = GameObject.Instantiate(itemPrefab);
        Item item = go.GetComponent<Item>();
        go.transform.SetParent(itemParent);
        Image image = go.GetComponent<Image>();

        string spritePath = type.ToString() + "/";
        item.Type = type;
        switch (item.Type)
        {
            case Item.EType.Head:
                spritePath += "oryx_16bit_fantasy_items_284";
                break;
            case Item.EType.Shoulder:
                break;
            case Item.EType.Chest:
                spritePath += "oryx_16bit_fantasy_items_265";
                break;
            case Item.EType.Back:
                spritePath += "oryx_16bit_fantasy_items_243";
                break;
            case Item.EType.Wrist:
                break;
            case Item.EType.Hands:
                spritePath += "oryx_16bit_fantasy_items_258";
                break;
            case Item.EType.Waist:
                spritePath += "oryx_16bit_fantasy_items_235";
                break;
            case Item.EType.Legs:
                spritePath += "oryx_16bit_fantasy_items_298";
                break;
            case Item.EType.Feet:
                spritePath += "oryx_16bit_fantasy_items_287";
                break;
            case Item.EType.Neck:
                break;
            case Item.EType.Trinket:
                break;
            case Item.EType.Finger:
                break;
            case Item.EType.OneHand:
                spritePath += "oryx_16bit_fantasy_items_159";
                break;
            case Item.EType.TwoHand:
                spritePath += "oryx_16bit_fantasy_items_169";
                break;
            case Item.EType.OffHand:
                spritePath += "oryx_16bit_fantasy_items_221";
                break;
            default:
                Debug.Log("INVALID TYPE ON ITEM!");
                break;
        }

        image.sprite = Resources.Load<Sprite>(spritePath);

        item.AddComponent(new StatComponent((Stats.EType)Random.Range(0,2), Random.Range(-100, 100)).BindToItem(item));
        
        return go;
    }

    private void SetSprite()
    {

    }
}