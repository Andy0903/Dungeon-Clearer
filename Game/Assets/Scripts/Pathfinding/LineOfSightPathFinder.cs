using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Pathfinder/LineOfSightPathFinder")]
public class LineOfSightPathFinder : PathFinder
{
    protected override List<Vector3> FindPathBehaviour(Vector3 from, Vector3 to)
    {
        return new List<Vector3>() { LineOfSight(from, to) ? to : from };
    }

    /// Uncheck: Edit -> Project Settings -> Physcis 2D -> Querries Start In Collider
    bool LineOfSight(Vector3 from, Vector3 to)
    {
        RaycastHit2D hit = Physics2D.Linecast(from, to, 1 << LayerMask.NameToLayer("Environment"));
        return hit.collider == null;
    }
}
