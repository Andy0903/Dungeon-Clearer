using UnityEngine;
using System.Collections.Generic;

public class Node
{
    public Vector3Int GridPosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }  //int?
    public Node Parent { get; set; }
    public float Gcost { get; set; }
    public float Hcost { get; set; }
    public float Cost { get { return Gcost + Hcost; } }

    public Node(Vector3Int gridPos, Vector3 worldPos)
    {
        GridPosition = gridPos;
        WorldPosition = new Vector3(worldPos.x + 0.5f, worldPos.y + 0.5f, worldPos.z);
        Parent = null;
        Gcost = float.PositiveInfinity;
        Hcost = float.PositiveInfinity;
    }
}


public class PriorityQueue
{
    List<Node> heap;

    public PriorityQueue()
    {
        heap = new List<Node>();
    }

    /// <summary>
    /// Add node to priority queue.
    /// </summary>
    /// <param name="newNode"> The node to be inserted into into the PQ</param>
    public void Enqueue(Node newNode)
    {
        heap.Add(newNode);
        int i = heap.Count - 1;
        while (i != 0)
        {
            int p = (i - 1) / 2;
            if (heap[p].Cost > heap[i].Cost)
            {
                Swap(i, p);
                i = p;
            }
            else
                return;
        }
    }

    /// <summary>
    /// Remove the top node in the PQ
    /// </summary>
    /// <returns>Returns the node that was removed</returns>
    public Node Dequeue()
    {
        int i = heap.Count - 1;
        Node first = heap[0];
        heap[0] = heap[i];
        heap.RemoveAt(i);
        --i;
        int p = 0;

        while (true)
        {
            int left = p * 2 + 1;
            if (left > i)
                break;
            int right = left + 1;
            if ((right < p) && (heap[right].Cost < heap[left].Cost))
                left = right;
            if (heap[p].Cost <= heap[left].Cost)
                break;
            Swap(p, left);
            p = left;
        }
        return first;
    }
    
    /// <summary>
    /// Looks for a specific node in the queue.
    /// </summary>
    /// <param name="n">The specific node to look for</param>
    /// <returns>Whether a specific node is in the PQ or not.</returns>
    public bool Contains(Node n)
    {
        return heap.Contains(n);
    }
    
    /// <summary>
    /// Tells if the queue is empty or not.
    /// </summary>
    /// <returns>Whether or not the queue is empty.</returns>
    public bool isEmpty()
    {
        return heap.Count == 0;
    }
    
    /// <summary>
    /// Removes a specific node from the priority queue
    /// </summary>
    /// <param name="n">The node to be removed</param>
    public void Remove(Node n)
    {
        heap.Remove(n);
    }

    /// <summary>
    /// Swap places of two nodes (A and B)
    /// </summary>
    /// <param name="a">Index in the queue of node A</param>
    /// <param name="b">Index in the queue of node B</param>
    private void Swap(int a, int b)
    {
        Node temp = heap[a];
        heap[a] = heap[b];
        heap[b] = temp;
    }
}