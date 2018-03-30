using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Pathfinder/AirBased")]
public class AirBasedPathFinder : PathFinder
{
    protected override List<Vector3> FindPathBehaviour(Vector3 from, Vector3 to)
    {
        return new List<Vector3>() { to };
    }
}
