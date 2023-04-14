using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovement : MonoBehaviour
{
    public HashSet<Hex> hex_reachable(Hex start,int movement)
    {
        HashSet<Hex> visited = new HashSet<Hex>();
        visited.Add(start);

        List<List<Hex>> fringes = new List<List<Hex>>();
        fringes.Add(new List<Hex> { start });

        for (int i = 1; i <= movement; i++)
        {
            fringes.Add(new List<Hex>());
            foreach(Hex hex in fringes[i-1])
            {
                for (int j = 0; j < 6; j++)
                {
                    // Hex neighbor = HexNeighbor(hex, j);
                    // if (!visited.Contains(neighbor) && !blocked)
                    // {
                    //     visited.Add(neighbor);
                    //     fringes[i].Add(neighbor);
                    // }
                }
            }
            
        }
        return visited;
    }

    

}
