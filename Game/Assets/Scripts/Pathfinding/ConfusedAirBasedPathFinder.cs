using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Pathfinder/ConfusedAirBasedPathFinder")]
public class ConfusedAirBasedPathFinder : PathFinder
{
    protected override List<Vector3> FindPathBehaviour(Vector3 from, Vector3 to)
    {
        List<Vector3> path = new List<Vector3>();
        float value = Random.value;
        Vector3 offset = Random.insideUnitCircle * 5;

        if (value >= 0.5f)
            return new List<Vector3> { to - offset };

        return new List<Vector3> { to + offset };
    }
}
