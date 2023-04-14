using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovement : MonoBehaviour
{
    private readonly HexGrid _hexGrid = Object.FindObjectOfType<HexGrid>();   
    public HashSet<Vector3> hex_reachable(Vector3 start,int movement)
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
                    Vector3 neighbor = _hexGrid.HexNeighbor(_hexGrid.GetHexAt(hex).GetVectorCoordinates(), j);
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

    

}
