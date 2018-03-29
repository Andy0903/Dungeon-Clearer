using DigitalRuby.RainMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldBuilder : MonoBehaviour
{
    class RoomTemplate
    {
        public GameObject RoomPrefab { get; private set; }
        public List<EDirection> Doors { get; private set; }
        public float MaxTemp { get; private set; }
        public float MinTemp { get; private set; }

        public RoomTemplate(GameObject prefab)
        {
            RoomPrefab = prefab;
            RoomInfo info = RoomPrefab.GetComponent<RoomInfo>();

            Doors = info.GetDoors();
            MaxTemp = info.MaxTemp;
            MinTemp = info.MinTemp;
        }
    }

    enum DoorStatus
    {
        Required,
        Optional,
        Forbidden,
    }

    public static WorldBuilder Instance { get; private set; }
    Dictionary<Vector2Int, GameObject> dungeon;

    //TODO make readonly
    List<RoomTemplate> templates = new List<RoomTemplate>();

    [SerializeField]
    GameObject[] roomPrefabs;
    [SerializeField]
    GameObject[] enemyPrefabs;
    [SerializeField]
    GameObject[] bossPrefabs;

    [SerializeField]
    AnimationCurve bossSpawnCurve;
    float dungeonStartTime;

    [SerializeField]
    GameObject rainMistEffectPrefab;
    [SerializeField]
    GameObject lightningPrefab;
    [SerializeField]
    GameObject[] lightPrefabs;
    [SerializeField]
    GameObject snowParticleSystem;
    [SerializeField]
    GameObject drizzleMistEffectPrefab;

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

        dungeon = new Dictionary<Vector2Int, GameObject>();
        foreach (GameObject room in roomPrefabs)
        {
            templates.Add(new RoomTemplate(room));
        }

        GameObject go = AddRoom(0, 0, FindAppropriateRoom(GetDoorRequirements(Vector2Int.zero)), Vector3.zero);
        dungeonStartTime = Time.time;
    }

    /// <summary>
    /// Adds a new room to the dungeon, gives appropriateWeatherEffects to it.
    /// </summary>
    private GameObject AddRoom(int x, int y, GameObject room, Vector3 pos)
    {
        GameObject go = GameObject.Instantiate(room, pos, Quaternion.identity, gameObject.transform);
        dungeon.Add(new Vector2Int(x, y), go);
        go.GetComponentInChildren<RoomInfo>().DungeonPosition = new Vector2Int(x, y);
        FindAppropriateWeatherEffect(go);

        return go;
    }

    private Vector3 GetPositionOfNewRoom(Transform trigger, EDirection direction, out Vector2Int newRoomDungeonGridPos)
    {
        Vector2Int currentDungeonGridPos = trigger.parent.GetComponentInChildren<RoomInfo>().DungeonPosition;
        newRoomDungeonGridPos = Vector2Int.zero;
        int offsetX = 0;
        int offsetY = 0;

        switch (direction)
        {
            case EDirection.North:
                offsetX = -1;
                offsetY = 0;
                newRoomDungeonGridPos = new Vector2Int(currentDungeonGridPos.x, currentDungeonGridPos.y + 1);
                break;
            case EDirection.East:
                offsetX = 0;
                offsetY = -1;
                newRoomDungeonGridPos = new Vector2Int(currentDungeonGridPos.x + 1, currentDungeonGridPos.y);
                break;
            case EDirection.South:
                offsetX = -1;
                offsetY = -2;
                newRoomDungeonGridPos = new Vector2Int(currentDungeonGridPos.x, currentDungeonGridPos.y - 1);
                break;
            case EDirection.West:
                offsetX = -2;
                offsetY = -1;
                newRoomDungeonGridPos = new Vector2Int(currentDungeonGridPos.x - 1, currentDungeonGridPos.y);
                break;
        }

        return new Vector3(trigger.position.x + offsetX, trigger.position.y + offsetY, trigger.position.z) + trigger.transform.localPosition;
    }

    private Dictionary<EDirection, DoorStatus> GetDoorRequirements(Vector2Int newRoomDungeonGridPos)
    {
        Dictionary<EDirection, DoorStatus> doors = new Dictionary<EDirection, DoorStatus>();
        foreach (EDirection dir in Enum.GetValues(typeof(EDirection)))
        {
            GameObject newRoomNeighbor = FindAdjacentRoom(newRoomDungeonGridPos, dir);
            if (newRoomNeighbor == null)
                doors[dir] = DoorStatus.Optional;
            else if (newRoomNeighbor.GetComponent<RoomInfo>().HasDoorAt(OppositeDirection(dir)))
                doors[dir] = DoorStatus.Required;
            else
                doors[dir] = DoorStatus.Forbidden;
        }

        return doors;
    }

    /// <summary>
    /// Note that only rooms made with this method can have enemies. The first room is added with "AddRoom" only, hence it shan't ever have enemies appearing.
    /// </summary>
    public void SpawnRoom(Transform trigger, EDirection direction)
    {
        Vector2Int newRoomDungeonGridPos;
        Vector3 newRoomPos = GetPositionOfNewRoom(trigger, direction, out newRoomDungeonGridPos);

        GameObject room = AddRoom(newRoomDungeonGridPos.x, newRoomDungeonGridPos.y,
                          FindAppropriateRoom(GetDoorRequirements(newRoomDungeonGridPos)),
                          newRoomPos.ToVector3IntOnGrid());

        DestoryUnnecessaryExits(room, direction);

        if (bossSpawnCurve.Evaluate(Time.timeSinceLevelLoad) >= UnityEngine.Random.value * 100)
        {
            PopulateWithBoss(room);
        }
        else
        {
            PopulateWithEnemies(room);
        }
    }

    private void FindAppropriateWeatherEffect(GameObject room)
    {
        string main = APIManager.Instance.Weather.Data.weather[0].main;

        switch (main)
        {
            case "Rain":
                RainEffects(room);
                break;
            case "Clouds":
                CloudEffects(room);
                break;
            case "Thunderstorm":
                ThunderEffects(room);
                break;
            case "Drizzle":
                DrizzleEffects(room);
                break;
            case "Snow":
                SnowEffects(room);
                break;
            case "Clear":
                ClearEffects(room);
                break;
            case "Extreme":
                break;
            case "Additional":
                break;
            default:
                Debug.Log("Unknown main");
                break;
        }
    }

    private void DrizzleEffects(GameObject room)
    {
        string description = APIManager.Instance.Weather.Data.weather[0].description;
        GameObject go = GameObject.Instantiate(drizzleMistEffectPrefab, room.transform, false);
        RainScript2D drizzle = go.GetComponent<RainScript2D>();
        GameObject go2 = GameObject.Instantiate(rainMistEffectPrefab, room.transform, false);
        RainScript2D r = go2.GetComponent<RainScript2D>();

        description = "shower rain and drizzle";

        switch (description)
        {
            case "light intensity drizzle":
                drizzle.RainIntensity = 0.024f;
                break;
            case "drizzle":
            case "shower drizzle":
                drizzle.RainIntensity = 0.1f;
                break;
            case "heavy intensity drizzle":
                drizzle.RainIntensity = 0.4f;
                break;
            case "light intensity drizzle rain":
                drizzle.RainIntensity = 0.024f;
                r.RainIntensity = 0.024f;
                break;
            case "drizzle rain":
            case "shower rain and drizzle":
                drizzle.RainIntensity = 0.1f;
                r.RainIntensity = 0.1f;
                break;
            case "heavy intensity drizzle rain":
            case "heavy shower and drizzle":
                drizzle.RainIntensity = 0.4f;
                r.RainIntensity = 0.4f;
                break;

            default:
                Debug.Log("Unknown description");
                break;
        }
    }

    private void SnowEffects(GameObject room)
    {
        string description = APIManager.Instance.Weather.Data.weather[0].description;
        GameObject go = GameObject.Instantiate(snowParticleSystem, room.transform, false);

        int rate = 0;
        //TODO add rain etc
        switch (description)
        {
            case "light snow":
                rate = 1;
                break;
            case "snow":
                rate = 3;
                break;
            case "heavy snow":
                rate = 6;
                break;
            case "sleet":
                rate = 2;
                break;
            case "shower sleet":
                rate = 2;
                break;
            case "light rain and snow":
                rate = 3;
                break;
            case "light shower snow":
                rate = 3;
                break;
            case "shower snow":
                rate = 3;
                break;
            case "heavy shower snow":
                rate = 6;
                break;
            default:
                Debug.Log("Unknown description");
                break;
        }

        ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem p in ps)
        {
            var emission = p.emission;
            emission.rateOverTime = rate;
        }
    }

    private void ClearEffects(GameObject room)
    {
        GameObject.Instantiate(lightPrefabs[UnityEngine.Random.Range(0, lightPrefabs.Length - 1)], room.transform, false);
    }

    private void ThunderEffects(GameObject room)
    {
        string description = APIManager.Instance.Weather.Data.weather[0].description;
        GameObject go = GameObject.Instantiate(rainMistEffectPrefab, room.transform, false);
        RainScript2D r = go.GetComponent<RainScript2D>();
        GameObject go2 = GameObject.Instantiate(lightningPrefab, room.transform, false);
        LightningBehaviour l = go2.GetComponent<LightningBehaviour>();

        switch (description)
        {
            case "thunderstom with light rain":
                r.RainIntensity = 0.024f;
                l.Initialize(10, 30);
                break;
            case "thunderstom with rain":
                r.RainIntensity = 0.1f;
                l.Initialize(10, 30);
                break;
            case "thuderstorm with heavy rain":
                r.RainIntensity = 0.6f;
                l.Initialize(10, 30);
                break;
            case "light thunderstorm":
                l.Initialize(10, 60);
                break;
            case "thunderstorm":
                l.Initialize(10, 30);
                break;
            case "heavy thunderstorm":
            case "ragged thunderstrom":
                l.Initialize(10, 20);
                break;
            case "thunderstorm with light drizzle":
            case "Thunderstorm with drizzle":
            case "thunderstorm with heavy drizzle":
                l.Initialize(10, 30);
                //TODO drizzle?
                break;
            default:
                Debug.Log("Unknown description");
                break;
        }
    }

    private void CloudEffects(GameObject room)
    {
        string description = APIManager.Instance.Weather.Data.weather[0].description;
        GameObject go = GameObject.Instantiate(rainMistEffectPrefab, room.transform, false);
        RainScript2D r = go.GetComponent<RainScript2D>();

        switch (description)
        {
            case "few clouds":
                r.MistIntensity = 0.2f;
                break;
            case "scattered clouds":
                r.MistIntensity = 0.4f;
                break;
            case "broken clouds":
                r.MistIntensity = 0.6f;
                break;
            case "overcast clouds":
                r.MistIntensity = 0.8f;
                break;

            default:
                Debug.Log("Unknown description");
                break;
        }
    }

    private void RainEffects(GameObject room)
    {
        string description = APIManager.Instance.Weather.Data.weather[0].description;
        GameObject go = GameObject.Instantiate(rainMistEffectPrefab, room.transform, false);
        RainScript2D r = go.GetComponent<RainScript2D>();

        switch (description)
        {
            case "light rain":
            case "light intensity shower rain":
                r.RainIntensity = 0.024f;
                break;
            case "moderate rain":
            case "shower rain":
            case "freezing rain":
                r.RainIntensity = 0.1f;
                break;
            case "heavy intensity rain":
            case "heavy intensity shower rain":
                r.RainIntensity = 0.4f;
                break;
            case "very heavy rain":
            case "ragged shower rain":
                r.RainIntensity = 0.6f;
                break;
            case "extreme rain":
                r.RainIntensity = 1f;
                break;

            default:
                Debug.Log("Unknown description");
                break;
        }
    }

    private bool IsTileColliderless(TileBase tile)
    {
        return tile != null && (tile as Tile).colliderType == Tile.ColliderType.None;
    }

    /// <summary>
    /// If boss is true it checks 2x2 tiles instead of just 1 tile. Since bosses are assumed to be double in scale on X and Y axis.
    /// </summary>
    private bool FoundValidSpawnLocation(GameObject room, out Vector3 position, bool boss = false)
    {
        Tilemap tm = room.GetComponent<Tilemap>();
        BoundsInt bounds = tm.cellBounds;
        bool result = false;
        Vector3Int tempWorldPosInt = Vector3Int.zero;

        for (int i = 0; i < 100; i++)
        {
            int x = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
            int y = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);

            Vector3Int tempCellPos = new Vector3Int(x, y, 0);
            TileBase tile = tm.GetTile(tempCellPos);

            if (boss)
            {
                TileBase tile2 = tm.GetTile(tempCellPos + new Vector3Int(1, 0, 0));
                TileBase tile3 = tm.GetTile(tempCellPos + new Vector3Int(0, 1, 0));
                TileBase tile4 = tm.GetTile(tempCellPos + new Vector3Int(1, 1, 0));
                if (IsTileColliderless(tile) && IsTileColliderless(tile2) && IsTileColliderless(tile3) && IsTileColliderless(tile4))
                {
                    Vector3 tempWorldPos = tm.CellToWorld(tempCellPos);
                    tempWorldPosInt = tempWorldPos.ToVector3Int();
                    result = true;
                    break;
                }
            }
            else
            {
                if (IsTileColliderless(tile))
                {
                    Vector3 tempWorldPos = tm.CellToWorld(tempCellPos);
                    tempWorldPosInt = tempWorldPos.ToVector3Int();
                    result = true;
                    break;
                }
            }
        }

        position = tempWorldPosInt;
        return result;
    }

    private void PopulateWithEnemies(GameObject room)
    {
        Vector3 enemySize = new Vector3(1, 1, 0);
        Vector3 position;
        for (int i = 0; i < 5; i++)
        {
            if (FoundValidSpawnLocation(room, out position))
            {
                GameObject.Instantiate(enemyPrefabs[0], position + (enemySize / 2), Quaternion.identity, room.transform);
            }
        }
    }

    private void PopulateWithBoss(GameObject room)
    {
        Vector3 bossSize = new Vector3(2, 2, 0);
        Vector3 position;

        if (FoundValidSpawnLocation(room, out position, true))
        {
            GameObject.Instantiate(bossPrefabs[0], position + (bossSize / 2), Quaternion.identity, room.transform);
            BoxCollider2D lockTrigger = room.AddComponent<BoxCollider2D>();
            lockTrigger.isTrigger = true;
            lockTrigger.size = new Vector2(lockTrigger.size.x - 3, lockTrigger.size.y - 3);
        }
    }

    private void RemoveDeadEndRoomsIfUnnecessary(Dictionary<EDirection, DoorStatus> doors, List<RoomTemplate> candidates)
    {
        int allowedDoors = doors.Values.Where(d => d != DoorStatus.Forbidden).Select(t => t).Count();
        if (allowedDoors > 1)
        {
            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                if (candidates[i].Doors.Count == 1)
                {
                    candidates.RemoveAt(i);
                }
            }
        }
    }

    private void RemoveRoomsWithoutAppropriateDoorStatus(Dictionary<EDirection, DoorStatus> doors, List<RoomTemplate> candidates)
    {
        foreach (EDirection dir in Enum.GetValues(typeof(EDirection)))
        {
            if (doors[dir] == DoorStatus.Optional)
            {
                continue;
            }

            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                if ((doors[dir] == DoorStatus.Forbidden && candidates[i].Doors.Contains(dir)) ||
                    (doors[dir] == DoorStatus.Required && !candidates[i].Doors.Contains(dir)))
                {
                    candidates.RemoveAt(i);
                }
            }
        }
    }

    private void RemoveRoomsWithUnfittingTemperatures(List<RoomTemplate> candidates)
    {
        float temperature = Mathf.Round(APIManager.Instance.Weather.Data.main.temp);
        for (int i = candidates.Count - 1; i >= 0; i--)
        {
            if (!(temperature <= candidates[i].MaxTemp && temperature >= candidates[i].MinTemp))
            {
                candidates.RemoveAt(i);
            }
        }
    }

    private GameObject FindAppropriateRoom(Dictionary<EDirection, DoorStatus> doors)
    {
        List<RoomTemplate> candidates = new List<RoomTemplate>(templates);
        RemoveDeadEndRoomsIfUnnecessary(doors, candidates);
        RemoveRoomsWithoutAppropriateDoorStatus(doors, candidates);
        RemoveRoomsWithUnfittingTemperatures(candidates);

        candidates.TrimExcess();
        return candidates[UnityEngine.Random.Range(0, candidates.Count)].RoomPrefab;
    }

    private void DestoryUnnecessaryExits(GameObject room, EDirection direction)
    {
        RoomExit[] newDoorPoints = room.GetComponentsInChildren<RoomExit>();
        foreach (RoomExit newExit in newDoorPoints)
        {
            if (newExit.Direction == OppositeDirection(direction))
            {
                Destroy(newExit.gameObject);
            }
        }
    }

    private GameObject FindAdjacentRoom(Vector2Int dictionaryPosition, EDirection direction)
    {
        GameObject adjacentRoom;
        Vector2Int pos = Vector2Int.zero;

        switch (direction)
        {
            case EDirection.North:
                pos = new Vector2Int(dictionaryPosition.x, dictionaryPosition.y + 1);
                break;
            case EDirection.East:
                pos = new Vector2Int(dictionaryPosition.x + 1, dictionaryPosition.y);
                break;
            case EDirection.South:
                pos = new Vector2Int(dictionaryPosition.x, dictionaryPosition.y - 1);
                break;
            case EDirection.West:
                pos = new Vector2Int(dictionaryPosition.x - 1, dictionaryPosition.y);
                break;
        }

        dungeon.TryGetValue(pos, out adjacentRoom);
        return adjacentRoom;
    }

    private EDirection OppositeDirection(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.North:
                return EDirection.South;
            case EDirection.East:
                return EDirection.West;
            case EDirection.South:
                return EDirection.North;
            case EDirection.West:
                return EDirection.East;
        }

        return EDirection.North;
    }
}
