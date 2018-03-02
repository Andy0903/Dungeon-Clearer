using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SmartTile : TileBase
{
    [Flags]
    enum NeighborsAt
    {
        North = 1,
        West = 2,
        East = 4,
        South = 8,
    }

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private Sprite preview;

    [SerializeField]
    private Tile.ColliderType m_TileColliderType;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasNeighbor(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos);
                }
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.colliderType = m_TileColliderType;
        NeighborsAt neighborsAt = 0;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasNeighbor(tilemap, nPos))
                {
                    AddToNeighborsAt(x, y, ref neighborsAt);
                }
            }
        }

        //int randomVal = Random.Range(0, 100);
        //if (randomVal < 15)
        //{
        //    tileData.sprite = waterSprites[46];
        //}
        //else if (randomVal >= 15 && randomVal < 35)
        //{
        //    tileData.sprite = waterSprites[48];
        //}
        //else
        //{
        //    tileData.sprite = waterSprites[47];
        //}

        switch (neighborsAt)
        {
            case NeighborsAt.East:
                tileData.sprite = sprites[1];
                break;
            case NeighborsAt.West | NeighborsAt.East:
                tileData.sprite = sprites[2];
                break;
            case NeighborsAt.West:
                tileData.sprite = sprites[3];
                break;
            case NeighborsAt.South:
                tileData.sprite = sprites[4];
                break;
            case NeighborsAt.South | NeighborsAt.North:
                tileData.sprite = sprites[5];
                break;
            case NeighborsAt.North:
                tileData.sprite = sprites[6];
                break;
            case NeighborsAt.South | NeighborsAt.East:
                tileData.sprite = sprites[7];
                break;
            case NeighborsAt.South | NeighborsAt.West:
                tileData.sprite = sprites[8];
                break;
            case NeighborsAt.North | NeighborsAt.East:
                tileData.sprite = sprites[9];
                break;
            case NeighborsAt.North | NeighborsAt.West:
                tileData.sprite = sprites[10];
                break;
            case NeighborsAt.North | NeighborsAt.East | NeighborsAt.South | NeighborsAt.West:
                tileData.sprite = sprites[11];
                break;
            case NeighborsAt.East | NeighborsAt.South | NeighborsAt.West:
                tileData.sprite = sprites[12];
                break;
            case NeighborsAt.North | NeighborsAt.South | NeighborsAt.West:
                tileData.sprite = sprites[13];
                break;
            case NeighborsAt.North | NeighborsAt.South | NeighborsAt.East:
                tileData.sprite = sprites[14];
                break;
            case NeighborsAt.North | NeighborsAt.West | NeighborsAt.East:
                tileData.sprite = sprites[15];
                break;
            default:
                tileData.sprite = sprites[0];
                break;
        }
    }

    private bool HasNeighbor(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    private void AddToNeighborsAt(int x, int y, ref NeighborsAt neighborsAt)
    {
        if (x == 1 && y == 0)
        {
            neighborsAt |= NeighborsAt.East;
        }
        else if (x == -1 && y == 0)
        {
            neighborsAt |= NeighborsAt.West;
        }
        else if (x == 0 && y == 1)
        {
            neighborsAt |= NeighborsAt.North;
        }
        else if (x == 0 && y == -1)
        {
            neighborsAt |= NeighborsAt.South;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/SmartTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save SmartTile", "New SmartTile", "asset", "Save SmartTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SmartTile>(), path);
    }
#endif
}