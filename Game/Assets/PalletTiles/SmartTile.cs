using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SmartTile : Tile
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

    private NeighborsAt neighborsAt;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; y++)
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
        tileData.sprite = sprites[1];
    }

    private bool HasNeighbor(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    //public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    //{
    //    return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
    //}

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