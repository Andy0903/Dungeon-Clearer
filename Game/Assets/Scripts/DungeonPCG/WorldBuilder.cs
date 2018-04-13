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

    WeatherEffectFactory weatherFactory;
    TimeEffectFactory timeFactory;

    Health.EAttackType attackType;

    const int DefaultSpawnAmount = 5;
    const int MaxSpawnAmount = 10;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //  DontDestroyOnLoad(gameObject);
        }

        weatherFactory = GetComponent<WeatherEffectFactory>();
        timeFactory = GetComponent<TimeEffectFactory>();
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
        weatherFactory.AddWeather(go);
        attackType = weatherFactory.AttackType;
        timeFactory.AddTimeFilter(go);

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

        if (bossSpawnCurve.Evaluate(Time.timeSinceLevelLoad) >= UnityEngine.Random.value * 100 && GameObject.FindObjectOfType<GameMaster>().boss == null)
        {
            PopulateWithBoss(room);
        }
        else
        {
            PopulateWithEnemies(room);
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

        GameData gd = GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().LoadedData;
        int amountToSpawn;

        //GameData gd = new GameData();
        //gd.EnemiesKilled = 5;
        //gd.EnemiesSpawned = 10;


        if (gd.EnemiesSpawned > 0)
        {
            const int RNGValue = 5;
            int rng = (int)(UnityEngine.Random.value * RNGValue);
            amountToSpawn = (int)(DefaultSpawnAmount * (rng * (float)gd.EnemiesKilled / (float)gd.EnemiesSpawned));
            //Debug.Log("Spawn Amount: " + amountToSpawn + " RNG: " + rng + " Estimate Value: " + (rng * (((float)gd.EnemiesKilled / (float)gd.EnemiesSpawned))));
            if (amountToSpawn <= 0)
            {
                amountToSpawn = DefaultSpawnAmount;
            }
            else if (amountToSpawn > MaxSpawnAmount)
            {
                //Tries to decrease the spawn amount as an RNG'd 6 with 50% kills yields 15 enemies
                amountToSpawn -= DefaultSpawnAmount;
            }

        }
        else
        {
            amountToSpawn = DefaultSpawnAmount;
        }
        GameObject.Find("GameMaster").GetComponent<GameMaster>().AddEnemiesSpawned(amountToSpawn);
        for (int i = 0; i < amountToSpawn; i++)
        {
            if (FoundValidSpawnLocation(room, out position))
            {
                GameObject GO =
                    GameObject.Instantiate(GetRandomEnemyWithDayFactor(room, enemyPrefabs), position + (enemySize / 2), Quaternion.identity, room.transform);
                GO.GetComponent<Enemy>().AttackType = attackType;
            }
        }
    }

    GameObject GetRandomEnemyWithDayFactor(GameObject room, GameObject[] collection)
    {
        List<float> p = new List<float>();
        foreach (GameObject g in collection)
        {
            Enemy e = g.GetComponent<Enemy>();
            int d = Mathf.Abs(room.GetComponent<RoomInfo>().Time - e.mainTime);
            float x = .5f + .5f * Mathf.Cos(d * 360 / (Enum.GetValues(typeof(TimeEffectFactory.DayPhase)).Length + 1));
            p.Add(x);
        }

        float u = p.Sum();
        float r = UnityEngine.Random.Range(0, u);
        float sum = 0;

        for (int i = 0; i < p.Count; i++)
        {
            sum += p[i];
            if (r < sum)
            {
                return collection[i];
            }
        }

        return collection[0];
    }

    private void PopulateWithBoss(GameObject room)
    {
        Vector3 bossSize = new Vector3(2, 2, 0);
        Vector3 position;

        if (FoundValidSpawnLocation(room, out position, true))
        {
            GameObject go = GameObject.Instantiate(GetRandomEnemyWithDayFactor(room, bossPrefabs), position + (bossSize / 2), Quaternion.identity, room.transform);
            GameObject.Find("GameMaster").GetComponent<GameMaster>().SetBoss(go.GetComponent<Enemy>());
            BoxCollider2D lockTrigger = room.AddComponent<BoxCollider2D>();
            lockTrigger.isTrigger = true;
            lockTrigger.size = new Vector2(lockTrigger.size.x - 5, lockTrigger.size.y - 5);
            lockTrigger.gameObject.layer = 2;
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
                newExit.CanSpawnRoom = false;
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
