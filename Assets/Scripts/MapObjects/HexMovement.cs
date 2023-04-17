using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Unit = MapObjects.Unit;

public class HexMovement : MonoBehaviour
{
    private HexGrid _hexGrid;
    public int _currentHexIndex;
    public int _goalHexIndex;
    private Hex _currentHex;
    private Hex _goalHex;
    private Unit _selectedUnit;
    private GameObject _selectedUnitObject;

    /* Heuristic was needed for A* algorithm
     https://www.redblobgames.com/pathfinding/a-star/introduction.html#greedy-best-first
    */
    private float Heuristic(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x ) + Mathf.Abs(a.y - b.y);
    }

    public Queue<Vector3> hex_reachable(Vector3 start, int movement)
    {
        Queue<Vector3> visited = new Queue<Vector3>();
        visited.Enqueue(start);

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
                        visited.Enqueue(neighbor);
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
       Utils.PriorityQueue<Vector3, int> frontier = new Utils.PriorityQueue<Vector3, int>(); 
       frontier.Enqueue(start, 0);

       Dictionary<Vector3, Vector3> came_from = new Dictionary<Vector3, Vector3>();
       Dictionary<Vector3, int> cost = new Dictionary<Vector3, int>();

       came_from[start] = start;
       cost[start] = 0;

       List<Hex> path = new List<Hex>();

       while(frontier.Count > 0)
       {
            Vector3 current = frontier.Dequeue();

            if(current == goal)
            {
                break;
            }
            // There should be a foreach, but couldn't figure it out.
            List<Vector3> neighborList = new List<Vector3>(); 
            for (int j = 0; j < 6; j++)
            {
                Vector3 neighbor = HexGrid.HexNeighbor(_hexGrid.GetHexAt(current).GetVectorCoordinates(), j);
                neighborList.Add(neighbor);     
            }
            foreach(Vector3 next in neighborList )
            {
                int blockCost = 0;
                int newCost = 0;
                int priority = 0;
                if(!_hexGrid.GetHexAt(next).IsBlocked())
                {
                    blockCost = 1;
                }
                newCost = cost[current] + blockCost;
                if(!cost.ContainsKey(next) || newCost < cost[next])
                {
                    cost[next] = newCost;
                    priority = (int)(newCost + Heuristic(goal, next));
                    frontier.Enqueue(next, priority);
                    came_from[next] = current;
                }

            }
            
       }
       return path;

    }

    // use WorldPosition in Hex to get actual positon

    private void Start()
    {
        _hexGrid = Object.FindObjectOfType<HexGrid>();
    }

    private void Update()
    {
        // check if a unit is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            if(!hit.transform.CompareTag("Unit")) return;

            _currentHexIndex = GetHexIndexAtWorldPos(hit.transform.position);
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            Debug.Log("Here");
            _goalHexIndex = GetHexIndexAtWorldPos(hit.transform.position);
            
            if (!SelectedTileIsNeighbor(_currentHexIndex, _goalHexIndex)) return;
            
            _selectedUnit = _hexGrid.GetUnitDictionary()[_currentHex];
            _selectedUnitObject = _hexGrid.GetUnitObjectDictionary()[_selectedUnit];
        
        
            MoveUnit();
        }
    }

    private bool SelectedTileIsNeighbor(int currentHexIndex, int goalHexIndex)
    {
        _currentHex = _hexGrid.GetHexList()[currentHexIndex];
        _goalHex = _hexGrid.GetHexList()[goalHexIndex];
        
        for (int j = 0; j < 6; j++)
        {
            Vector3 neighbor = HexGrid.HexNeighbor(_currentHex.GetVectorCoordinates(), j);
            if (neighbor == _goalHex.GetVectorCoordinates())
            {
                return !_goalHex.IsBlocked();
            }
        }

        return false;
    }

    /// <summary> ***********************************************
    /// This function takes in a Vector3 of world coordinates
    /// and returns the Hex at that position.
    /// </summary> **********************************************
    private int GetHexIndexAtWorldPos(Vector3 coordinates)
    {
        Debug.Log(coordinates);
        int hexIndex = -1;
        for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
        {
            Debug.Log(_hexGrid.GetHexList()[i].WorldPosition);
            if (_hexGrid.GetHexList()[i].WorldPosition.x == coordinates.x &&
                _hexGrid.GetHexList()[i].WorldPosition.z == coordinates.z)
            {
                hexIndex = i;
                break;
            }
        }
        return hexIndex;
    }

    private void MoveUnit()
    {
        // unit q, r, s = goal q, r, s
        _selectedUnit.Q = _goalHex.Q;
        _selectedUnit.R = _goalHex.R;
        _selectedUnit.S = _goalHex.S;

        Vector3 goalVector = _goalHex.WorldPosition; 
        Vector3 vectorToChange = new Vector3(goalVector.x, 1, goalVector.z);
        _selectedUnitObject.transform.position = vectorToChange;
    }
}
