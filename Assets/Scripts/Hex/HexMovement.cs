using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovement : MonoBehaviour
{
    public readonly HexGrid _hexGrid = Object.FindObjectOfType<HexGrid>(); 

    /* Heuristic was needed for A* algorithm
     https://www.redblobgames.com/pathfinding/a-star/introduction.html#greedy-best-first
    */
    private float Heuristic(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x ) + Mathf.Abs(a.y - b.y);
    }

    public HashSet<Vector3> hex_reachable(Vector3 start, int movement)
    {
        HashSet<Vector3> visited = new HashSet<Vector3>();
        visited.Add(start);

        List<List<Vector3>> fringes = new List<List<Vector3>>();
        fringes.Add(new List<Vector3> { start });

        for (int i = 1; i <= movement; i++)
        {
            fringes.Add(new List<Vector3>());
            foreach(Vector3 hex in fringes[i-1])
            {
                for (int j = 0; j < 6; j++)
                {
                    Vector3 neighbor = HexGrid.HexNeighbor(_hexGrid.GetHexAt(hex).GetVectorCoordinates(), j);
                    if (!visited.Contains(neighbor) && _hexGrid.GetHexAt(hex).IsBlocked())
                    {  
                        visited.Add(neighbor);
                        fringes[i].Add(neighbor);
                    }
                }
            }
            
        }
        return visited; 
    }

    /********
    This is the beginning of an A* search according to https://www.redblobgames.com/pathfinding/a-star/introduction.html#breadth-first-search
    may have made hex_reachable redundant, but we'll see
    Queue needs to be replaced by PriorityQueue, but not sure how to do that, this should work but not efficiently
    */
    public List<Hex> pathFind(Vector3 start, Vector3 goal)
    {
       Queue<Vector3> frontier = new Queue<Vector3>(); 
       frontier.Enqueue(start);

       Dictionary<Vector3, float> came_from = new Dictionary<Vector3, float>();
       Dictionary<Vector3, Vector3> cost = new Dictionary<Vector3, Vector3>();

       came_from[start] = 0;
       cost[start] = start;

       List<Hex> path = new List<Hex>();

       while(frontier.Count > 0)
       {
            Vector3 current = frontier.Dequeue();

            if(current == goal)
            {
                break;
            }
            // There should be a foreach, but couldn't figure it out.
            
       }
       return path;

    }

    // use WorldPosition in Hex to get actual positon

}
