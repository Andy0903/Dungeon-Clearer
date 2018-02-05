using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Pathfinder/GroundBased")]
public class GroundBasedPathFinder : PathFinder
{
    HashSet<Node> closed;

    public override List<Vector3> FindPath(Vector3 from, Vector3 to)
    {
        Node start;
        Node goal;
        WorldNode.TryGetValue(from.ToVector3IntOnGrid(), out start);
        WorldNode.TryGetValue(to.ToVector3IntOnGrid(), out goal);

        PriorityQueue open = new PriorityQueue();
        closed = new HashSet<Node>();
        start.Gcost = 0;
        start.Parent = start;
        start.Hcost = 1.1f * Pythagoras(start, goal);

        open.Enqueue(start);

        while (open.isEmpty() == false)
        {
            Node current = open.Dequeue();
            SetVertex(current, closed);

            if (current == goal)
            {
                List<Vector3> result = new List<Vector3>();
                while (current != start)
                {
                    result.Add(current.WorldPosition);
                    current = current.Parent;
                }
                result[0] = to;
                result.Reverse();
                return result;
            }

            closed.Add(current);
            foreach (Node neighbor in GetNeighbors(current))
            {
                if (LineOfSight(current, neighbor))
                {
                    if (closed.Contains(neighbor) == false)
                    {
                        if (open.Contains(neighbor) == false)
                        {
                            neighbor.Gcost = float.PositiveInfinity;
                            neighbor.Parent = null;
                            neighbor.Hcost = 1.1f * Pythagoras(neighbor, goal);
                            // GameObject.Instantiate(visited, neighbor.WorldPosition, Quaternion.identity);
                        }

                        UpdateVertex(current, neighbor, open);
                    }
                }
            }
        }

        return null;
    }

    void SetVertex(Node node, HashSet<Node> closed)
    {
        if (!LineOfSight(node.Parent, node))
        {
            node.Gcost = float.PositiveInfinity;
            node.Parent = null;
            //Path 1
            foreach (Node s2 in GetNeighbors(node))
            {
                if (closed.Contains(s2))
                {
                    if (LineOfSight(node, s2))
                    {
                        float distance = s2.Gcost + Pythagoras(s2, node);
                        if (distance < node.Gcost)
                        {
                            node.Gcost = distance;
                            node.Parent = s2;
                        }
                    }
                }
            }
        }
    }

    /// Uncheck: Edit -> Project Settings -> Physcis 2D -> Querries Start In Collider
    bool LineOfSight(Node from, Node to)
    {
        RaycastHit2D hit = Physics2D.Linecast(from.WorldPosition, to.WorldPosition);
        return hit.collider == null;
    }

    void UpdateVertex(Node from, Node to, PriorityQueue open)
    {
        float oldGCost = to.Gcost;
        ComputeCost(from, to);

        if (to.Gcost < oldGCost)
        {
            if (open.Contains(to))
            {
                open.Remove(to);
            }

            open.Enqueue(to);
        }
    }

    float Pythagoras(Node from, Node to)
    {
        return Vector3Int.Distance(from.GridPosition, to.GridPosition);
    }

    void ComputeCost(Node from, Node to)
    {
        //Path 2
        if (((from.Parent.Gcost + Pythagoras(from.Parent, to)) < to.Gcost))
        {
            to.Parent = from.Parent;
            to.Gcost = from.Parent.Gcost + Pythagoras(from.Parent, to);
        }
    }

    private List<Node> GetNeighbors(Node from)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int fromX = Mathf.FloorToInt(from.WorldPosition.x);
                int fromY = Mathf.FloorToInt(from.WorldPosition.y);

                int neighborX = fromX + i;
                int neighborY = fromY + j;

                Vector3Int neighborPos = new Vector3Int(neighborX, neighborY, 0);

                Node neighbor;
                WorldNode.TryGetValue(neighborPos, out neighbor);

                if (neighbor != null)
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
