using System;
using Hex;
using UnityEngine;

namespace MapObjects
{
    /// <summary> ************************************************************
    /// This class handles the movement of units. It checks if the player
    /// selects a unit, then if they want to move it or attack with it. A
    /// player is only allowed to move their own units.
    /// </summary> ***********************************************************
    /// TODO: maybe transfer some of the combat related functions to the combat file
    public class HexMovement : MonoBehaviour
    {
        private HexGrid _hexGrid;
        private int _currentHexIndex;
        private int _goalHexIndex;
        private Hex.Hex _currentHex;
        private Hex.Hex _goalHex;
        private Unit _selectedUnit;
        private GameObject _selectedUnitObject;

        private int _playerID;
        private Camera _camera;
        
        /* Heuristic was needed for A* algorithm
     https://www.redblobgames.com/pathfinding/a-star/introduction.html#greedy-best-first
    */
        /*
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
    */
        /********
    This is the beginning of an A* search according to https://www.redblobgames.com/pathfinding/a-star/introduction.html#breadth-first-search
    may have made hex_reachable redundant, but we'll see
    Queue needs to be replaced by PriorityQueue, but not sure how to do that, this should work but not efficiently
    
        public List<Hex.Hex> pathFind(Vector3 start, Vector3 goal)
        {
            Utils.PriorityQueue<Vector3, int> frontier = new Utils.PriorityQueue<Vector3, int>(); 
            frontier.Enqueue(start, 0);

            Dictionary<Vector3, Vector3> came_from = new Dictionary<Vector3, Vector3>();
            Dictionary<Vector3, int> cost = new Dictionary<Vector3, int>();

            came_from[start] = start;
            cost[start] = 0;

            List<Hex.Hex> path = new List<Hex.Hex>();

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
        */

        // use WorldPosition in Hex to get actual position

        private void Start()
        {
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            _currentHexIndex = -1;
            _goalHexIndex = -1;
            _playerID = 1;
        }

        private void Update()
        {
            DetectClick();
        }

        /// <summary> ***********************************************
        /// This function handles user input and calls the necessary
        /// functions.
        /// </summary> **********************************************
        private void DetectClick()
        {
            // check if a unit is clicked
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("unit Left click detected in UnitMovement.cs");
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;
                if(!hit.transform.CompareTag("Unit")) return;

                _currentHexIndex = GetHexIndexAtWorldPos(hit.transform.position);

                return;
            }


            // check if a tile is clicked

            if (Input.GetMouseButtonDown(1) && _currentHexIndex != -1)
            {
                Debug.Log("Tile right click detected in UnitMovement.cs");
                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit)) return;
                Debug.Log("passed first if");

                _goalHexIndex = GetHexIndexAtWorldPos(hit.transform.position);
                if (_currentHexIndex < 0 || _goalHexIndex < 0) return;
                
                _currentHex = _hexGrid.GetHexList()[_currentHexIndex];
                _goalHex = _hexGrid.GetHexList()[_goalHexIndex];

                if (_hexGrid.GetUnitDictionary()[_currentHex].GetCurrentMovementPoints() <= 0)
                {
                    _currentHexIndex = -1;
                    _goalHexIndex = -1;
                    return;
                }
                Debug.Log("made it to comparetag");
                bool doDeplete = false;
                if (hit.transform.CompareTag("Unit"))
                {
                    if (_hexGrid.GetUnitDictionary()[_currentHex] == _hexGrid.GetUnitDictionary()[_goalHex]) return;
                    if (!IsTargetInRange(_hexGrid.GetUnitDictionary()[_currentHex].AttackRadius)) return;
                    
                    doDeplete = true;
                    // if target is still alive
                    if (DoCombat()) 
                    {
                        _hexGrid.GetUnitDictionary()[_currentHex].DepleteMovementPoints();
                        _currentHexIndex = -1;
                        _goalHexIndex = -1;
                        return;
                    }

                    if (_hexGrid.GetUnitDictionary()[_currentHex].GetUnitType() != Unit.UnitType.Melee)
                    {
                        _hexGrid.GetUnitDictionary()[_currentHex].DepleteMovementPoints();
                        _currentHexIndex = -1;
                        _goalHexIndex = -1;
                        return;
                    }
                }

                if (!SelectedTileIsNeighbor()) return;
                if (_goalHex.IsBlocked() && 
                    _hexGrid.GetUnitDictionary()[_currentHex].GetUnitType() != Unit.UnitType.Airship)
                    return;

                if (_hexGrid.GetUnitDictionary().ContainsKey(_currentHex))
                {
                    if (_hexGrid.GetUnitDictionary()[_currentHex].GetOwnerID() != _playerID)
                    {
                        _currentHexIndex = -1;
                        _goalHexIndex = -1;
                        return;
                    }
                }
                
                _selectedUnit = _hexGrid.GetUnitDictionary()[_currentHex];
                _selectedUnitObject = _hexGrid.GetUnitObjectDictionary()[_selectedUnit];
                
                if (_hexGrid.GetUnitDictionary().ContainsKey(_goalHex)) return;
                MoveUnit();
                
                if (doDeplete) _selectedUnit.DepleteMovementPoints();
                _currentHexIndex = -1;
                _goalHexIndex = -1;
            }
        }
        
        /// <summary> ***********************************************
        /// This function sets the current and goal Hex, then checks
        /// if the goal hex is a neighbor of current hex. If it is,
        /// then it will check if the goal hex is a blocked hex.
        /// If the hex is not blocked and goal hex is a neighbor
        /// return true, if hex is neighbor but is blocked or is not
        /// a neighbor, then return false.
        /// </summary> **********************************************
        private bool SelectedTileIsNeighbor()
        {
            for (int j = 0; j < 6; j++)
            {
                Vector3 neighbor = HexGrid.HexNeighbor(_currentHex.GetVectorCoordinates(), j);
                if (neighbor == _goalHex.GetVectorCoordinates())
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary> ***********************************************
        /// This function compares the distance from attacker to
        /// the target. Returns true if target is in range.
        /// </summary> **********************************************
        private bool IsTargetInRange(int range)
        {
            Vector3 resultVector = _currentHex.GetVectorCoordinates() - _goalHex.GetVectorCoordinates();
            int distance = (int)(Math.Abs(resultVector.x) + Math.Abs(resultVector.y) + Math.Abs(resultVector.z)) / 2;

            return distance <= range;
        }

        /// <summary> ***********************************************
        /// This function takes in a Vector3 of world coordinates
        /// and returns the Hex at that position.
        /// </summary> **********************************************
        private int GetHexIndexAtWorldPos(Vector3 coordinates)
        {
            int hexIndex = -1;
            for (int i = 0; i < _hexGrid.GetHexList().Count; i++)
            {
                if (_hexGrid.GetHexList()[i].WorldPosition.x == coordinates.x &&
                    _hexGrid.GetHexList()[i].WorldPosition.z == coordinates.z)
                {
                    hexIndex = i;
                    break;
                }
            }
            return hexIndex;
        }

        /// <summary> ***********************************************
        /// This function updates the Units' coordinates and updates
        /// the dictionary that holds the relation between the Hex
        /// and the Unit.
        /// </summary> **********************************************
        private void MoveUnit()
        {
            _selectedUnit.Q = _goalHex.Q;
            _selectedUnit.R = _goalHex.R;
            _selectedUnit.S = _goalHex.S;

            Vector3 goalVector = _goalHex.WorldPosition; 
            Vector3 vectorToChange = new Vector3(goalVector.x, _goalHex.WorldPosition.y + 1.3f, goalVector.z);
            _selectedUnitObject.transform.position = vectorToChange;
        
            _hexGrid.GetUnitDictionary().Remove(_currentHex);
            _hexGrid.GetUnitDictionary().Add(_goalHex, _selectedUnit);
            _selectedUnit.UseMovementPoints();
        }

        /// <summary> ***********************************************
        /// This function sets the playerID based on whose turn it
        /// currently is. 
        /// </summary> **********************************************
        public void SetPlayer(int currentPlayer)
        {
            _playerID = currentPlayer;
        }

        private bool DoCombat()
        {
            if (!_hexGrid.GetUnitDictionary().ContainsKey(_currentHex) ||
                !_hexGrid.GetUnitDictionary().ContainsKey(_goalHex))
                return true;

            if (_hexGrid.GetUnitDictionary()[_currentHex].GetOwnerID() != _playerID) return true;

            bool dead = Combat.InitiateCombat(_hexGrid.GetUnitDictionary()[_currentHex],
                _hexGrid.GetUnitDictionary()[_goalHex]);
            if (!dead) return true;

            Destroy(_hexGrid.GetUnitObjectDictionary()[_hexGrid.GetUnitDictionary()[_goalHex]]);
            _hexGrid.GetUnitObjectDictionary().Remove(_hexGrid.GetUnitDictionary()[_goalHex]);
            _hexGrid.GetUnitDictionary().Remove(_goalHex);
            
            return false;
        }

        /// <summary> ***********************************************
        /// This function sets all unit movement points back to their
        /// base values.
        /// </summary> **********************************************
        public void ResetUnitMovementPoints()
        {
            foreach (Unit unit in _hexGrid.GetUnitDictionary().Values)
            {
                unit.ResetMovementPoints();
            }
        }
    }
}
