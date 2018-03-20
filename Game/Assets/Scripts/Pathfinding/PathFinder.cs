using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "PluggableAI/Pathfinder")]
public abstract class PathFinder : ScriptableObject
{
    protected Tilemap BackgroundLayer { get; private set; }
    protected Tilemap ForegroundLayer { get; private set; }
    protected Dictionary<Vector3Int, Node> WorldNode { get; private set; }

    private void Initialize()
    {
       // if (BackgroundLayer == null)// && ForegroundLayer == null)
        {
            BackgroundLayer = GameObject.FindGameObjectWithTag("BackgroundLayer").GetComponent<Tilemap>();

          //  GameObject.Find
            
            //ForegroundLayer = GameObject.FindGameObjectWithTag("ForegroundLayer").GetComponent<Tilemap>();
        }
        MapNodesToDictionary();
    }

    private void MapNodesToDictionary()
    {
        BackgroundLayer.CompressBounds();
        //ForegroundLayer.CompressBounds();

        BoundsInt bounds = BackgroundLayer.cellBounds;
        WorldNode = new Dictionary<Vector3Int, Node>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tempCellPos = new Vector3Int(x, y, 0);
                TileBase tile = BackgroundLayer.GetTile(tempCellPos);

                if (tile != null && (tile as Tile).colliderType == Tile.ColliderType.None)
                {
                    Vector3 tempWorldPos = BackgroundLayer.CellToWorld(tempCellPos);
                    Node temp = new Node(tempCellPos, tempWorldPos);

                    Vector3Int tempWorldPosInt = tempWorldPos.ToVector3Int();
                    WorldNode.Add(tempWorldPosInt, temp);
                }
            }
        }
    }

    protected abstract List<Vector3> FindPathBehaviour(Vector3 from, Vector3 to);

    public List<Vector3> FindPath(Vector3 from, Vector3 to)
    {
        Initialize();
        return FindPathBehaviour(from, to);
    }
}
