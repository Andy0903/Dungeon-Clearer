using UnityEngine;

public static class Vector3Extension
{
    public static Vector3Int ToVector3Int(this Vector3 rhs)
    {
        return new Vector3Int((int)rhs.x, (int)rhs.y, (int)rhs.z);
    }

    public static Vector3Int ToVector3IntOnGrid(this Vector3 rhs)
    {
        return new Vector3Int(Mathf.FloorToInt(rhs.x), Mathf.FloorToInt(rhs.y), Mathf.FloorToInt(rhs.z));
    }
}